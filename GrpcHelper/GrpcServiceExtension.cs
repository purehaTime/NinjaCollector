using GrpcHelper.Clients;
using GrpcHelper.DbService;
using GrpcHelper.Interfaces;
using GrpcHelper.LogService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GrpcHelper
{
    public static class GrpcServiceExtension
    {
        private static Dictionary<string, string> _configMapping = CreateConfigMapping();

        public static void AddGrpcHelper(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<ILoggerServiceClient, LoggerServiceClient>();
            services.AddScoped<IDatabaseServiceClient, DatabaseServiceClient>();
            //services.AddScoped<IWorkerServiceClient, WorkerServiceClient>();
            services.AddScoped<IWorkerClientAggregator, WorkerClientAggregator>();

            services.AddGrpcClient<Database.DatabaseClient>(x => x.Address = new Uri(GetUrl<Database.DatabaseClient>(config)));
            services.AddGrpcClient<Logger.LoggerClient>(x => x.Address = new Uri(GetUrl<Logger.LoggerClient>(config)));

            services.AddGrpcClient<WorkerService.WorkerService.WorkerServiceClient>("reddit", x => x.Address = new Uri(GetUrl("RedditService", config)));
        }

        public static IServiceCollection AddGrpsHelper<TService, TClass>(this IServiceCollection services)
            where TService : class
            where TClass : class, TService
        {
            services.AddScoped<TService, TClass>();
            return services;
        }

        private static string GetUrl<TService>(IConfiguration config)
        {
            var serviceName = typeof(TService);
            return GetUrl(serviceName.Name, config);
        }

        private static string GetUrl(string name, IConfiguration config)
        {
            var isValid = _configMapping.TryGetValue(name, out var configKey);
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
                { nameof(Database.DatabaseClient), "ServiceAddress:DbService" },
                { "RedditService", "ServiceAddress:RedditService" },
            };

            return mapping;
        }

    }
}