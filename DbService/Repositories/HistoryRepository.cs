using DbService.Interfaces;
using DbService.Models;
using MongoDB.Driver;
using ILogger = Serilog.ILogger;

namespace DbService.Repositories
{
    public class HistoryRepository : BaseRepository<History>
    {
        public HistoryRepository(IMongoClient client, IDbConfiguration dbConfig, ILogger logger) : base(client, dbConfig, logger)
        {
        }
    }
}
