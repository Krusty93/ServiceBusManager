using System.Text.Json.Serialization;

namespace ServiceBusManager.Server.Application.Queries
{
    public interface IServiceBusQueries
    {
        Task<QueueGetAllResponse> GetAllQueuesAsync(CancellationToken cancellationToken = default);

        Task<QueueGetDetailsResponse> GetQueueDetailsAsync(string name, CancellationToken cancellationToken = default);
    }

    public class QueueGetAllResponse
    {
        public QueueGetAllResponse(IEnumerable<QueueGetResponse> queues)
        {
            Queues = new List<QueueGetResponse>(queues).AsReadOnly();
        }

        [JsonPropertyName("queues")]
        public IReadOnlyCollection<QueueGetResponse> Queues { get; }
    }

    public record QueueGetResponse
    {
        [JsonPropertyName("name")]
        public string? Name { get; init; }

        [JsonPropertyName("activeCount")]
        public long ActiveCount { get; init; }

        [JsonPropertyName("deadLetterCount")]
        public long DeadLetterCount { get; init; }
    }

    public record QueueGetDetailsResponse
    {
        [JsonPropertyName("name")]
        public string? Name { get; init; }

        [JsonPropertyName("status")]
        public ServiceBusQueueStatusResponse Status { get; }

        [JsonPropertyName("autoDeleteOnIdle")]
        public TimeSpan AutoDeleteOnIdle { get; init; }

        [JsonPropertyName("defaultMessageTimeToLive")]
        public TimeSpan DefaultMessageTimeToLive { get; init; }

        [JsonPropertyName("lockDuration")]
        public TimeSpan LockDuration { get; init; }

        [JsonPropertyName("duplicateDetectionHistoryTimeWindow")]
        public TimeSpan DuplicateDetectionHistoryTimeWindow { get; init; }

        [JsonPropertyName("maxSizeInMegabytes")]
        public long MaxSizeInMegabytes { get; init; }

        [JsonPropertyName("maxDeliveryCount")]
        public int MaxDeliveryCount { get; init; }

        [JsonPropertyName("enableBatchedOperations")]
        public bool EnableBatchedOperations { get; init; }

        [JsonPropertyName("enablePartitioning")]
        public bool EnablePartitioning { get; init; }

        [JsonPropertyName("requireSession")]
        public bool RequireSession { get; init; }

        [JsonPropertyName("requireDuplicateDetection")]
        public bool RequireDuplicateDetection { get; init; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ServiceBusQueueStatusResponse
    {
        Active,
        Disabled,
        SendDisabled,
        ReceiveDisabled,
    }
}
