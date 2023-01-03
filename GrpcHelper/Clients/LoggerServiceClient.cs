using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcHelper.Interfaces;
using GrpcHelper.LogService;
using Serilog;

namespace GrpcHelper.Clients
{
    public class LoggerServiceClient : ILoggerServiceClient
    {
        private IServiceConfiguration _serviceConfig;
        private ILogger _logger;

        public LoggerServiceClient(IServiceConfiguration serviceConfig)
        {
            _serviceConfig = serviceConfig;
            _logger = new LoggerConfiguration().CreateLogger();
        }

        public async Task WriteLog(string message, string? eventId, string? application)
        {
            var serverAddress = _serviceConfig.GetServiceAddress<Logger.LoggerClient>();

            try
            {
                using var channel = GrpcChannel.ForAddress(serverAddress);

                var client = new Logger.LoggerClient(channel);
                var result = await client.WriteLogAsync(new LogModel
                {
                    Id = eventId,
                    Message = message,
                    Application = application,
                    Timestamp = Timestamp.FromDateTime(DateTime.UtcNow),
                });
                if (!result.Success)
                {
                    _logger.Fatal("Connection to logger service is unavailable");
                }
            }
            catch (RpcException err)
            {
                _logger.Fatal($"Logger service has an error: {err.Message}");
            }
        }
    }
}
