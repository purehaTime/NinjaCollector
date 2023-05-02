﻿using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using GrpcHelper.DbService;
using GrpcHelper.Interfaces;
using RedditService.Interfaces;
using RedditService.Model;
using Worker.Interfaces;
using Worker.Model;
using Filter = Worker.Model.Filter;
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
            var settings = await GetSettings(null);
            return settings.Count > 0 ? settings : new List<Settings>{ GetDefaultSettings() };
        }

        public async Task<Settings> LoadSettings(string settingsId)
        {
            var settings = await GetSettings(settingsId);
            return settings.FirstOrDefault(f => f.Id == settingsId) ?? GetDefaultSettings();
        }

        public async Task<Settings> Run(Settings settings)
        {
            var contents = new List<Content>();
            var filter = settings.Filters;

            var posts = settings.ByLastPostId && !string.IsNullOrEmpty(settings.UntilPostId)
                ? await _redditService.GetPostsUntilPostId(settings.ForGroup, settings.UntilPostId, filter)
                : await _redditService.GetPostsBetweenDates(settings.ForGroup, settings.FromDate ?? DateTime.UtcNow, settings.UntilDate ?? DateTime.MinValue, filter);

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
                            Group = settings.ForGroup ?? "",
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
                    _logger.Error($"Can't save posts for {settings.ForGroup}");
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

        private async Task UpdateSettings(Settings oldSettings)
        {
            var result = await _dbClient.SaveParserSettings(new ParserSettingsModel
            {
                Id = oldSettings.Id ?? "",
                Description = oldSettings.Description,
                Source = "reddit",
                Group = oldSettings.ForGroup,
                Timeout = oldSettings.Timeout,
                Hold = oldSettings.Hold,
                Counts = oldSettings.Counts,
                RetryAfterErrorCount = oldSettings.RetryAfterErrorCount,
                Tags = { oldSettings.Tags },
                FromDate = Timestamp.FromDateTime(oldSettings.FromDate ?? DateTime.UtcNow),
                UntilDate = Timestamp.FromDateTime(oldSettings.UntilDate ?? new DateTime()),
                UntilPostId = oldSettings.UntilPostId,
                ByLastPostId = oldSettings.ByLastPostId,
                ContinueMonitoring = oldSettings.ContinueMonitoring,
                Disabled = oldSettings.Disabled,
                Filters = DbFilterMap(oldSettings.Filters)
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
                Source = Name,
                ForGroup = "games",
                Counts = 0,
                Hold = 20000,
                Timeout = 30000,
                RetryAfterErrorCount = 3,
                ByLastPostId = false,
                //FromDate = DateTime.UtcNow - TimeSpan.FromDays(2),
                UntilDate = DateTime.UtcNow - TimeSpan.FromDays(1),
                UntilPostId = null,
                ContinueMonitoring = true,
                FromPostId = null,
                Tags = new List<string>(),
                Disabled = false,
                Filters = new Filter()
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

        private async Task<List<Settings>> GetSettings(string settingsId)
        {
            var settings = await _dbClient.GetParserSettings(new ParserSettingsRequest
            {
                Source = Name,
                SettingsId = settingsId ?? string.Empty,
            }) ?? new List<ParserSettingsModel>();

            return settings?.Where(w => !w.Disabled).Select(s => new Settings
            {
                Id = s.Id ?? "",
                Description = s.Description,
                Source = "reddit",
                ForGroup = s.Group,
                Timeout = s.Timeout,
                Hold = s.Hold,
                Counts = s.Counts,
                RetryAfterErrorCount = s.RetryAfterErrorCount,
                Tags = s.Tags.ToList(),
                FromDate = s.FromDate.ToDateTime(),
                UntilDate = s.UntilDate.ToDateTime(),
                UntilPostId = s.UntilPostId,
                ByLastPostId = s.ByLastPostId,
                ContinueMonitoring = s.ContinueMonitoring,
                Disabled = s.Disabled,
                Filters = FilterMapping(s.Filters)
            }).ToList();
        }

        private Filter FilterMapping(GrpcHelper.DbService.Filter map)
        {
            return new Filter
            {
                IgnoreVideo = map.IgnoreVideo,
                IgnoreRepost = map.IgnoreRepost,
                IgnoreAuthors = map.IgnoreAuthors.ToList(),
                IgnoreDescriptions = map.IgnoreDescriptions.ToList(),
                IgnoreTitles = map.IgnoreTitles.ToList(),
                IgnoreWords = map.IgnoreWords.ToList()
            };
        }

        private GrpcHelper.DbService.Filter DbFilterMap(Filter filter)
        {
            return new GrpcHelper.DbService.Filter
            {
                IgnoreVideo = filter.IgnoreVideo,
                IgnoreRepost = filter.IgnoreRepost,
                IgnoreAuthors = { filter.IgnoreAuthors },
                IgnoreDescriptions = { filter.IgnoreDescriptions },
                IgnoreTitles = { filter.IgnoreTitles.ToList() },
                IgnoreWords = { filter.IgnoreWords.ToList() }
            };
        }
    }
}
