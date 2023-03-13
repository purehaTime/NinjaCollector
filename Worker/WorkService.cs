using Serilog;
using Worker.Model;

namespace Worker
{
    public class WorkService : IWorkService
    {
        private readonly IWorker _worker;

        public WorkService(IWorker worker, ILogger logger)
        {
            _worker = worker;
            WorkRunner.InitLogger(logger);
        }
        public async Task RestartWorkers()
        {
            await WorkRunner.RestartWorker(_worker);
        }

        public Task RestartWorker(int taskId)
        {
            throw new NotImplementedException();
        }

        public async Task Run()
        {
            await WorkRunner.RunWorker(_worker, null);
        }

        public IReadOnlyCollection<Work> GetWorkers()
        {
            var works = WorkRunner.GetWorkers();

            return works;
        }
    }
}
