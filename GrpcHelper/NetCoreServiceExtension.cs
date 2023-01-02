using GrpcHelper.Clients;
using GrpcHelper.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GrpcHelper
{
    public static class NetCoreServiceExtension
    {
        public static void AddGrpcHelper(this IServiceCollection services)
        {
            services.AddSingleton<IServiceConfiguration, ServiceConfiguration>();
            services.AddScoped<ILoggerServiceClient, LoggerServiceClient>();
            services.AddScoped<IDatabaseServiceClient, DatabaseServiceClient>();
        }

        public static IServiceCollection AddGrpsHelper<TService, TClass>(this IServiceCollection services) 
            where TService : class
            where TClass : class, TService
        {
            services.AddScoped<TService, TClass>();
            return services;
        }
    }
}