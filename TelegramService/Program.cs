using GrpcHelper;
using Logger;
using Serilog;

namespace TelegramService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddGrpcHelper(builder.Configuration);


            builder.Host.UseSerilog(LoggerSetup.ConfigureWithHttp);

            var app = builder.Build();

            app.Run();
        }
    }
}