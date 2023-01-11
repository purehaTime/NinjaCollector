using GrpcHelper;
using Logger;
using Serilog;

namespace MainService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddGrpcHelper(builder.Configuration);
            builder.Host.UseSerilog(LoggerSetup.ConfigureWithHttp);
            var app = builder.Build();

            app.MapControllers();
            app.Run();
        }
    }
}