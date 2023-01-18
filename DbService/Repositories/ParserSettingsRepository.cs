using DbService.Interfaces;
using DbService.Models;
using MongoDB.Driver;
using ILogger = Serilog.ILogger;

namespace DbService.Repositories
{
    public class ParserSettingsRepository : BaseRepository<ParserSettings>
    {
        public ParserSettingsRepository(IMongoClient client, IDbConfiguration dbConfig, ILogger logger)
            : base(client, dbConfig, logger)
        {

        }
    }
}
