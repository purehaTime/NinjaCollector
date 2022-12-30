using Grpc.Net.Client;
using GrpcHelper.Interfaces;
using GrpcHelper.LogService;
using Serilog;

namespace GrpcHelper
{
    public class LoggerServiceClient : ILoggerServiceClient
    {
        private string _serverAddress;
        private ILogger _logger;

        public LoggerServiceClient(string serverAddress)
        {
            _serverAddress = serverAddress;
            _logger = new LoggerConfiguration().CreateLogger();
        }

        public LoggerServiceClient(IServiceConfiguration config)
        {
            Console.WriteLine(config);
        }

        public async Task WriteLog(string? message)
        {
            using var channel = GrpcChannel.ForAddress(_serverAddress);
            
            var client = new Logger.LoggerClient(channel);
            var result = await client.WriteLogAsync(new LogModel
            {
                Message = message
            });

            if (!result.Success)
            {
                _logger.Fatal("Connection to logger service is unavailable");
            }
        }
    }
}
