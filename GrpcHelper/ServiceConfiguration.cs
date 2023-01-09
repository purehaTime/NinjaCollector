using GrpcHelper.DbService;
using GrpcHelper.Interfaces;
using GrpcHelper.LogService;
using Microsoft.Extensions.Configuration;

namespace GrpcHelper
{
    public class ServiceConfiguration : IServiceConfiguration
    {
        private IConfiguration _appConfig;

        public ServiceConfiguration(IConfiguration appConfig)
        {
            _appConfig = appConfig;
        }

        public string GetServiceAddress<TService>()
        {
            var serviceName = typeof(TService);

            var isValid = CreateMapping().TryGetValue(serviceName.Name, out var configKey);

            if (!isValid)
            {
                throw new InvalidCastException(
                    $"Configuration mapping error. Cant find config key for service: {serviceName.Name}");
            }

            var result = _appConfig.GetSection(configKey);

            if (result.Value == null)
            {
                throw new InvalidCastException(
                    $"Configuration error. Cant service address for service: {serviceName.Name}. Check appSettings.json");
            }

            return result.Value;
        }

        private Dictionary<string, string> CreateMapping()
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
