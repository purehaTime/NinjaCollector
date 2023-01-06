using DbService.Interfaces;
using MongoDB.Driver;
using ILogger = Serilog.ILogger;

namespace DbService.Repositories
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {

        private readonly ILogger _logger;
        private readonly IMongoClient _mongoClient;
        private readonly string _dbName;
        private readonly string _collectionName;


        public BaseRepository(IMongoClient client, string dbName, string collectionName, ILogger logger)
        {
            _logger = logger;
            _mongoClient = client;
            _dbName = dbName;
            _collectionName = collectionName;
        }

        public virtual async Task<TEntity> Find(FilterDefinition<TEntity> filter, FindOptions options, CancellationToken cToken)
        {
            try
            {
                var collection = InitCollection();
                var result = await collection
                    .Find(filter, options)
                    .FirstOrDefaultAsync(cToken);
                return result;
            }
            catch(Exception err)
            {
                _logger.Error(err, $"Find error. Details: {err.Message}");
            }

            return null!;
        }

        public virtual async Task<IEnumerable<TEntity>> FindMany(FilterDefinition<TEntity> filter, FindOptions options, CancellationToken cToken)
        {
            try
            {
                var collection = InitCollection();
                var result = await collection
                    .Find(filter, options)
                    .ToListAsync(cToken);

                return result;
            }
            catch (Exception err)
            {
                _logger.Error(err, $"FindMany error. Details: {err.Message}");
            }

            return null!;
        }

        public virtual async Task<bool> Insert(TEntity entity, InsertOneOptions options, CancellationToken cToken)
        {
            try
            {
                var collection = InitCollection();
                await collection.InsertOneAsync(entity, options, cToken);

                return true;
            }
            catch (Exception err)
            {
                _logger.Error(err, $"Insert error. Details: {err.Message}");
            }

            return false;
        }

        public virtual async Task<bool> InsertMany(IEnumerable<TEntity> entities, InsertManyOptions options, CancellationToken cToken)
        {
            try
            {
                var collection = InitCollection();
                await collection.InsertManyAsync(entities, options, cToken);

                return true;
            }
            catch (Exception err)
            {
                _logger.Error(err, $"InsertMany error. Details: {err.Message}");
            }

            return false;
        }

        public virtual async Task<TEntity> Update(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update, FindOneAndUpdateOptions<TEntity> options, CancellationToken cToken)
        {
            try
            {
                var collection = InitCollection();
                var result = await collection.FindOneAndUpdateAsync(filter, update, options, cToken);

                return result;
            }
            catch (Exception err)
            {
                _logger.Error(err, $"Update error. Details: {err.Message}");
            }

            return null!;
        }

        public virtual async Task<UpdateResult> UpdateMany(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update, UpdateOptions options, CancellationToken cToken)
        {
            try
            {
                var collection = InitCollection();
                var result = await collection.UpdateManyAsync(filter, update, options, cToken);

                return result;
            }
            catch (Exception err)
            {
                _logger.Error(err, $"UpdateMany error. Details: {err.Message}");
            }

            return null!;
        }

        public virtual async Task<TEntity> Delete(FilterDefinition<TEntity> filter, FindOneAndDeleteOptions<TEntity> options, CancellationToken cToken)
        {
            try
            {
                var collection = InitCollection();
                var result = await collection.FindOneAndDeleteAsync(filter, options, cToken);

                return result;
            }
            catch (Exception err)
            {
                _logger.Error(err, $"Delete error. Details: {err.Message}");
            }

            return null!;
        }

        public virtual async Task<DeleteResult> DeleteMany(FilterDefinition<TEntity> filter, DeleteOptions options, CancellationToken cToken)
        {
            try
            {
                var collection = InitCollection();
                var result = await collection.DeleteManyAsync(filter, options, cToken);

                return result;
            }
            catch (Exception err)
            {
                _logger.Error(err, $"DeleteMany error. Details: {err.Message}");
            }

            return null!;
        }

        protected IMongoCollection<TEntity> InitCollection()
        {
            var collection = _mongoClient
                .GetDatabase(_dbName)
                .GetCollection<TEntity>(_collectionName);

            return collection;
        }

    }
}
