using ServiceBusManager.Server.Providers.Common;

namespace ServiceBusManager.Server.Providers.Azure.Models
{
    public class AzureServiceBusQueueDetails : ServiceBusQueueDetails
    {
        public AzureServiceBusQueueDetails(
            string name,
            ServiceBusQueueStatus status,
            TimeSpan autoDeleteOnIdle,
            TimeSpan defaultMessageTimeToLive,
            TimeSpan lockDuration,
            TimeSpan duplicateDetectionHistoryTimeWindow,
            long maxSizeInMegabytes,
            int maxDeliveryCount,
            bool enableBatchedOperations,
            bool enablePartitioning,
            bool requireSession,
            bool requireDuplicateDetection)
            : base(name, status)
        {
            MessageSettings = new AzureServiceBusQueueMessageSettings(
                    autoDeleteOnIdle,
                    defaultMessageTimeToLive,
                    lockDuration);

            Properties = new AzureServiceBusQueueProperties(
                duplicateDetectionHistoryTimeWindow,
                maxSizeInMegabytes,
                maxDeliveryCount);

            Settings = new AzureServiceBusQueueSettings(
                enableBatchedOperations,
                enablePartitioning,
                requireSession,
                requireDuplicateDetection);
        }

        public AzureServiceBusQueueDetails(
            string name,
            TimeSpan autoDeleteOnIdle,
            TimeSpan defaultMessageTimeToLive,
            TimeSpan lockDuration,
            TimeSpan duplicateDetectionHistoryTimeWindow,
            long maxSizeInMegabytes,
            int maxDeliveryCount,
            bool enableBatchedOperations,
            bool enablePartitioning,
            bool requireSession,
            bool requireDuplicateDetection)
            : base(name)
        {
            MessageSettings = new AzureServiceBusQueueMessageSettings(
                autoDeleteOnIdle,
                defaultMessageTimeToLive,
                lockDuration);

            Properties = new AzureServiceBusQueueProperties(
                duplicateDetectionHistoryTimeWindow,
                maxSizeInMegabytes,
                maxDeliveryCount);

            Settings = new AzureServiceBusQueueSettings(
                enableBatchedOperations,
                enablePartitioning,
                requireSession,
                requireDuplicateDetection);
        }

        public AzureServiceBusQueueMessageSettings MessageSettings { get; }

        public AzureServiceBusQueueProperties Properties { get; }

        public AzureServiceBusQueueSettings Settings { get; }
    }

    public class AzureServiceBusQueueMessageSettings
    {
        public AzureServiceBusQueueMessageSettings(
            TimeSpan autoDeleteOnIdle,
            TimeSpan defaultMessageTimeToLive,
            TimeSpan lockDuration)
        {
            AutoDeleteOnIdle = autoDeleteOnIdle;
            DefaultMessageTimeToLive = defaultMessageTimeToLive;
            LockDuration = lockDuration;
        }

        public TimeSpan AutoDeleteOnIdle { get; }

        public TimeSpan DefaultMessageTimeToLive { get; }

        public TimeSpan LockDuration { get; }
    }

    public class AzureServiceBusQueueProperties
    {
        public AzureServiceBusQueueProperties(
            TimeSpan duplicateDetectionHistoryTimeWindow,
            long maxSizeInMegabytes,
            int maxDeliveryCount)
        {
            DuplicateDetectionHistoryTimeWindow = duplicateDetectionHistoryTimeWindow;
            MaxSizeInMegabytes = maxSizeInMegabytes;
            MaxDeliveryCount = maxDeliveryCount;
        }

        public TimeSpan DuplicateDetectionHistoryTimeWindow { get; }

        public long MaxSizeInMegabytes { get; }

        public int MaxDeliveryCount { get; }
    }

    public class AzureServiceBusQueueSettings
    {
        public AzureServiceBusQueueSettings(
            bool enableBatchedOperations,
            bool enablePartitioning,
            bool requireSession,
            bool requireDuplicateDetection)
        {
            EnableBatchedOperations = enableBatchedOperations;
            EnablePartitioning = enablePartitioning;
            RequireSession = requireSession;
            RequireDuplicateDetection = requireDuplicateDetection;
        }

        public bool EnableBatchedOperations { get; }

        public bool EnablePartitioning { get; }

        public bool RequireSession { get; }

        public bool RequireDuplicateDetection { get; }
    }
}
