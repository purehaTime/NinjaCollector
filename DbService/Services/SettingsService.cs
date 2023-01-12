using DbService.Interfaces;
using DbService.Models;
using DbService.Repositories;
using MongoDB.Driver;
using ILogger = Serilog.ILogger;

namespace DbService.Services
{
    public class SettingsService : ISettingsService
    {
        private SettingsRepository _repository;

        private ILogger _logger;

        public SettingsService(SettingsRepository settingsRepository, ILogger logger)
        {
            _repository = settingsRepository;
            _logger = logger;
        }
        
        public async Task<List<ParserSettings>> GetSettings(string source)
        {
            var filter = Builders<ParserSettings>.Filter.Eq("Source", source);
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
