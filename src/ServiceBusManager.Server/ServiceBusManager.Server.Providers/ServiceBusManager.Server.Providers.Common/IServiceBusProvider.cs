namespace ServiceBusManager.Server.Providers.Common
{
    public interface IServiceBusProvider
    {
        Task<IReadOnlyCollection<ServiceBusQueue>> GetQueuesAsync(CancellationToken cancellationToken = default);

        Task<ServiceBusQueueDetails> GetQueueDetailsAsync(string name, CancellationToken cancellationToken = default);

        Task DeleteQueueAsync(string name, CancellationToken cancellationToken = default);

        Task CreateQueueAsync(string name, ServiceBusQueueDetails details, CancellationToken cancellationToken = default);

        Task PurgeActiveQueueAsync(string name, CancellationToken cancellationToken = default);

        Task PurgeDeadLetterQueueAsync(string name, CancellationToken cancellationToken = default);

        Task GetQueueActiveMessagesAsync(string name, CancellationToken cancellationToken = default);
    }

    public enum ServiceBusQueueStatus
    {
        InvalidState = 0,

        Active,
        Disabled,
        SendDisabled,
        ReceiveDisabled,
    }

    public class ServiceBusQueue
    {
        public ServiceBusQueue(
            string name)
        {
            Name = name;
        }

        public string Name { get; }
    }

    public class ServiceBusQueueDetails
    {
        public ServiceBusQueueDetails(string name)
        {
            Name = name;
        }

        public ServiceBusQueueDetails(
            string name,
            ServiceBusQueueStatus status)
            : this(name)
        {
            Status = status;
        }

        public string Name { get; }

        public ServiceBusQueueStatus Status { get; }
    }
}
