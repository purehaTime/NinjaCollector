using Grpc.Net.Client;
using GrpcHelper.DbService;
using GrpcHelper.Interfaces;
using static GrpcHelper.DbService.Database;

namespace GrpcHelper.Clients
{
    public class DatabaseServiceClient : IDatabaseServiceClient
    {
        private IServiceConfiguration _serviceConfig;

        public DatabaseServiceClient(IServiceConfiguration serviceConfig)
        {
            _serviceConfig = serviceConfig;
        }

        public async Task<bool> WriteLogToDb(DbLogModel? message)
        {
            var serverAddress = _serviceConfig.GetServiceAddress<DatabaseClient>();
            using var channel = GrpcChannel.ForAddress(serverAddress);

            var client = new DatabaseClient(channel);
            var result = await client.WriteLogAsync(message);

            return result.Success;
        }
    }
}
