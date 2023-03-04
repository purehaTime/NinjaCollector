using Worker.Model;

namespace Worker
{
    internal static class WorkRunner
    {
        private static List<Work> _workers = new();

        public static void RunWorker(Func<Task> work, Settings settings)
        {
            var ct = new CancellationTokenSource();
            
            var task = Task.Run(() => Worker(work, settings), ct.Token);

            _workers.Add(new Work
            {
                Token = ct,
                TaskId = task.Id,
                GroupName = settings.ApiName
            });
        }

        public static IReadOnlyCollection<Work> GetWorkers()
        {
            return _workers.AsReadOnly();
        }

        private static async Task Worker(Func<Task> work, Settings settings)
        {
            //timeout
            await Task.Delay(settings.Hold);

            var counter = 1; //by zero means run eternally
            var errorCounter = 0;

            while (counter != settings.Counts)
            {
                if (errorCounter == settings.RetryAfterErrorCount)
                {
                    Console.WriteLine($"task {Task.CurrentId} was stopped die to lots of error");
                }

                try
                {
                    await work();
                    counter++;
                    await Task.Delay(settings.Timeout);
                }
                catch (Exception err)
                {
                    Console.WriteLine($"Worker error for TaskId: {Task.CurrentId}");
                    Console.WriteLine(err.Message);
                    errorCounter++;
                }
            }
        }
    }


}
