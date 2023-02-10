using Reddit;
using Reddit.Controllers;
using RedditService.Interfaces;
using ILogger = Serilog.ILogger;

namespace RedditService.API
{
    public class RedditAsyncClient : IRedditAsyncClient
    {
        private readonly IRedditSession _session;
        private readonly IRedditConfig _config;
        private readonly ILogger _logger;

        public RedditAsyncClient(IRedditSession session, IRedditConfig config, ILogger logger)
        {
            _session = session;
            _config = config;
            _logger = logger;
        }

        public async Task<Subreddit> GetSubreddit(string subredditName)
        {
            var client = await GetRedditClient();
            var subreddit = await Task.Run(() => client.Subreddit(subredditName).About());

            return subreddit;
        }

        public async Task<IEnumerable<Post>> GetPostsBetweenDates(Subreddit subreddit, DateTime fromDate, DateTime toDate)
        {
            var posts = await Task.Run(() => subreddit.Posts.New.Where(w => w.Created >= fromDate && w.Created <= toDate));

            return posts;
        }

        public async Task<Post> GetLastPost(Subreddit subreddit)
        {
            var post = await Task.Run(() => subreddit.Posts.New.First());

            return post;
        }

        private async Task<RedditClient> GetRedditClient()
        {
            var configs = _config.GetRedditConfig();
            var token = await _session.GetAccessToken();

            _logger.Information("Create new reddit client");
            return new RedditClient(configs.ClientId, null, configs.AppSecret, token);
        }
    }
}
