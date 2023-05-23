using GrpcHelper.DbService;
using GrpcHelper.Interfaces;
using MainService.Interfaces;
using ModelsHelper.Mapping;
using ILogger = Serilog.ILogger;
using PosterSettings = ModelsHelper.Models.PosterSettings;

namespace MainService.Services
{
    public class PosterService : IPosterService
    {
        private readonly IDatabaseServiceClient _dbClient;
        private readonly ILogger _logger;

        public PosterService(IDatabaseServiceClient dbClient, ILogger logger)
        {
            _dbClient = dbClient;
            _logger = logger;
        }

        public async Task<bool> SavePosterSettings(PosterSettings posterSettings)
        {
            try
            {
                var result = await _dbClient.SavePosterSettings(posterSettings.ToGrpcData());
                return result;
            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
            }

            return false;
        }

        public async Task<List<PosterSettings>> GetAllPosterSettings()
        {
            try
            {
                var result = await _dbClient.GetPosterSettings(new PosterSettingsRequest());
                return result.Select(s => s.ToModel()).ToList();
            }
            catch (Exception err)
            {
                _logger.Error(err.Message, err);
            }

            return null;
        }

        public async Task<PosterSettings> GetPosterSettings(string settingId)
        {
            var result = await _dbClient.GetPosterSettings(new PosterSettingsRequest
            {
                SettingsId = settingId
            });

            return result.FirstOrDefault(f => f.Id == settingId)?.ToModel();
        }

        public async Task<bool> DeletePosterSettings(string id)
        {
            var result = await _dbClient.DeletePosterSettings(id);
            return result;
        }
    }
}
