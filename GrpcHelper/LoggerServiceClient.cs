using Grpc.Net.Client;
using GrpcHelper.LogService;

namespace GrpcHelper
{
    public class LoggerServiceClient
    {
        private string _serverAddress;

        public LoggerServiceClient(string serverAddress)
        {
            _serverAddress = serverAddress;
        }

        public async Task WriteLog(string? message)
        {
            using var channel = GrpcChannel.ForAddress(_serverAddress);
            var client = new Logger.LoggerClient(channel);
            var result = await client.WriteLogAsync(new LogModel
            {
                Message = message
            });

        }
    }
}
