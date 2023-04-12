using MainService.Models;

namespace MainService.Interfaces
{
    public interface IWorkerService<TStatusModel> where TStatusModel : StatusModel
    {
        public Task<bool> StopWorker(int taskId);

        public Task<TStatusModel> GetStatus();
    }
}
