using AutoMapper;
using ServiceBusManager.Server.Infrastructure;

namespace ServiceBusManager.Server.Application.Queries
{
    internal class ServiceBusQueries
        : IServiceBusQueries
    {
        private readonly IServiceBusProvider _serviceBusProvider;
        private readonly IMapper _mapper;

        public ServiceBusQueries(
            IServiceBusProvider serviceBusProvider,
            IMapper mapper)
        {
            _serviceBusProvider = serviceBusProvider;
            _mapper = mapper;
        }

        public async Task<QueueGetAllResponse> GetAllQueuesAsync(CancellationToken cancellationToken = default)
        {
            IReadOnlyCollection<ServiceBusQueue> queues = await _serviceBusProvider.GetQueuesAsync(cancellationToken);

            IEnumerable<QueueGetResponse> dtos = queues.Select(_mapper.Map<QueueGetResponse>);

            return new QueueGetAllResponse(dtos);
        }

        public async Task<QueueGetDetailsResponse> GetQueueDetailsAsync(string name, CancellationToken cancellationToken = default)
        {
            ServiceBusQueueDetails details = await _serviceBusProvider.GetQueueDetailsAsync(name, cancellationToken);

            QueueGetDetailsResponse dto = _mapper.Map<QueueGetDetailsResponse>(details);
            return dto;
        }
    }
}
