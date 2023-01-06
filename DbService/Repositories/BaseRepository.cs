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
            var collection = InitCollection();

            var result = await collection.Find(filter, options).FirstOrDefaultAsync(cToken);

            return result;
        }

        public virtual Task<IEnumerable<TEntity>> FindMany(FilterDefinition<TEntity> filter, FindOptions options, CancellationToken cToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TEntity> Insert(TEntity entity, InsertOneOptions options, CancellationToken cToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> InsertMany(IEnumerable<TEntity> entities, InsertOneOptions options, CancellationToken cToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TEntity> Update(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update, UpdateOptions options, CancellationToken cToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<UpdateResult> UpdateMany(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update, UpdateOptions options, CancellationToken cToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TEntity> Delete(FilterDefinition<TEntity> filter, DeleteOptions options, CancellationToken cToken)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> DeleteMany(FilterDefinition<TEntity> filter, DeleteOptions options, CancellationToken cToken)
        {
            throw new NotImplementedException();
        }

        protected IMongoCollection<TEntity> InitCollection()
        {
            var db = _mongoClient
                .GetDatabase(_dbName)
                .GetCollection<TEntity>(_collectionName);

            return db;
        }

    }
}
