using GrpcHelper.Clients;
using GrpcHelper.DbService;
using GrpcHelper.Interfaces;
using GrpcHelper.LogService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GrpcHelper
{
    public static class NetCoreServiceExtension
    {
        public static void AddGrpcHelper(this IServiceCollection services, IConfiguration config)
        {
            var configMapping = CreateConfigMapping();
            
            services.AddScoped<ILoggerServiceClient, LoggerServiceClient>();
            services.AddScoped<IDatabaseServiceClient, DatabaseServiceClient>();
            services.AddScoped<IWorkerServiceClient, WorkerServiceClient>();

            services.AddGrpcClient<Database.DatabaseClient>(x => x.Address = new Uri(GetUrl<Database.DatabaseClient>(configMapping, config)));
            services.AddGrpcClient<Logger.LoggerClient>(x => x.Address = new Uri(GetUrl<Logger.LoggerClient>(configMapping, config)));
            services.AddGrpcClient<WorkerService.WorkerService.WorkerServiceClient>(x => x.Address = new Uri("http://localhost:80"));
        }

        public static IServiceCollection AddGrpsHelper<TService, TClass>(this IServiceCollection services) 
            where TService : class
            where TClass : class, TService
        {
            services.AddScoped<TService, TClass>();
            return services;
        }

        private static string GetUrl<TService>(Dictionary<string, string> mapping, IConfiguration config)
        {
            var serviceName = typeof(TService);
            var isValid = mapping.TryGetValue(serviceName.Name, out var configKey);
            if (isValid)
            {
                return config.GetSection(configKey)?.Value ?? "";
            }

            return "";
        }

        private static Dictionary<string, string> CreateConfigMapping()
        {
            var mapping = new Dictionary<string, string>
            {
                { nameof(Logger.LoggerClient), "ServiceAddress:LoggerService" },
                { nameof(Database.DatabaseClient), "ServiceAddress:DbService" }
            };

            return mapping;
        }

    }
}