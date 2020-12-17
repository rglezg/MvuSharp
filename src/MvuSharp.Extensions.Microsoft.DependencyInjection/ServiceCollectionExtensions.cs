using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace MvuSharp.Extensions.Microsoft.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMvuSharp(this IServiceCollection services, IEnumerable<Assembly> assembliesToScan)
        {
            services.AddScoped<ServiceFactory>(p => p.GetService);
            services.AddScoped<IMediator, Mediator>();
            services.AddRequestHandlers(assembliesToScan);
            return services;
        } 
    }
}