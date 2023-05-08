using Serilog;
using Worker.Interfaces;

namespace Worker
{
    public class WorkService : IWorkService
    {
        private readonly IEnumerable<IWorker> _workers;
        private readonly ILogger _logger;

        public WorkService(IEnumerable<IWorker> workers, ILogger logger)
        {
            _workers = workers;
            _logger = logger;
            WorkEngine.InitLogger(logger);
        }

        public async Task RunWorkers()
        {
            Console.WriteLine("starting workers");
            await WorkEngine.RunWorkers(_workers);
        }

        public async Task RunWorker(IWorker worker)
        {
            try
            {
                await WorkEngine.RunWorker(worker);
            }
            catch (Exception err)
            {
                _logger.Error($"error while start worker {worker.Name}", err);
            }
        }


        public IReadOnlyCollection<Model.Worker> GetWorkers()
        {
            return WorkEngine.GetWorkers();
        }

        public async Task<bool> RestartWorkers()
        {
            try
            {
                var workers = WorkEngine.GetWorkers();
                WorkEngine.StopAll();
                await WorkEngine.RunWorkers(workers.Select(s => s.WorkerInstance));
            }
            catch (Exception err)
            {
                _logger.Error("Can't restart workers.", err);
                return false;
            }

            return true;
        }

        public async Task<bool> RestartWorker(int taskId, string settingId)
        {
            try
            {
                return await WorkEngine.RestartWorker(taskId, settingId);
            }
            catch (Exception err)
            {
                _logger.Error($"Can't restart work {taskId}", err);
            }

            return false;
        }

        public bool StopWork(string settingId)
        {
            try
            {
                return WorkEngine.StopWork(settingId);
            }
            catch (Exception err)
            {
                _logger.Error($"Can't restart work {settingId}", err);
            }

            return false;
        }

        public bool StopAll()
        {
            return WorkEngine.StopAll();
        }

        public bool StopWorker(int taskId)
        {
            return WorkEngine.StopWorker(taskId);
        }
    }
}
