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
                var filteredPost = new List<Post>();
                var newPosts = await Task.Run(() => subreddit.Posts.GetNew(afterFullname));

                if (newPosts.Count == 0)
                {
                    break;
                }

                if (skipFilter != null)
                {
                    filteredPost = newPosts.SkipWhile(skipFilter).ToList();
                }

                if (filteredPost.Count == 0 && skipFilter != null)
                {
                    afterFullname = newPosts.LastOrDefault()?.Fullname ?? "";
                    await Task.Delay(1000); // to prevent a spam reddit API
                    continue;
                }

                if (takeFilter != null)
                {
                    filteredPost = filteredPost.TakeWhile(takeFilter).ToList();
                }

                if (filteredPost.Count == 0 || filteredPost.Last()?.Fullname == afterFullname)
                {
                    break;
                }

                posts.AddRange(filteredPost);
                afterFullname = posts.LastOrDefault()?.Fullname ?? "";

                await Task.Delay(1000); // to prevent a spam reddit API
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
