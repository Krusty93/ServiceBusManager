using System.Diagnostics;
using ServiceBusManager.Server.Providers.Common;

namespace ServiceBusManager.Server.Providers.Azure.Models
{
    [DebuggerDisplay("{Name} ({ActiveCount}, {DeadLetterCount})")]
    public class AzureServiceBusQueue : ServiceBusQueue
    {
        public AzureServiceBusQueue(string name, long activeCount, long deadLetterCount)
            : base(name)
        {
            ActiveCount = activeCount;
            DeadLetterCount = deadLetterCount;
        }

        public long ActiveCount { get; }

        public long DeadLetterCount { get; }
    }
}
