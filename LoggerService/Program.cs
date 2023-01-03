using GrpcHelper;
using LoggerService.Services;
using Serilog;

namespace LoggerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var builder = WebApplication.CreateBuilder(args);

                builder.Services.AddGrpcHelper();
                builder.Services.AddGrpc();
                builder.Host.UseSerilog();

                var app = builder.Build();

                app.MapGrpcService<LogService>();
                app.Run();
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
        }
    }
}