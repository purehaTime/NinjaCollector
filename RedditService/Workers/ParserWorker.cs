using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using GrpcHelper.DbService;
using GrpcHelper.Interfaces;
using Models.Mapping;
using Models.Models;
using RedditService.Interfaces;
using RedditService.Model;
using Worker.Interfaces;
using ILogger = Serilog.ILogger;
using ParserSettings = Models.Models.ParserSettings;

namespace RedditService.Workers
{
    public class ParserWorker : IWorker
    {
        private IRedditService _redditService;
        private IDatabaseServiceClient _dbClient;
        private ILogger _logger;

        public ParserWorker(IRedditService redditService, IDatabaseServiceClient dbClient, ILogger logger)
        {
            _redditService = redditService;
            _dbClient = dbClient;
            _logger = logger;
            Name = "Reddit_Parser";
        }

        public string Name { get; }

        /// <summary>
        /// should be separate for different interface and class
        /// </summary>
        /// <returns></returns>
        public async Task<List<ParserSettings>> Init()
        {
            var settings = await GetSettings(null);
            return settings.Count > 0 ? settings : new List<ParserSettings> { GetDefaultSettings() };
        }

        public async Task<ParserSettings> LoadSettings(string settingsId)
        {
            var settings = await GetSettings(settingsId);
            return settings.FirstOrDefault(f => f.Id == settingsId) ?? GetDefaultSettings();
        }

        public async Task<ParserSettings> Run(ParserSettings settings)
        {
            var contents = new List<Content>();

            var posts = settings.ByLastPostId && !string.IsNullOrEmpty(settings.UntilPostId)
                ? await _redditService.GetPostsUntilPostId(settings.Group, settings.UntilPostId, settings.Filter)
                : await _redditService.GetPostsBetweenDates(settings.Group, settings.FromDate, settings.UntilDate, settings.Filter);

            contents.AddRange(posts);

            if (contents.Count > 0)
            {
                var savedResult = await _dbClient.AddPosts(new PostModel
                {
                    Posts =
                    {
                        contents.Select(s => new Post
                        {
                            Description = s.Description ?? "",
                            Group = settings.Group ?? "",
                            Text = s.Text ?? "",
                            OriginalLink = s.OriginalLink ?? "",
                            Source = "reddit",
                            Title = s.Title,
                            UserName = s.UserName ?? "",
                            PostDate = Timestamp.FromDateTime(s.Created),
                            Images = { s.Images.Select(img => ImageMapper(img, settings.Tags)) },
                            Tags = { settings.Tags }
                        })
                    }
                });

                if (!savedResult)
                {
                    settings.Disabled = true;
                    _logger.Error($"Can't save posts for {settings.Group}");
                }

                if (settings.ContinueMonitoring)
                {
                    settings.ByLastPostId = true;
                    settings.UntilPostId = contents.First().Id;

                    await UpdateSettings(settings);
                    return settings;
                }
            }

            settings.Disabled = true;
            await UpdateSettings(settings);

            return settings;
        }

        private async Task UpdateSettings(ParserSettings oldSettings)
        {
            var result = await _dbClient.SaveParserSettings(oldSettings.ToGrpcData());

            if (!result)
            {
                _logger.Error($"Can't save settings for reddit. Settings id: {oldSettings.Id}");
            }
        }

        private ParserSettings GetDefaultSettings()
        {
            return new ParserSettings
            {
                Source = Name,
                Group = "games",
                Counts = 0,
                Hold = 20000,
                Timeout = 30000,
                RetryAfterErrorCount = 3,
                ByLastPostId = false,
                FromDate = DateTime.UtcNow,
                UntilDate = DateTime.UtcNow - TimeSpan.FromDays(1),
                UntilPostId = null,
                ContinueMonitoring = true,
                FromPostId = null,
                Tags = new List<string>(),
                Disabled = false,
                Filter = new Models.Models.Filter()
            };
        }

        private GrpcHelper.DbService.Image ImageMapper(ImageContainer image, IEnumerable<string> tags)
        {
            return new GrpcHelper.DbService.Image
            {
                Description = image.Image.Description,
                OriginalLink = image.Image.DirectLink,
                Name = image.Image.Name,
                Width = image.Image.Width,
                Height = image.Image.Height,
                Tags = { tags },
                File = ByteString.CopyFrom(image.Data),
            };
        }

        private async Task<List<ParserSettings>> GetSettings(string settingsId)
        {
            var settings = await _dbClient.GetParserSettings(new ParserSettingsRequest
            {
                Source = Name,
                SettingsId = settingsId ?? string.Empty,
            }) ?? new List<ParserSettingsModel>();

            return settings?.Where(w => !w.Disabled).Select(s => s.ToModel()).ToList();
        }
    }
}
