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

        public async Task<bool> StopWorker(int taskId)
        {
            var response = await _client.StopWorkerAsync(new WorkerTaskId
            {
                TaskId = taskId,
            });
            return response.Success;
        }

        public async Task<bool> RestartWorker(int taskId)
        {
            var response = await _client.RestartWorkerAsync(new WorkerRestart
            {
                TaskId = taskId,
                SettingsId = string.Empty
            });
            return response.Success;
        }

        public async Task<bool> StartWorker(string settingId)
        {
            var response = await _client.StartWorkerAsync(new WorkerSettingsId
            {
                SettingsId = settingId
            });
            return response.Success;
        }

        public async Task<bool> StopAll()
        {
            var response = await _client.StopAllAsync(new Empty());
            return response.Success;
        }

        public async Task<bool> RunAll()
        {
            var response = await _client.RunAllAsync(new Empty());
            return response.Success;
        }
    }
}
