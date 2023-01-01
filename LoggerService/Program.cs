using GrpcHelper;
using GrpcHelper.LogService;
using LoggerService.Services;

namespace LoggerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            builder.Services.AddGrpcHelper();
            builder.Services.AddGrpc();

            var app = builder.Build();

            app.MapGrpcService<LogService>();
            app.Run();
        }
    }
}