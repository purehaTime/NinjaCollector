using GrpcHelper;
using Logger;
using Serilog;
using TelegramService.Workers;
using Worker.ServiceExtension;

namespace TelegramService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddGrpcHelper(builder.Configuration);

            builder.Services.AddWorker<PosterWorker>();

            builder.Host.UseSerilog(LoggerSetup.ConfigureWithHttp);

            var app = builder.Build();

            app.MapWorkerService();
            await app.RunWorkers();

            await app.RunAsync();
        }
    }
}