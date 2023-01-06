using LoggerService.Models;

namespace LoggerService.Interfaces
{
    public interface IDatabase
    {
        Task<bool> Add<TEntity>(TEntity entity);
        Task<IEnumerable<TEntity>> GetAll<TEntity>();
    }
}
