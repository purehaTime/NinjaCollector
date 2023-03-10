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
                _logger.Error($"Cant save history for {history.Service} : {history.ForGroup}");
            }

            return result;
        }

        public async Task<IEnumerable<History>> GetHistory(IEnumerable<ObjectId> entities, string service,
            string forGroup)
        {
            var historyFilter = Builders<History>.Filter
                .Where(w => entities.Any(a => a == w.EntityId));
            historyFilter &= Builders<History>.Filter.Eq(e => e.Service, service);
            historyFilter &= Builders<History>.Filter.Eq(e => e.ForGroup, forGroup);
            var histories = await _historyRepository.FindMany(historyFilter, null!, CancellationToken.None);

            return histories;
        }
    }
}
