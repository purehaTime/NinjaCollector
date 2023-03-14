using GrpcHelper.WorkerService;

namespace GrpcHelper.Interfaces
{
    public interface IWorkerServiceClient
    {
        public Task<WorkerModel> GetWorkers();

    }
}
