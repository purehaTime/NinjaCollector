using Google.Protobuf.WellKnownTypes;
using GrpcHelper.Interfaces;
using GrpcHelper.WorkerService;

namespace GrpcHelper.Clients
{
    public class WorkerServiceClient : IWorkerServiceClient
    {
        private readonly WorkerService.WorkerService.WorkerServiceClient _client;


        public WorkerServiceClient(WorkerService.WorkerService.WorkerServiceClient client)
        {
            _client = client;
        }

        public async Task<WorkerModel> GetWorkers()
        {
            var response = await _client.GetWorkersAsync(new Empty());
            return response;
        }
    }
}
