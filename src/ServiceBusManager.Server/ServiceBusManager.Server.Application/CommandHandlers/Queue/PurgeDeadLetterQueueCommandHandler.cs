using MediatR;
using ServiceBusManager.Server.Application.Commands;
using ServiceBusManager.Server.Infrastructure;

namespace ServiceBusManager.Server.Application.CommandHandlers
{
    internal class PurgeDeadLetterQueueCommandHandler : IRequestHandler<PurgeDeadLetterQueueCommand>
    {
        private readonly IServiceBusProvider _serviceBusProvider;

        public PurgeDeadLetterQueueCommandHandler(IServiceBusProvider serviceBusProvider)
        {
            _serviceBusProvider = serviceBusProvider;
        }

        public async Task<Unit> Handle(PurgeDeadLetterQueueCommand request, CancellationToken cancellationToken)
        {
            await _serviceBusProvider.PurgeDeadLetterQueueAsync(request.Name, cancellationToken);

            return Unit.Value;
        }
    }
}
