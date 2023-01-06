using DbService.Interfaces;
using MongoDB.Driver;
using ILogger = Serilog.ILogger;

namespace DbService.Repositories
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {

        private readonly ILogger _logger;

        public BaseRepository(IMongoClient client, string dbName, string collectionName, ILogger logger)
        {
            _logger = logger;
        }

        public virtual Task<TEntity> Find(FilterDefinition<TEntity> filter, FindOptions options, CancellationToken cToken)
        {
            throw new NotImplementedException();
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

    }
}
