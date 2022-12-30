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
            services.AddScoped<DatabaseServiceClient>();
        }
    }
}