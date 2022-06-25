using ServiceBusManager.Server.Providers.Common;

namespace ServiceBusManager.Server.Providers.Aws
{
    public class AwsServiceBusProvider : IServiceBusProvider
    {
        public Task CreateQueueAsync(string name, ServiceBusQueueDetails details, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteQueueAsync(string name, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task GetQueueActiveMessagesAsync(string name, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceBusQueueDetails> GetQueueDetailsAsync(string name, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<ServiceBusQueue>> GetQueuesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task PurgeActiveQueueAsync(string name, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task PurgeDeadLetterQueueAsync(string name, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
