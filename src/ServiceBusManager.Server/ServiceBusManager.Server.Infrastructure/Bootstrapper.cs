using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ServiceBusManager.Server.Providers.Aws;
using ServiceBusManager.Server.Providers.Azure;
using ServiceBusManager.Server.Providers.Common;

namespace ServiceBusManager.Server.Infrastructure
{
    public static class Bootstrapper
    {
        public static void InitializeInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.InitializeAwsProvider();
            services.InitializeAzureProvider(configuration);

            services.AddScoped<IServiceBusProvider>(sp =>
            {
                IOptionsSnapshot<ProviderOption> option = sp.GetRequiredService<IOptionsSnapshot<ProviderOption>>();

                return option.Value.Type switch
                {
                    ProviderType.Az => sp.GetRequiredService<AzureServiceBusProvider>(),
                    ProviderType.Aws => sp.GetRequiredService<AwsServiceBusProvider>(),
                    _ => throw new NotImplementedException($"Unknown '{option.Value.Type}' type")
                };
            });
        }
    }
}
