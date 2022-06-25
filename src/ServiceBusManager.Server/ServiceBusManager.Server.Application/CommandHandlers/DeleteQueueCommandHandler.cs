using MediatR;
using ServiceBusManager.Server.Application.Commands;
using ServiceBusManager.Server.Providers.Common;

namespace ServiceBusManager.Server.Application.CommandHandlers
{
    internal class DeleteQueueCommandHandler : IRequestHandler<DeleteQueueCommand>
    {
        private readonly IServiceBusProvider _serviceBusProvider;

        public DeleteQueueCommandHandler(IServiceBusProvider serviceBusProvider)
        {
            _serviceBusProvider = serviceBusProvider;
        }

        public async Task<Unit> Handle(DeleteQueueCommand request, CancellationToken cancellationToken)
        {
            if (request is null)
                throw new ArgumentNullException(nameof(request));

            await _serviceBusProvider.DeleteQueueAsync(request.Name, cancellationToken);

            return Unit.Value;
        }
    }
}
