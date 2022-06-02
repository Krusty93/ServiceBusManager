namespace ServiceBusManager.Server.Infrastructure
{
    public class ServiceBusQueueDetails
    {
        public ServiceBusQueueDetails(
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
        {
            Name = name;
            Status = status;

            MessageSettings = new ServiceBusQueueMessageSettings(
                autoDeleteOnIdle,
                defaultMessageTimeToLive,
                lockDuration);

            Properties = new ServiceBusQueueProperties(
                duplicateDetectionHistoryTimeWindow,
                maxSizeInMegabytes,
                maxDeliveryCount);

            Settings = new ServiceBusQueueSettings(
                enableBatchedOperations,
                enablePartitioning,
                requireSession,
                requireDuplicateDetection);
        }

        public string Name { get; }

        public ServiceBusQueueStatus Status { get; }

        public ServiceBusQueueMessageSettings MessageSettings { get; }

        public ServiceBusQueueProperties Properties { get; }

        public ServiceBusQueueSettings Settings { get; }
    }

    public class ServiceBusQueueMessageSettings
    {
        public ServiceBusQueueMessageSettings(
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

    public class ServiceBusQueueProperties
    {
        public ServiceBusQueueProperties(
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

    public class ServiceBusQueueSettings
    {
        public ServiceBusQueueSettings(
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
