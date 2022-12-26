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
            builder.Host.UseSerilog(LoggerSetup.Configure);

            var app = builder.Build();

            
            app.MapControllers();
            app.Run();
        }
    }
}