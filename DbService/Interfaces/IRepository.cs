using MongoDB.Driver;

namespace DbService.Interfaces
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        Task<TEntity> Find (FilterDefinition<TEntity> filter, FindOptions options, CancellationToken cToken);
        Task<TEntity> Insert(TEntity entity, InsertOneOptions options, CancellationToken cToken);
        Task<TEntity> Update (FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update, UpdateOptions options, CancellationToken cToken);
        Task<TEntity> Delete(FilterDefinition<TEntity> filter, DeleteOptions options, CancellationToken cToken);


        Task<IEnumerable<TEntity>> FindMany(FilterDefinition<TEntity> filter, FindOptions options, CancellationToken cToken);
        Task<bool> InsertMany(IEnumerable<TEntity> entities, InsertOneOptions options, CancellationToken cToken);
        Task<UpdateResult> UpdateMany(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update, UpdateOptions options, CancellationToken cToken);
        Task<bool> DeleteMany(FilterDefinition<TEntity> filter, DeleteOptions options, CancellationToken cToken);
    }
}
