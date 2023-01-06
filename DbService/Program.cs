using DbService.Services;
using Logger;
using Serilog;

namespace DbService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddGrpc();
            builder.Host.UseSerilog(LoggerSetup.ConfigureWithHttp);
            var app = builder.Build();

            app.MapGrpcService<DbLoggerService>();
            app.MapGet("/", () => "Database service");

            app.Run();
        }
    }
}