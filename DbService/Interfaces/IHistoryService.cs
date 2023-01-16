using DbService.Models;
using MongoDB.Bson;

namespace DbService.Interfaces
{
    public interface IHistoryService
    {
        public Task<bool> SaveHistory(History history);

        public Task<IEnumerable<History>> GetHistory(IEnumerable<ObjectId> entities, string service,
            string forGroup);
    }
}
