
namespace ServiceBusManager.Server.Infrastructure
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
}
