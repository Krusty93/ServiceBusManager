using Azure;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Logging;

namespace ServiceBusManager.Server.Infrastructure.AzureServiceBus
{
    internal class AzureServiceBusProvider : IServiceBusProvider
    {
        private readonly ServiceBusAdministrationClient _client;
        private readonly ILogger<AzureServiceBusProvider> _logger;

        public AzureServiceBusProvider(
            ServiceBusAdministrationClient client,
            ILogger<AzureServiceBusProvider> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<IReadOnlyCollection<ServiceBusQueue>> GetQueuesAsync(CancellationToken cancellationToken = default)
        {
            var results = new List<ServiceBusQueue>();

            AsyncPageable<QueueRuntimeProperties> queues = _client.GetQueuesRuntimePropertiesAsync(cancellationToken);

            await foreach (QueueRuntimeProperties queue in queues)
            {
                var item = new ServiceBusQueue(
                    name: queue.Name,
                    activeCount: queue.ActiveMessageCount,
                    deadLetterCount: queue.DeadLetterMessageCount);

                results.Add(item);
            }

            return results;
        }

        public async Task<ServiceBusQueueDetails> GetQueueDetailsAsync(string name, CancellationToken cancellationToken)
        {
            QueueProperties details = await _client.GetQueueAsync(name, cancellationToken);

            if (!Enum.TryParse(details.Status.ToString(), true, out ServiceBusQueueStatus status))
            {
                _logger.LogWarning("Unknown queue status value: {value}", details.Status.ToString());
            }

            var queueDetails = new ServiceBusQueueDetails(
                name: details.Name,
                status: status,
                autoDeleteOnIdle: details.AutoDeleteOnIdle,
                defaultMessageTimeToLive: details.DefaultMessageTimeToLive,
                lockDuration: details.LockDuration,
                duplicateDetectionHistoryTimeWindow: details.DuplicateDetectionHistoryTimeWindow,
                maxSizeInMegabytes: details.MaxSizeInMegabytes,
                maxDeliveryCount: details.MaxDeliveryCount,
                enableBatchedOperations: details.EnableBatchedOperations,
                enablePartitioning: details.EnablePartitioning,
                requireSession: details.RequiresSession,
                requireDuplicateDetection: details.RequiresDuplicateDetection);

            return queueDetails;
        }
    }
}
