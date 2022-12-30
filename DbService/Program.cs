using DbService.Services;

namespace DbService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddGrpc();
            var app = builder.Build();

            app.MapGrpcService<DbLoggerService>();
            app.MapGet("/", () => "Database service");

            app.Run();
        }
    }
}