using Reddit;
using RedditService.Interfaces;
using Post = Reddit.Controllers.Post;
using Subreddit = Reddit.Controllers.Subreddit;

namespace RedditService.API
{
    public class RedditAsyncClient : IRedditAsyncClient
    {
        private readonly IRedditSession _session;
        private readonly IRedditConfig _config;

        public RedditAsyncClient(IRedditSession session, IRedditConfig config)
        {
            _session = session;
            _config = config;
        }

        public async Task<Subreddit> GetSubreddit(string subredditName)
        {
            var client = await GetRedditClient();
            var subreddit = await Task.Run(() => client.Subreddit(subredditName).About());

            return subreddit;
        }

        public async Task<Post> GetLastPost(Subreddit subreddit)
        {
            var post = await Task.Run(() => subreddit.Posts.New.First());
            return post;
        }

        public async Task<List<Post>> GetNewPosts(Subreddit subreddit, string afterFullName)
        {
            var newPosts = await Task.Run(() => subreddit.Posts.GetNew(afterFullName));
            return newPosts;
        }

        /// <summary>
        /// to prevent a spam reddit API.
        /// </summary>
        /// <returns></returns>
        public async Task Hold()
        {
            var timeout = _config.GetRedditConfig();
            await Task.Delay(timeout.AntiSpamTimeout);
        }

        private async Task<RedditClient> GetRedditClient()
        {
            var configs = _config.GetRedditConfig();
            var token = await _session.GetAccessToken();

            return new RedditClient(configs.ClientId, null, configs.AppSecret, token);
        }
    }
}
