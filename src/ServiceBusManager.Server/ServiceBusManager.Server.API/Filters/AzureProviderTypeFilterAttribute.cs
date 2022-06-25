﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using ServiceBusManager.Server.Providers.Common;

namespace ServiceBusManager.Server.API.Filters
{
    public class AzureProviderTypeFilterAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            using var scope = context.HttpContext.RequestServices.CreateScope();

            var opt = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<ProviderOption>>();

            if (opt.Value.Type != ProviderType.Az)
                throw new InvalidOperationException();

            await next();
        }
    }
}
