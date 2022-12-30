using Grpc.Net.Client;
using GrpcHelper.DbService;
using static GrpcHelper.DbService.DatabaseLog;

namespace GrpcHelper
{
    public class DatabaseServiceClient
    {
        private string _serverAddress;

        public DatabaseServiceClient(string serverAddress)
        {
            _serverAddress = serverAddress;
        }

        public DatabaseServiceClient()
        {
            
        }

        public async Task<bool> WriteLogToDb(string? message)
        {
            using var channel = GrpcChannel.ForAddress(_serverAddress);

            var client = new DatabaseLogClient(channel);
            var result = await client.WriteLogAsync(new DbLogModel());

            return result.Success;
        }
    }
}
