using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Worker.ServiceExtension
{
    public static class RunWorkerServiceExtension
    {
        public static async Task RunWorker<TWorker>(this IApplicationBuilder application) where TWorker : class, IWorker
        {
            var scope = application.ApplicationServices.CreateScope();
            var worker = scope.ServiceProvider.GetRequiredService<TWorker>();

            var settings = await worker.Init();

            foreach (var setting in settings) // possible change to parallel foreach
            {
                WorkRunner.RunWorker(() => worker.Run(setting), setting);
            }
        }
    }
}
