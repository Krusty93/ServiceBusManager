using MediatR;
using ServiceBusManager.Server.Application.Commands;
using ServiceBusManager.Server.Infrastructure;

namespace ServiceBusManager.Server.Application.CommandHandlers
{
    internal class CreateQueueCommandHandler : IRequestHandler<CreateQueueCommand>
    {
        private readonly IServiceBusProvider _serviceBusProvider;

        public CreateQueueCommandHandler(IServiceBusProvider serviceBusProvider)
        {
            _serviceBusProvider = serviceBusProvider;
        }

        public async Task<Unit> Handle(CreateQueueCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException(nameof(request.Name));

            var details = new ServiceBusQueueDetails(
                request.Name,
                request.AutoDeleteOnIdle,
                request.DefaultMessageTimeToLive,
                request.LockDuration,
                request.DuplicateDetectionHistoryTimeWindow,
                request.MaxSizeInMegabytes,
                request.MaxDeliveryCount,
                request.EnableBatchedOperations,
                request.EnablePartitioning,
                request.RequireSession,
                request.RequireDuplicateDetection);

            await _serviceBusProvider.CreateQueueAsync(request.Name, details, cancellationToken);

            return Unit.Value;
        }
    }
}
