using GrpcHelper.DbService;
using GrpcHelper.Interfaces;
using ModelsHelper.Mapping;
using ModelsHelper.Models;
using RedditService.Interfaces;
using Worker.Interfaces;
using ILogger = Serilog.ILogger;
using ParserSettings = ModelsHelper.Models.ParserSettings;
using Post = ModelsHelper.Models.Post;

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
            return settings.Count > 0 ? settings : new List<Settings> ();
        }

        public async Task<Settings> LoadSettings(string settingsId)
        {
            var settings = await GetSettings(settingsId);
            return settings.FirstOrDefault(f => f.Id == settingsId) ?? new ParserSettings();
        }

        public async Task<Settings> Run(Settings settings)
        {
            if (settings is not ParserSettings parserSettings)
            {
                return settings;
            }

            var contents = new List<Post>();

            var posts = parserSettings.ByLastPostId && !string.IsNullOrEmpty(parserSettings.UntilPostId)
                ? await _redditService.GetPostsUntilPostId(parserSettings.Group, parserSettings.UntilPostId, parserSettings.Filter)
                : await _redditService.GetPostsBetweenDates(parserSettings.Group, parserSettings.FromDate, parserSettings.UntilDate, parserSettings.Filter);

            contents.AddRange(posts);

            if (contents.Count > 0)
            {
                var data = contents.Select(s => s.ToGrpcData(parserSettings.Tags));
                var savedResult = await _dbClient.AddPosts(data.ToList());

                if (!savedResult)
                {
                    parserSettings.Disabled = true;
                    _logger.Error($"Can't save posts for {parserSettings.Group}");
                }

                if (parserSettings.ContinueMonitoring)
                {
                    parserSettings.ByLastPostId = true;
                    parserSettings.UntilPostId = contents.First().PostId;

                    await UpdateSettings(parserSettings);
                    return settings;
                }
            }

            parserSettings.Disabled = !parserSettings.ContinueMonitoring;
            await UpdateSettings(parserSettings);

            return parserSettings;
        }

        private async Task UpdateSettings(ParserSettings oldSettings)
        {
            var result = await _dbClient.SaveParserSettings(oldSettings.ToGrpcData());

            if (!result)
            {
                _logger.Error($"Can't save settings for reddit. Settings id: {oldSettings.Id}");
            }
        }

        private async Task<List<Settings>> GetSettings(string settingsId)
        {
            var settings = await _dbClient.GetParserSettings(new ParserSettingsRequest
            {
                Source = "reddit",
                SettingsId = settingsId ?? string.Empty,
            }) ?? new List<ParserSettingsModel>();

            var result = settings?.Where(w => !w.Disabled).Select(s => s.ToModel());

            return new List<Settings>(result);
        }
    }
}
