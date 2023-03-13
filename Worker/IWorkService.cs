using Worker.Model;

namespace Worker
{
    public interface IWorkService
    {
        public Task RestartWorkers();
        public Task RestartWorker(int taskId);

        public Task Run();

        public IReadOnlyCollection<Work> GetWorkers();
    }
}
