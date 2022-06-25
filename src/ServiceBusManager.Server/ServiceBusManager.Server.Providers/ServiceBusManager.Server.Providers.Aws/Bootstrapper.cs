using Microsoft.Extensions.DependencyInjection;

namespace ServiceBusManager.Server.Providers.Aws
{
    public static class Bootstrapper
    {
        public static void InitializeAwsProvider(this IServiceCollection services)
        {
            services.AddScoped<AwsServiceBusProvider>();
        }
    }
}
