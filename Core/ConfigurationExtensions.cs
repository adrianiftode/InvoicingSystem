﻿using Core.Services;
using Microsoft.Extensions.DependencyInjection;


namespace Core
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services) 
            => services
            .AddTransient<IInvoicesService, InvoicesService>();
    }
}
