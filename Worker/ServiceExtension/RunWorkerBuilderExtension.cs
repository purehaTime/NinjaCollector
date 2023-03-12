using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using Serilog;
using Worker.Model;

namespace Worker.ServiceExtension
{
    public static class RunWorkerServiceExtension
    {
        public static async Task RunWorker<TWorker>(this IApplicationBuilder application) where TWorker : class, IWorker
        {
            var scope = application.ApplicationServices.CreateScope();
            var worker = scope.ServiceProvider.GetRequiredService<TWorker>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger>();

            await WorkRunner.RunWorker(worker, null, logger);
        }

        public static async Task RunWorker<TWorker>(this IApplicationBuilder application, Settings settings) where TWorker : class, IWorker
        {
            var scope = application.ApplicationServices.CreateScope();
            var worker = scope.ServiceProvider.GetRequiredService<TWorker>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger>();

            await WorkRunner.RunWorker(worker, settings, logger);
        }
    }
}
