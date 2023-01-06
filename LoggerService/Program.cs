using System.Runtime.CompilerServices;
using GrpcHelper;
using LoggerService.Interfaces;
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
                builder.Services.AddSingleton(Log.Logger);
                builder.Services.AddScoped<IDatabase, DatabaseService>();

                var app = builder.Build();

                app.MapGrpcService<LogService>();
                app.MapGet("/", () => "logger service");
                app.Run();
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
        }
    }
}