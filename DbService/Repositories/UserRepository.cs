using DbService.Interfaces;
using DbService.Models;
using MongoDB.Driver;
using ILogger = Serilog.ILogger;

namespace DbService.Repositories
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(IMongoClient client, IDbConfiguration dbConfig, ILogger logger) : base(client, dbConfig, logger)
        {
        }
    }
}
