using GrpcHelper.DbService;
using GrpcHelper.Interfaces;
using ModelsHelper.Mapping;
using ModelsHelper.Models;
using Worker.Interfaces;
using ILogger = Serilog.ILogger;

namespace TelegramService.Workers
{
    public class PosterWorker : IWorker
    {
        private readonly IDatabaseServiceClient _dbClient;
        private readonly ILogger _logger;

        public string Name { get; }

        public PosterWorker(IDatabaseServiceClient dbClient, ILogger logger)
        {
            Name = "telegram_poster";
            _dbClient = dbClient;
            _logger = logger;
        }

        
        public async Task<List<Settings>> Init()
        {
            var posters = await _dbClient.GetPosterSettings(new PosterSettingsRequest
            {
                Service = "telegram"
            });
            return new List<Settings>(posters.Select(s => s.ToModel()));
        }

        public async Task<Settings> LoadSettings(string settingsId)
        {
            var posters = await _dbClient.GetPosterSettings(new PosterSettingsRequest
            {
                Service = "telegram",
            });
            return posters.FirstOrDefault().ToModel();
        }

        public async Task<Settings> Run(Settings setting)
        {
            throw new NotImplementedException();
        }
    }
}
