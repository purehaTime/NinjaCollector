using LiteDB.Async;
using LoggerService.Interfaces;
using ILogger = Serilog.ILogger;

namespace LoggerService.Services
{
    public class DatabaseService : IDatabase
    {
        private IConfiguration _config;
        private ILogger _logger;

        public DatabaseService(IConfiguration config, ILogger logger)
        {
            _config = config;
            _logger = logger;
        }
        
        public async Task<bool> Add<TEntity>(TEntity entity)
        {
            try
            {
                var path = _config.GetSection("LiteDb:FileName")?.Value ?? "log.data";
                var pass = _config.GetSection("LiteDb:Password")?.Value ?? new Guid().ToString();

                using var db = new LiteDatabaseAsync($"Filename={path};Connection=shared;Password={pass}");
                var collection = db.GetCollection<TEntity>(nameof(TEntity) + "s");
                await collection.InsertAsync(entity);
                return true;
            }
            catch (Exception err)
            {
                _logger.Fatal(err, $"Can't write logs to db. Reason: {err.Message}");
                return false;
            }
        }

        public async Task<IEnumerable<TEntity>> GetAll<TEntity>()
        {
            try
            {
                var path = _config.GetSection("LiteDb:FileName")?.Value ?? "log.data";
                var pass = _config.GetSection("LiteDb:Password")?.Value ?? new Guid().ToString();

                using var db = new LiteDatabaseAsync($"Filename={path};Connection=shared;Password={pass}");
                var collection = db.GetCollection<TEntity>(nameof(TEntity) + "s");
                var result =  await collection.FindAllAsync();
                return result;
            }
            catch (Exception err)
            {
                _logger.Fatal(err, $"Can't get all logs to db. Reason: {err.Message}");
                return Enumerable.Empty<TEntity>();
            }
        }
    }
}
