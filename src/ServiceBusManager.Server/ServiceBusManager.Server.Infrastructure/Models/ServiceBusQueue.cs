using System.Diagnostics;

namespace ServiceBusManager.Server.Infrastructure
{
    [DebuggerDisplay("{Name} ({ActiveCount}, {DeadLetterCount})")]
    public class ServiceBusQueue
    {
        public ServiceBusQueue(string name, long activeCount, long deadLetterCount)
        {
            Name = name;
            ActiveCount = activeCount;
            DeadLetterCount = deadLetterCount;
        }

        public string Name { get; }

        public long ActiveCount { get; }

        public long DeadLetterCount { get; }
    }
}
