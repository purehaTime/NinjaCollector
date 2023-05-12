using DbService.Interfaces;
using DbService.Models;
using DbService.Repositories;
using DbService.Services;
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
            builder.Services.AddGrpcHelper(builder.Configuration);

            var connectionString = builder.Configuration.GetConnectionString("mongodb");
            builder.Services.AddSingleton<IDbConfiguration, DbConfiguration>();
            builder.Services.AddSingleton<IMongoClient>(_ => new MongoClient(connectionString));

            builder.Services.AddScoped<IGridFsService, GridFsService>();
            builder.Services.AddScoped<IRepository<ParserSettings>, ParserSettingsRepository>();
            builder.Services.AddScoped<IRepository<Image>, ImageRepository>();
            builder.Services.AddScoped<IRepository<Post>, PostRepository>();
            builder.Services.AddScoped<IRepository<History>, HistoryRepository>();
            builder.Services.AddScoped<IRepository<PosterSettings>, PosterSettingsRepository>();
            builder.Services.AddScoped<IRepository<User>, UserRepository>();

            builder.Services.AddScoped<ISettingsService, SettingsService>();
            builder.Services.AddScoped<IHistoryService, HistoryService>();
            builder.Services.AddScoped<IImageService, ImageService>();
            builder.Services.AddScoped<IPostService, PostService>();
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Host.UseSerilog(LoggerSetup.ConfigureWithHttp);
            var app = builder.Build();

            app.MapGrpcService<Services.DbService>();
            app.Run();
        }
    }
}