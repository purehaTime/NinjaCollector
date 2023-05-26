using MongoDB.Driver;

namespace DbService.Interfaces
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        Task<TEntity> Find (FilterDefinition<TEntity> filter, FindOptions options, CancellationToken cToken);
        Task<bool> Insert(TEntity entity, InsertOneOptions options, CancellationToken cToken);
        Task<bool> Update(FilterDefinition<TEntity> filter, TEntity entity, FindOneAndReplaceOptions<TEntity> update, CancellationToken cToken);
        Task<TEntity> Delete(FilterDefinition<TEntity> filter, FindOneAndDeleteOptions<TEntity> options, CancellationToken cToken);


        Task<IEnumerable<TEntity>> FindMany(FilterDefinition<TEntity> filter, FindOptions options, CancellationToken cToken);
        Task<bool> InsertMany(IEnumerable<TEntity> entities, InsertManyOptions options, CancellationToken cToken);
        Task<bool> UpdateMany(FilterDefinition<TEntity> filter, UpdateDefinition<TEntity> update, UpdateOptions options, CancellationToken cToken);
        Task<DeleteResult> DeleteMany(FilterDefinition<TEntity> filter, DeleteOptions options, CancellationToken cToken);
    }
}
