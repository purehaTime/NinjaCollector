using DbService.Interfaces;
using DbService.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using ILogger = Serilog.ILogger;

namespace DbService.Services
{
    public class HistoryService : IHistoryService
    {
        private readonly IRepository<History> _historyRepository;
        private readonly ILogger _logger;

        public HistoryService(IRepository<History> historyRepository, ILogger logger)
        {
            _historyRepository = historyRepository;
            _logger = logger;
        }

        public async Task<bool> SaveHistory(History history)
        {
            var result = await _historyRepository.Insert(history, null!, CancellationToken.None);
            if (!result)
            {
                _logger.Error($"Cant save history for {history.Source} : {history.Group}");
            }

            return result;
        }

        public async Task<IEnumerable<History>> GetHistory(IEnumerable<string> entities, string service,
            string forGroup)
        {
            var historyFilter = Builders<History>.Filter.In(x => x.EntityId, entities);
            historyFilter &= Builders<History>.Filter.Eq(e => e.Source, service);
            historyFilter &= Builders<History>.Filter.Eq(e => e.Group, forGroup);
            var histories = await _historyRepository.FindMany(historyFilter, null, CancellationToken.None);

            return histories;
        }
    }
}
