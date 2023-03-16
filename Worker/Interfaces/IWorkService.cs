using Worker.Model;

namespace Worker.Interfaces
{
    public interface IWorkService
    {
        Task RunWorkers();
        Task RunWorker(IWorker worker);
        IReadOnlyCollection<Model.Worker> GetWorkers();
        Task<bool> RestartWorkers();
        Task<bool> RestartWorker(int taskId, string settingId);
        bool StopAll();
        bool StopWorker(int taskId);
    }
}
