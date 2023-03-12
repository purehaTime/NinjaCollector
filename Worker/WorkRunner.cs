using Serilog;
using Worker.Model;

namespace Worker
{
    internal static class WorkRunner
    {
        private static List<Work> _workers = new();

        public static IReadOnlyCollection<Work> GetWorkers()
        {
            return _workers.AsReadOnly();
        }

        public static async Task RunWorker(IWorker worker, Settings setting, ILogger logger)
        {
            var allSettings = new List<Settings>();
            if (setting == null)
            {
                allSettings = await worker.Init();
            }
            else
            {
                allSettings.Add(setting);
            }

            foreach (var settings in allSettings)
            {
                var ct = new CancellationTokenSource();
                var task = Task.Run(() => Worker(worker, settings, logger), ct.Token);
                _workers.Add(new Work
                {
                    Token = ct,
                    TaskId = task.Id,
                    GroupName = settings.ForGroup
                });
            }
        }


        public static async Task Worker(IWorker work, Settings settings, ILogger logger)
        {
            //timeout
            await Task.Delay(settings.Hold);

            var counter = 1; //by zero means run eternally
            var errorCounter = 0;

            while (counter != settings.Counts)
            {
                if (errorCounter == settings.RetryAfterErrorCount)
                {
                    logger.Error($"task {Task.CurrentId} was stopped die to lots of error");
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
                    logger.Error(err, $"Worker error for TaskId: {Task.CurrentId}");
                    errorCounter++;
                }

                await Task.Delay(settings.Timeout);
            }

            logger.Information($"Task {Task.CurrentId} with {settings.Id} finish work");
        }
    }


}
