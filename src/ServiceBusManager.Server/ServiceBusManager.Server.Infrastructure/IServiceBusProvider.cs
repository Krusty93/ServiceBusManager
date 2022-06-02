
namespace ServiceBusManager.Server.Infrastructure
{
    public interface IServiceBusProvider
    {
        Task<IReadOnlyCollection<ServiceBusQueue>> GetQueuesAsync(CancellationToken cancellationToken = default);

        Task<ServiceBusQueueDetails> GetQueueDetailsAsync(string name, CancellationToken cancellationToken);
    }
}
