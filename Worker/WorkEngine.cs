using Models.DataModels;
using Serilog;
using Serilog.Core;
using Worker.Interfaces;
using Worker.Model;

namespace Worker
{
    internal static class WorkEngine
    {
        private static readonly List<Model.Worker> _workers = new();
        private static ILogger _logger = Logger.None;

        public static void InitLogger(ILogger logger)
        {
            _logger = logger;
        }

        public static async Task RunWorkers(IEnumerable<IWorker> workers)
        {
            await Parallel.ForEachAsync(workers, async (worker, ct) =>
            {
                await RunWorker(worker);
            });
        }

        public static async Task RunWorker(IWorker worker)
        {
            var settings = await worker.Init();
            var works = new List<Work>();
            foreach (var setting in settings)
            {
                var ct = new CancellationTokenSource();
                var task = Task.Run(() => Worker(worker, setting), ct.Token);
                works.Add(new Work
                {
                    Token = ct,
                    TaskId = task.Id,
                    Settings = setting
                });
                Console.WriteLine("setting " + setting.Id + " started");
            }

            _workers.Add(new Model.Worker
            {
                WorkerInstance = worker,
                Works = works
            });
        }


        public static IReadOnlyCollection<Model.Worker> GetWorkers()
        {
            return _workers.AsReadOnly();
        }

        public static async Task<bool> RestartWorker(int taskId, string settingId)
        {
            foreach (var worker in _workers)
            {
                var work = worker.Works.FirstOrDefault(f => f.TaskId == taskId);

                if (work != null)
                {
                    work.Token.Cancel();
                    worker.Works.Remove(work);

                    var setting = string.IsNullOrEmpty(settingId)
                        ? work.Settings
                        : await worker.WorkerInstance.LoadSettings(settingId);

                    var ct = new CancellationTokenSource();
                    var task = Task.Run(() => Worker(worker.WorkerInstance, setting ?? work.Settings), ct.Token);

                    worker.Works.Add(new Work
                    {
                        Settings = setting,
                        TaskId = task.Id,
                        Token = ct
                    });

                    return true;
                }
            }

            return false;
        }

        public static bool StopAll()
        {
            foreach (var work in _workers.SelectMany(worker => worker.Works))
            {
                work.Token.Cancel();
            }
            _workers.Clear();

            return true;
        }

        public static bool StopWorker(int taskId)
        {
            foreach (var worker in _workers)
            {
                var work = worker.Works.FirstOrDefault(f => f.TaskId == taskId);

                if (work != null)
                {
                    work.Token.Cancel();
                    worker.Works.Remove(work);
                    return true;
                }
            }

            return false;
        }

        private static async Task Worker(IWorker work, ParserSettings settings)
        {
            //timeout
            await Task.Delay(settings.Hold);

            var counter = 1; //by zero means run eternally
            var errorCounter = 0;

            while (counter != settings.Counts)
            {
                if (errorCounter == settings.RetryAfterErrorCount)
                {
                    _logger.Error($"task {Task.CurrentId} was stopped die to lots of error");
                }

                try
                {
                    settings = await work.Run(settings);
                    if (settings.Disabled)
                    {
                        break;
                    }

                    counter++;

                }
                catch (Exception err)
                {
                    _logger.Error(err, $"Worker error for TaskId: {Task.CurrentId}");
                    errorCounter++;
                }

                await Task.Delay(settings.Timeout);
            }

            _logger.Information($"Task {Task.CurrentId} with {settings.Id} finish work");
        }
    }


}
