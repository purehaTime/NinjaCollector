﻿using DbService.Interfaces;
using DbService.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using ILogger = Serilog.ILogger;

namespace DbService.Services
{
    public class SettingsService : ISettingsService
    {
        private IRepository<DbParserSettings> _parserRepository;
        private IRepository<PosterSettings> _posterRepository;

        private ILogger _logger;

        public SettingsService(IRepository<DbParserSettings> parserRepository, IRepository<PosterSettings> posterRepository, ILogger logger)
        {
            _parserRepository = parserRepository;
            _posterRepository = posterRepository;
            _logger = logger;
        }
        
        public async Task<List<DbParserSettings>> GetParserSettings(string source, ObjectId? settingsId)
        {
            var filter = Builders<DbParserSettings>.Filter.Eq(e => e.Source, source);
            if (settingsId != null)
            {
                filter &= Builders<DbParserSettings>.Filter.Eq(e => e.DbId, settingsId);
            }
            var results = await _parserRepository.FindMany(filter, null!, CancellationToken.None);

            return results.ToList();
        }

        public async Task<bool> SaveParserSettings(DbParserSettings settings)
        {
            var result = await _parserRepository.Insert(settings, null!, CancellationToken.None);

            if (!result)
            {
                 _logger.Error($"Cant save settings for parser {settings.Source}");
            }
            return result;
        }

        public async Task<List<PosterSettings>> GetPosterSettings(string service, string forGroup)
        {
            var filter = Builders<PosterSettings>.Filter.Eq(e => e.Service, service);
            filter &= Builders<PosterSettings>.Filter.Eq(e => e.ForGroup, forGroup);
            var results = await _posterRepository.FindMany(filter, null!, CancellationToken.None);

            return results.ToList();
        }

        public async Task<bool> SavePosterSettings(PosterSettings settings)
        {
            var result = await _posterRepository.Insert(settings, null!, CancellationToken.None);

            if (!result)
            {
                _logger.Error($"Cant save settings for poster {settings.Service}");
            }
            return result;
        }
    }
}
