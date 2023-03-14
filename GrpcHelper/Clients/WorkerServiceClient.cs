using Google.Protobuf.WellKnownTypes;
using GrpcHelper.Interfaces;
using GrpcHelper.WorkerService;

namespace GrpcHelper.Clients
{
    public class WorkerServiceClient : IWorkerServiceClient
    {
        private readonly Worker.WorkerClient _client;

        public WorkerServiceClient(Worker.WorkerClient client)
        {
            _client = client;
        }
        public async Task<WorkerModel> GetWorkers()
        {
            var result = await _client.GetWorkersAsync(new Empty());
            return result;
        }
    }
}
