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

        public async Task RestartWorker(int taskId)
        {
            var works = WorkRunner.GetWorkers();
            var worker = works.FirstOrDefault(f => f.TaskId == taskId);
            if (worker != null)
            {
                worker.Token.Cancel();
                await WorkRunner.RunWorker(_worker, worker.Settings);
            }
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
