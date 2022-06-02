using MediatR;
using ServiceBusManager.Server.Application.Commands;
using ServiceBusManager.Server.Infrastructure;

namespace ServiceBusManager.Server.Application.CommandHandlers
{
    internal class PurgeActiveQueueCommandHandler : IRequestHandler<PurgeActiveQueueCommand>
    {
        private readonly IServiceBusProvider _serviceBusProvider;

        public PurgeActiveQueueCommandHandler(IServiceBusProvider serviceBusProvider)
        {
            _serviceBusProvider = serviceBusProvider;
        }

        public async Task<Unit> Handle(PurgeActiveQueueCommand request, CancellationToken cancellationToken)
        {
            await _serviceBusProvider.PurgeActiveQueueAsync(request.Name, cancellationToken);

            return Unit.Value;
        }
    }
}
