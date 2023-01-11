using LiteDB.Async;
using LoggerService.Interfaces;
using ILogger = Serilog.ILogger;

namespace LoggerService.Services
{
    public class DatabaseService : IDatabase
    {
        private IDbConfiguration _config;
        private ILogger _logger;

        public DatabaseService(IDbConfiguration config, ILogger logger)
        {
            _config = config;
            _logger = logger;
        }
        
        public async Task<bool> Add<TEntity>(TEntity entity)
        {
            try
            {
                using var db = new LiteDatabaseAsync(_config.GetConnectionString());
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
                using var db = new LiteDatabaseAsync(_config.GetConnectionString());
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
