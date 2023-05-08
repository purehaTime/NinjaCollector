using GrpcHelper.WorkerService;

namespace GrpcHelper.Interfaces
{
    public interface IWorkerServiceClient
    {
        public Task<WorkerModel> GetWorkers();
        public Task<bool> StopWorker(int taskId);
        public Task<bool> RestartWorker(int taskId);
        public Task<bool> StartWorker(string settingId);
        public Task<bool> StopAll();
        public Task<bool> RunAll();

    }
}
