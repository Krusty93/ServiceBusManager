using MediatR;
using ServiceBusManager.Server.Application.Commands.Azure;
using ServiceBusManager.Server.Providers.Azure.Models;
using ServiceBusManager.Server.Providers.Common;

namespace ServiceBusManager.Server.Application.CommandHandlers.Azure
{
    internal class CreateAzureQueueCommandHandler : IRequestHandler<CreateAzureQueueCommand>
    {
        private readonly IServiceBusProvider _serviceBusProvider;

        public CreateAzureQueueCommandHandler(IServiceBusProvider serviceBusProvider)
        {
            _serviceBusProvider = serviceBusProvider;
        }

        public async Task<Unit> Handle(CreateAzureQueueCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException(nameof(request.Name));

            var details = new AzureServiceBusQueueDetails(
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
