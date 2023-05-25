using GrpcHelper.DbService;
using GrpcHelper.Interfaces;
using ModelsHelper.Mapping;
using ModelsHelper.Models;
using TelegramService.Interfaces;
using Worker.Interfaces;
using ILogger = Serilog.ILogger;
using Image = ModelsHelper.Models.Image;
using PosterSettings = ModelsHelper.Models.PosterSettings;

namespace TelegramService.Workers
{
    public class PosterWorker : IWorker
    {
        private readonly IDatabaseServiceClient _dbClient;
        private readonly ITelegramBotService _tgBot;
        private readonly ILogger _logger;

        public string Name { get; }

        public PosterWorker(IDatabaseServiceClient dbClient, ITelegramBotService tgBot, ILogger logger)
        {
            Name = "telegram_poster";
            _dbClient = dbClient;
            _logger = logger;
            _tgBot = tgBot;
        }

        
        public async Task<List<Settings>> Init()
        {
            var posters = await GetPosterSettings(null);
            return new List<Settings>(posters);
        }

        public async Task<Settings> LoadSettings(string settingsId)
        {
            var posters = await GetPosterSettings(null);
            return posters.FirstOrDefault();
        }

        public async Task<Settings> Run(Settings setting)
        {
            var posterSettings = (PosterSettings)setting;
            var chatId = posterSettings.Group;
            if (posterSettings.UseImagesOnly)
            {
                var image = await _dbClient.GetImage(new EntityRequest
                {
                    SettingsId = posterSettings.Id
                });

                if (image != null && image.File != null)
                {
                    await _tgBot.SendPost(chatId, null, new List<Image> { image.ToModel() });
                    return posterSettings;
                }

                _logger.Warning($"Images for {posterSettings.Id} empty or ending");
                posterSettings.ContinuePosting = false;
                await UpdateSettings(posterSettings);
                return posterSettings;
            }

            var post = await _dbClient.GetPost(new EntityRequest
            {
                SettingsId = posterSettings.Id
            });

            posterSettings.ContinuePosting = false;
            if (post != null && !string.IsNullOrEmpty(post.PostId))
            {
                var text = posterSettings.UseSettingsText ? posterSettings.TextForPost : post.Text;
                var images = post.Images.Select(s => s.ToModel());
                await _tgBot.SendPost(chatId, text, images.ToList());
                posterSettings.ContinuePosting = true;
            }

            await UpdateSettings(posterSettings);
            return posterSettings;
        }

        private async Task UpdateSettings(PosterSettings settings)
        {
            await _dbClient.SavePosterSettings(settings.ToGrpcData());
        }

        private async Task<List<PosterSettings>> GetPosterSettings(string settingsId)
        {
            var settings = await _dbClient.GetPosterSettings(new PosterSettingsRequest
            {
                Service = "telegram",
                SettingsId = settingsId,
            }) ?? new List<PosterSettingsModel>();

            var result = settings.Where(w => !w.Disabled).Select(s => s.ToModel());
            return new List<PosterSettings>(result);
        }
    }
}
