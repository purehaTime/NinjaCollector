using DbService.Interfaces;
using DbService.Repositories;
using Logger;
using MongoDB.Driver;
using Serilog;

namespace DbService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            //builder.Services.AddGrpc();

            var connectionString = builder.Configuration.GetConnectionString("mongodb");
            //builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(connectionString));
            //builder.Services.AddScoped<IGridFsRepository, GridFsRepository>();

            builder.Host.UseSerilog(LoggerSetup.ConfigureWithHttp);
            var app = builder.Build();

            
            app.MapGet("/", () => "Database service");

            app.Run();
        }
    }
}