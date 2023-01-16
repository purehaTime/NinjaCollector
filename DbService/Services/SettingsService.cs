using DbService.Interfaces;
using DbService.Models;
using MongoDB.Driver;
using ILogger = Serilog.ILogger;

namespace DbService.Services
{
    public class SettingsService : ISettingsService
    {
        private IRepository<ParserSettings> _repository;

        private ILogger _logger;

        public SettingsService(IRepository<ParserSettings> settingsRepository, ILogger logger)
        {
            _repository = settingsRepository;
            _logger = logger;
        }
        
        public async Task<List<ParserSettings>> GetSettings(string source)
        {
            var filter = Builders<ParserSettings>.Filter.Eq(e => e.Source, source);
            var results = await _repository.FindMany(filter, null!, CancellationToken.None);

            _logger.Information($"Get settings for source: {source}");
            return results.ToList();
        }

        public async Task<bool> SaveSettings(ParserSettings settings)
        {
            var result = await _repository.Insert(settings, null!, CancellationToken.None);

            if (!result)
            {
                 _logger.Error($"Cant save settings for {settings.Source}");
            }
            return result;
        }
    }
}
