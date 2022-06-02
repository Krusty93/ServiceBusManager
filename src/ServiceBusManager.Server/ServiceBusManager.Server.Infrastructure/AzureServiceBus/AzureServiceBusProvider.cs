using Azure;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Logging;

namespace ServiceBusManager.Server.Infrastructure.AzureServiceBus
{
    internal class AzureServiceBusProvider : IServiceBusProvider
    {
        private readonly ServiceBusAdministrationClient _adminClient;
        private readonly ServiceBusClient _client;
        private readonly ILogger<AzureServiceBusProvider> _logger;

        public AzureServiceBusProvider(
            ServiceBusAdministrationClient adminClient,
            ServiceBusClient client,
            ILogger<AzureServiceBusProvider> logger)
        {
            _adminClient = adminClient;
            _client = client;
            _logger = logger;
        }

        public async Task<IReadOnlyCollection<ServiceBusQueue>> GetQueuesAsync(CancellationToken cancellationToken = default)
        {
            var results = new List<ServiceBusQueue>();

            AsyncPageable<QueueRuntimeProperties> queues = _adminClient.GetQueuesRuntimePropertiesAsync(cancellationToken);

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
            QueueProperties details = await _adminClient.GetQueueAsync(name, cancellationToken);

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

        public async Task DeleteQueueAsync(string name, CancellationToken cancellationToken = default)
        {
            await _adminClient.DeleteQueueAsync(name, cancellationToken);
        }

        public async Task CreateQueueAsync(string name, ServiceBusQueueDetails details, CancellationToken cancellationToken = default)
        {
            var options = new CreateQueueOptions(name);

            options.LockDuration = details.MessageSettings.LockDuration;
            options.AutoDeleteOnIdle = details.MessageSettings.AutoDeleteOnIdle;
            options.DefaultMessageTimeToLive = details.MessageSettings.DefaultMessageTimeToLive;
            options.DuplicateDetectionHistoryTimeWindow = details.Properties.DuplicateDetectionHistoryTimeWindow;
            options.MaxDeliveryCount = details.Properties.MaxDeliveryCount;
            options.MaxSizeInMegabytes = details.Properties.MaxSizeInMegabytes;
            options.EnableBatchedOperations = details.Settings.EnableBatchedOperations;
            options.RequiresDuplicateDetection = details.Settings.RequireDuplicateDetection;
            options.EnablePartitioning = details.Settings.EnablePartitioning;
            options.RequiresSession = details.Settings.RequireSession;

            await _adminClient.CreateQueueAsync(options);
        }

        public async Task PurgeActiveQueueAsync(string name, CancellationToken cancellationToken = default)
        {
            ServiceBusReceiver receiver = GetReceiver(name);

            IReadOnlyList<ServiceBusReceivedMessage> messages;

            bool isLastIteration = false;

            const int MAX_COUNT = 50;

            do
            {
                messages = await receiver.ReceiveMessagesAsync(
                    MAX_COUNT,
                    maxWaitTime: TimeSpan.FromSeconds(30),
                    cancellationToken: cancellationToken);

                if (messages.Count < MAX_COUNT)
                    isLastIteration = true;

                foreach (ServiceBusReceivedMessage message in messages)
                {
                    await receiver.CompleteMessageAsync(message, cancellationToken);
                }
            }
            while (!isLastIteration);
        }

        private ServiceBusReceiver GetReceiver(string name) =>
            _client.CreateReceiver(name);
    }
}
