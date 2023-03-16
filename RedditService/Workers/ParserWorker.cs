using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using GrpcHelper.DbService;
using GrpcHelper.Interfaces;
using RedditService.Interfaces;
using RedditService.Model;
using Worker.Interfaces;
using Worker.Model;
using ILogger = Serilog.ILogger;

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
        public async Task<List<Settings>> Init()
        {
            var settings = await _dbClient.GetParserSettings(new ParserSettingsRequest
            {
                Source = "reddit",
            });

            var result = new List<Settings>
            {
                GetDefaultSettings()
            };
            if (settings.Count > 0)
            {
                result = settings.Select(s => new Settings
                {
                    Counts = s.PostsCount,
                    Timeout = s.Timeout,
                    Hold = s.Hold,
                    ApiName = s.Source,
                    RetryAfterErrorCount = 3,
                    Id = s.Id,
                    ByLastPostId = s.StartFromPastPost,
                    TagsForPosts = s.TagsForPost.ToList(),
                    ForGroup = s.Group,
                    FromDate = s.FromDate.ToDateTime(),
                    UntilDate = s.UntilDate.ToDateTime(),
                    FromPostId = s.LastPostId,
                }).ToList();
            }

            return result;
        }

        public Task<Settings> LoadSettings(string settingsId)
        {
            throw new NotImplementedException();
        }

        public async Task<Settings> Run(Settings settings)
        {
            var contents = new List<Content>();

            var posts = settings.ByLastPostId && !string.IsNullOrEmpty(settings.UntilPostId)
                ? await _redditService.GetPostsUntilPostId(settings.ForGroup, settings.UntilPostId)
                : await _redditService.GetPostsBetweenDates(settings.ForGroup, settings.FromDate ?? DateTime.UtcNow, settings.UntilDate ?? DateTime.MinValue);

            contents.AddRange(posts);

            var savedResult = await _dbClient.AddPosts(new PostModel
            {
                Posts =
                {
                    contents.Select(s => new Post
                    {
                        Description = s.Description ?? "",
                        Group = settings.ForGroup ?? "",
                        Text = s.Text ?? "",
                        OriginalLink = s.OriginalLink ?? "",
                        Source = "reddit",
                        Title = s.Title,
                        UserName = s.UserName ?? "",
                        PostDate = Timestamp.FromDateTime(s.Created),
                        Images = { s.Images.Select(img => ImageMapper(img, settings.TagsForPosts)) },
                        Tags = { settings.TagsForPosts }
                    })
                }
            });

            if (!savedResult)
            {
                settings.Disabled = true;
                _logger.Error($"Can't save posts for {settings.ForGroup}");
                return settings;
            }

            if (settings.ContinueMonitoring)
            {
                settings.ByLastPostId = true;
                if (contents.Count > 0)
                {
                    settings.UntilPostId = contents.First().Id;
                }
                else
                {
                    settings.FromDate = DateTime.UtcNow;
                }
            }
            else
            {
                settings.Disabled = true;
            }

            await UpdateSettings(settings);

            return settings;
        }

        private async Task UpdateSettings(Settings oldSettings)
        {

            var result = await _dbClient.SaveParserSettings(new ParserSettingsModel
            {
                Id = oldSettings.Id ?? "",
                TagsForPost = { oldSettings.TagsForPosts },
                Timeout = oldSettings.Timeout,
                Hold = oldSettings.Hold,
                Source = "reddit",
                LastPostId = oldSettings.UntilPostId,
                StartFromPastPost = oldSettings.ByLastPostId,
            });

            if (!result)
            {
                _logger.Error($"Can't save settings for reddit. Settings id: {oldSettings.Id}");
            }
        }

        private Settings GetDefaultSettings()
        {
            return new Settings
            {
                ApiName = "reddit",
                ForGroup = "games",
                Counts = 0,
                Hold = 200000,
                Timeout = 30000,
                RetryAfterErrorCount = 3,
                ByLastPostId = false,
                //FromDate = DateTime.UtcNow - TimeSpan.FromDays(2),
                UntilDate = DateTime.UtcNow - TimeSpan.FromDays(1),
                UntilPostId = null,
                ContinueMonitoring = true,
                FromPostId = null,
                TagsForPosts = new List<string>(),
                Disabled = false,
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
    }
}
