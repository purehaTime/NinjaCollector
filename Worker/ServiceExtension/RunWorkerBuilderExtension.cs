using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Worker.Grpc;
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

            WorkRunner.InitLogger(logger);
            await WorkRunner.RunWorker(worker, null);
        }

        public static async Task RunWorker<TWorker>(this IApplicationBuilder application, Settings settings) where TWorker : class, IWorker
        {
            var scope = application.ApplicationServices.CreateScope();
            var worker = scope.ServiceProvider.GetRequiredService<TWorker>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger>();

            WorkRunner.InitLogger(logger);
            await WorkRunner.RunWorker(worker, settings);
        }

        public static void MapWorkerService(this IEndpointRouteBuilder builder)
        {
            builder.MapGrpcService<WorkerService>();
        }
    }
}
