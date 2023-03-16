using GrpcHelper.WorkerService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Worker.Grpc;
using Worker.Interfaces;
using Worker.Model;
using WorkerService = Worker.Grpc.WorkerService;

namespace Worker.ServiceExtension
{
    public static class RunWorkerServiceExtension
    {
        public static async Task RunWorkers(this IApplicationBuilder application)
        {
            var scope = application.ApplicationServices.CreateScope();

            var logger = scope.ServiceProvider.GetRequiredService<ILogger>();
            var workService = scope.ServiceProvider.GetRequiredService<IWorkService>();

            WorkEngine.InitLogger(logger);
            await workService.RunWorkers();
        }

        public static async Task RunWorker<TWorker>(this IApplicationBuilder application) where TWorker : class, IWorker
        {
            var scope = application.ApplicationServices.CreateScope();
            var worker = scope.ServiceProvider.GetRequiredService<TWorker>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger>();

            var workService = scope.ServiceProvider.GetRequiredService<IWorkService>();

            await workService.RunWorker(worker);
        }

        public static void MapWorkerService(this IEndpointRouteBuilder builder)
        {
            builder.MapGrpcService<WorkerService>();
        }
    }
}
