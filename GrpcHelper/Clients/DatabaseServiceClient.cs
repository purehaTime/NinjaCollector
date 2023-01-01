using Grpc.Net.Client;
using GrpcHelper.DbService;
using GrpcHelper.Interfaces;
using static GrpcHelper.DbService.DatabaseLog;

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
            var serverAddress = _serviceConfig.GetServiceAddress<DatabaseLogClient>();
            using var channel = GrpcChannel.ForAddress(serverAddress);

            var client = new DatabaseLogClient(channel);
            var result = await client.WriteLogAsync(new DbLogModel
            {
                Jsondata = "" 
            });

            return result.Success;
        }
    }
}
