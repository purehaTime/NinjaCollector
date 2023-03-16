using Google.Protobuf.WellKnownTypes;
using GrpcHelper.Interfaces;
using GrpcHelper.WorkerService;

namespace GrpcHelper.Clients
{
    public class WorkerServiceClient : IWorkerServiceClient
    {
        private readonly WorkerService.WorkerService.WorkerServiceClient _client;

        public WorkerServiceClient(IEnumerable<WorkerService.WorkerService.WorkerServiceClient> clients)
        {
            _client = clients?.FirstOrDefault();
        }
        public async Task<WorkerModel> GetWorkers()
        {
            var result = await _client.GetWorkersAsync(new Empty());
            return result;
        }
    }
}
