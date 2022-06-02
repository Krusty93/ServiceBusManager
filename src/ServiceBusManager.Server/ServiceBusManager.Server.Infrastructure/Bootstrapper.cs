using Microsoft.Extensions.DependencyInjection;
using ServiceBusManager.Server.Infrastructure.AzureServiceBus;

namespace ServiceBusManager.Server.Infrastructure
{
    public class Bootstrapper
    {
        public static void Initialize(IServiceCollection services)
        {
            services.AddScoped<IServiceBusProvider, AzureServiceBusProvider>();
        }
    }
}
