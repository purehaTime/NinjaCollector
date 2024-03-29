﻿using DbService.Interfaces;
using DbService.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using ILogger = Serilog.ILogger;

namespace DbService.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly IRepository<ParserSettings> _parserRepository;
        private readonly IRepository<PosterSettings> _posterRepository;

        private ILogger _logger;

        public SettingsService(IRepository<ParserSettings> parserRepository, IRepository<PosterSettings> posterRepository, ILogger logger)
        {
            _parserRepository = parserRepository;
            _posterRepository = posterRepository;
            _logger = logger;
        }
        
        public async Task<List<ParserSettings>> GetParserSettings(string source, ObjectId? settingsId)
        {
            var filter = Builders<ParserSettings>.Filter.Empty;
            if (!string.IsNullOrEmpty(source))
            {
                filter = Builders<ParserSettings>.Filter.Eq(e => e.Source, source);
            }

            if (settingsId != null)
            {
                filter &= Builders<ParserSettings>.Filter.Eq(e => e.Id, settingsId);
            }
            var results = await _parserRepository.FindMany(filter, null, CancellationToken.None);

            return results?.ToList() ?? new List<ParserSettings>();
        }

        public async Task<bool> SaveParserSettings(ParserSettings settings)
        {
            var filter = Builders<ParserSettings>.Filter.Eq(e => e.Id, settings.Id);
            var existingSetting = await _parserRepository.Find(filter, null, CancellationToken.None);

            if (existingSetting != null)
            {
                filter = Builders<ParserSettings>.Filter.Eq(e => e.Id, settings.Id);
                var updateResult = await _parserRepository.Update(filter, settings, null, CancellationToken.None);
                return updateResult;
            }

            var insertResult = await _parserRepository.Insert(settings, null, CancellationToken.None);
            return insertResult;
        }

        public async Task<bool> RemoveParserSettings(string id)
        {
            var filter = Builders<ParserSettings>.Filter.Eq(e => e.Id, ObjectId.Parse(id));
            var result = await _parserRepository.Delete(filter, null, CancellationToken.None);

            if (result == null)
            {
                _logger.Error($"Cant delete settings for parser {id}");
                return false;
            }
            return true;
        }

        public async Task<List<PosterSettings>> GetPosterSettings(string source, ObjectId? settingsId)
        {
            var filter = Builders<PosterSettings>.Filter.Empty;
            if (!string.IsNullOrEmpty(source))
            {
                filter = Builders<PosterSettings>.Filter.Eq(e => e.Source, source);
            }

            if (settingsId != null)
            {
                filter &= Builders<PosterSettings>.Filter.Eq(e => e.Id, settingsId);
            }
            var results = await _posterRepository.FindMany(filter, null, CancellationToken.None);

            return results.ToList();
        }

        public async Task<bool> SavePosterSettings(PosterSettings settings)
        {
            var filter = Builders<PosterSettings>.Filter.Eq(e => e.Id, settings.Id);
            var existingSetting = await _posterRepository.Find(filter, null, CancellationToken.None);

            if (existingSetting != null)
            {
                filter = Builders<PosterSettings>.Filter.Eq(e => e.Id, settings.Id);
                var updateResult = await _posterRepository.Update(filter, settings, null, CancellationToken.None);
                return updateResult;
            }

            var insertResult = await _posterRepository.Insert(settings, null, CancellationToken.None);
            return insertResult;
        }

        public async Task<bool> RemovePosterSettings(string id)
        {
            var filter = Builders<PosterSettings>.Filter.Eq(e => e.Id, ObjectId.Parse(id));
            var result = await _posterRepository.Delete(filter, null, CancellationToken.None);

            if (result == null)
            {
                _logger.Error($"Cant delete settings for parser {id}");
                return false;
            }
            return true;
        }

        public async Task<PosterSettings> GetPosterSetting(string id)
        {
            var filter = Builders<PosterSettings>.Filter.Eq(e => e.Id, ObjectId.Parse(id));
            var result = await _posterRepository.Find(filter, null, CancellationToken.None);

            if (result == null)
            {
                _logger.Error($"Cant delete settings for parser {id}");
                return null;
            }
            return result;
        }
    }
}
