﻿using GrpcHelper.DbService;
using GrpcHelper.Interfaces;
using MainService.Interfaces;
using ModelsHelper.Mapping;
using ILogger = Serilog.ILogger;
using ParserSettings = ModelsHelper.Models.ParserSettings;

namespace MainService.Services
{
    public class ParserService : IParserService
    {
        private readonly IDatabaseServiceClient _dbClient;
        private readonly ILogger _logger;

        public ParserService(IDatabaseServiceClient dbClient, ILogger logger)
        {
            _dbClient = dbClient;
            _logger = logger;
        }

        public async Task<bool> SaveParserSettings(ParserSettings parserSettings)
        {
            try
            {
                var result = await _dbClient.SaveParserSettings(parserSettings.ToGrpcData());
                return result;
            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
            }

            return false;
        }

        public async Task<List<ParserSettings>> GetAllParserSettings()
        {
            try
            {
                var result = await _dbClient.GetParserSettings(new ParserSettingsRequest());
                return result.Select(s => s.ToModel()).ToList();
            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
            }

            return null;
        }

        public async Task<ParserSettings> GetParserSettings(string settingId)
        {
            var result = await _dbClient.GetParserSettings(new ParserSettingsRequest
            {
                SettingsId = settingId
            });

            return result.FirstOrDefault(f => f.Id == settingId)?.ToModel();
        }

        public async Task<bool> DeleteParserSettings(string id)
        {
            var result = await _dbClient.DeleteParserSettings(id);
            return result;
        }
    }
}
