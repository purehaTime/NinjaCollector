using DbService.Interfaces;
using DbService.Models;
using DbService.Repositories;
using GrpcHelper;
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
            builder.Services.AddGrpc();
            builder.Services.AddGrpcHelper(builder.Configuration);

            var connectionString = builder.Configuration.GetConnectionString("mongodb");
            builder.Services.AddSingleton<IDbConfiguration, IDbConfiguration>();
            builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(connectionString));

            builder.Services.AddScoped<IGridFsRepository, GridFsRepository>();
            builder.Services.AddScoped<IRepository<ParserSettings>, SettingsRepository>();

            builder.Host.UseSerilog(LoggerSetup.ConfigureWithHttp);
            var app = builder.Build();

            
            app.MapGet("/", () => "Database service");

            app.Run();
        }
    }
}