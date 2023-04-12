using Grpc.Net.ClientFactory;
using GrpcHelper.Interfaces;

namespace GrpcHelper.Clients
{
    public class WorkerClientFactory : IWorkerClientFactory
    {
        private readonly GrpcClientFactory _grpcClientFactory;

        public WorkerClientFactory(GrpcClientFactory grpcClientFactory)
        {
            _grpcClientFactory = grpcClientFactory;
        }
        public IWorkerServiceClient CreateClient(string name)
        {
            var client = _grpcClientFactory.CreateClient<WorkerService.WorkerService.WorkerServiceClient>(name);
            return new WorkerServiceClient(client);
        }
    }
}
