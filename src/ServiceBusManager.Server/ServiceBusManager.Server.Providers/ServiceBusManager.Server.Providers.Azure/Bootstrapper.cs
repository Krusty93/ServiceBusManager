using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceBusManager.Server.Providers.Azure
{
    public static class Bootstrapper
    {
        public static void InitializeAzureProvider(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            SetupServiceBus(services, configuration);

            services.AddScoped<AzureServiceBusProvider>();
        }

        private static void SetupServiceBus(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAzureClients(builder =>
            {
                string connectionString = configuration.GetConnectionString("ServiceBus");
                builder.AddServiceBusAdministrationClient(connectionString);
                builder.AddServiceBusClient(connectionString);
            });
        }
    }
}
