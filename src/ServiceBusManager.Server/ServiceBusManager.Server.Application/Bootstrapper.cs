using Microsoft.Extensions.DependencyInjection;
using ServiceBusManager.Server.Application.Queries;

namespace ServiceBusManager.Server.Application
{
    public class Bootstrapper
    {
        public static void Initialize(IServiceCollection services)
        {
            services.AddScoped<IServiceBusQueries, ServiceBusQueries>();
        }
    }
}
