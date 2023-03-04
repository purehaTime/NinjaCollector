using Google.Protobuf.WellKnownTypes;
using GrpcHelper.DbService;
using GrpcHelper.Interfaces;
using RedditService.Interfaces;
using Worker;
using Worker.Model;

namespace RedditService.Workers
{
    public class ParserWorker : IWorker
    {
        private IRedditService _redditService;
        private IDatabaseServiceClient _dbClient;

        public ParserWorker(IRedditService redditService, IDatabaseServiceClient dbClient)
        {
            _redditService = redditService;
            _dbClient = dbClient;
        }

        /// <summary>
        /// should be separate for different interface and class
        /// </summary>
        /// <returns></returns>
        public async Task<List<Settings>> Init()
        {
            await Task.Delay(1);

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
                    Counts = 0,
                    Timeout = s.Interval,
                    Hold = 1000,
                    ApiName = s.Source,
                    RetryAfterErrorCount = 4,
                }).ToList();
            }

            return result;
        }

        public async Task Run(Settings settings)
        {
            var post = await _redditService.GetLastPost(settings.ForGroup);

            await _dbClient.AddPost(new PostModel
            {
                Description = post.Description ?? "",
                Group = settings.ForGroup,
                Text = post.Text,
                OriginalLink = post.OriginalLink,
                Source = "reddit",
                Title = post.Title,
                UserName = post.UserName,
                PostDate = Timestamp.FromDateTime(post.Created),
                Images = {  new List<Image>() },
                Tags = { "test" }
            });
        }

        public void GetStatus()
        {
            throw new NotImplementedException();
        }

        private Settings GetDefaultSettings()
        {
            return new Settings
            {
                ApiName = "reddit",
                ForGroup = "games",
                Counts = 0,
                Hold = 2000,
                Timeout = 3000,
                RetryAfterErrorCount = 3
            };
        }
    }
}
