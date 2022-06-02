using System.Text.Json.Serialization;
using MediatR;
using ServiceBusManager.Server.API.Converters;

namespace ServiceBusManager.Server.Application.Commands
{
    public class CreateQueueCommand : IRequest
    {
        [JsonIgnore]
        public string? Name { get; set; }

        [JsonPropertyName("autoDeleteOnIdle")]
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan AutoDeleteOnIdle { get; set; }

        [JsonConverter(typeof(TimeSpanConverter))]
        [JsonPropertyName("defaultMessageTimeToLive")]
        public TimeSpan DefaultMessageTimeToLive { get; init; }

        [JsonConverter(typeof(TimeSpanConverter))]
        [JsonPropertyName("lockDuration")]
        public TimeSpan LockDuration { get; init; }

        [JsonConverter(typeof(TimeSpanConverter))]
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
}
