using Grpc.Net.ClientFactory;
using GrpcHelper.Interfaces;

namespace GrpcHelper.Clients
{
    public class WorkerClientAggregator : IWorkerClientAggregator
    {
        private readonly IWorkerServiceClient _client;

        public IWorkerServiceClient Reddit { get; }

        public WorkerClientAggregator(GrpcClientFactory grpcClientFactory)
        {
            var redditClient =  grpcClientFactory.CreateClient<WorkerService.WorkerService.WorkerServiceClient>("reddit");
            Reddit = new WorkerServiceClient(redditClient);
        }
    }
}
