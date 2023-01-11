using DbService.Interfaces;
using DbService.Models;
using MongoDB.Driver;
using ILogger = Serilog.ILogger;

namespace DbService.Repositories
{
    public class SettingsRepository : BaseRepository<ParserSettings>
    {
        public SettingsRepository(IMongoClient client, IDbConfiguration dbConfig, ILogger logger)
            : base(client, dbConfig, logger)
        {

        }
    }
}
