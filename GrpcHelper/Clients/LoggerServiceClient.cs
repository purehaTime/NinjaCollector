using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcHelper.Interfaces;
using GrpcHelper.LogService;
using Serilog;

namespace GrpcHelper.Clients
{
    public class LoggerServiceClient : ILoggerServiceClient
    {
        private readonly ILogger _logger;
        private readonly Logger.LoggerClient _client;

        public LoggerServiceClient(Logger.LoggerClient client)
        {
            _logger = new LoggerConfiguration().CreateLogger();
            _client = client;
        }

        public async Task WriteLog(string message, string? eventId, string? application)
        {
            try
            {
                var result = await _client.WriteLogAsync(new LogModel
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
