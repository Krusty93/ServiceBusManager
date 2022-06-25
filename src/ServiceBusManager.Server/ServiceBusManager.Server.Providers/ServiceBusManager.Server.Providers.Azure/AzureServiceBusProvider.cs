using Azure;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.Extensions.Logging;
using ServiceBusManager.Server.Providers.Azure.Models;
using ServiceBusManager.Server.Providers.Common;

namespace ServiceBusManager.Server.Providers.Azure
{
    public class AzureServiceBusProvider : IServiceBusProvider
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
                var item = new AzureServiceBusQueue(
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

            var queueDetails = new AzureServiceBusQueueDetails(
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

        public async Task GetQueueActiveMessagesAsync(string name, CancellationToken cancellationToken = default)
        {
            const int MAX_COUNT = 50;

            var receiver = GetReceiver(name);
            _ = await receiver.PeekMessagesAsync(MAX_COUNT, cancellationToken: cancellationToken);
        }

        public async Task DeleteQueueAsync(string name, CancellationToken cancellationToken = default)
        {
            await _adminClient.DeleteQueueAsync(name, cancellationToken);
        }

        public async Task CreateQueueAsync(string name, ServiceBusQueueDetails details, CancellationToken cancellationToken = default)
        {
            if (details is not AzureServiceBusQueueDetails azureDetails)
                throw new InvalidOperationException();

            var options = new CreateQueueOptions(name);

            options.LockDuration = azureDetails.MessageSettings.LockDuration;
            options.AutoDeleteOnIdle = azureDetails.MessageSettings.AutoDeleteOnIdle;
            options.DefaultMessageTimeToLive = azureDetails.MessageSettings.DefaultMessageTimeToLive;
            options.DuplicateDetectionHistoryTimeWindow = azureDetails.Properties.DuplicateDetectionHistoryTimeWindow;
            options.MaxDeliveryCount = azureDetails.Properties.MaxDeliveryCount;
            options.MaxSizeInMegabytes = azureDetails.Properties.MaxSizeInMegabytes;
            options.EnableBatchedOperations = azureDetails.Settings.EnableBatchedOperations;
            options.RequiresDuplicateDetection = azureDetails.Settings.RequireDuplicateDetection;
            options.EnablePartitioning = azureDetails.Settings.EnablePartitioning;
            options.RequiresSession = azureDetails.Settings.RequireSession;

            await _adminClient.CreateQueueAsync(options, cancellationToken);
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

        public async Task PurgeDeadLetterQueueAsync(string name, CancellationToken cancellationToken = default)
        {
            ServiceBusReceiver receiver = GetDeadLetterReceiver(name);

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

        private ServiceBusReceiver GetDeadLetterReceiver(string name) =>
            _client.CreateReceiver(name, new ServiceBusReceiverOptions
            {
                SubQueue = SubQueue.DeadLetter
            });
    }
}
