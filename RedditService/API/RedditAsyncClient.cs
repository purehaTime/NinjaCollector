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

        public async Task<IEnumerable<Post>> GetPostsByFilter(Subreddit subreddit, Func<Post, bool> skipFilter ,Func<Post, bool> takeFilter)
        {
            var afterFullname = "";
            var posts = new List<Post>();

            while (true)
            {
                var newPosts = await Task.Run(() => subreddit.Posts.GetNew(afterFullname));

                if (newPosts.Count == 0)
                {
                    break;
                }

                if (skipFilter != null)
                {
                    newPosts = newPosts.SkipWhile(skipFilter).ToList();
                }

                if (takeFilter != null)
                {
                    newPosts = newPosts.TakeWhile(takeFilter).ToList();
                }

                afterFullname = posts.LastOrDefault()?.Fullname ?? "";
                if (newPosts.Last()?.Fullname == afterFullname)
                {
                    break;
                }

                posts.AddRange(newPosts);
            }

            return posts;
        }

        private async Task<RedditClient> GetRedditClient()
        {
            var configs = _config.GetRedditConfig();
            var token = await _session.GetAccessToken();

            return new RedditClient(configs.ClientId, null, configs.AppSecret, token);
        }
    }
}
