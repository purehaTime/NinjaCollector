using Reddit.Controllers;
using RedditService.Interfaces;

namespace RedditService.API
{
    public class RedditApiClient : IRedditApiClient
    {
        private IRedditAsyncClient _client;

        public RedditApiClient(IRedditAsyncClient client)
        {
            _client = client;
        }

        public async Task<Post> GetLastPost(string subReddit)
        {
            var sub = await _client.GetSubreddit(subReddit);
            var post = await _client.GetLastPost(sub);

            return post;
        }

        public async Task<IEnumerable<Post>> GetPostsBetweenDates(string subReddit, DateTime fromDate, DateTime toDate)
        {
            var sub = await _client.GetSubreddit(subReddit);
            var posts = await GetPostsByFilter(sub, post => post.Created >= fromDate, post => post.Created >= toDate);

            return posts;
        }

        public async Task<IEnumerable<Post>> GetPostsFromPostIdUntilPostId(string subReddit, string fromPostId, string untilPostId)
        {
            var sub = await _client.GetSubreddit(subReddit);

            var takeFilter = fromPostId != null ?
                new Func<Post, bool>(post => post.Id == fromPostId) 
                : null;

            var posts = await GetPostsByFilter(sub, takeFilter, post => post.Id == untilPostId);

            return posts;
        }

        public async Task<IEnumerable<Post>> GetPostsFromPostIdUntilDate(string subReddit, string fromPostId, DateTime untilDate)
        {
            var sub = await _client.GetSubreddit(subReddit);

            var takeFilter = fromPostId != null ?
                new Func<Post, bool>(post => post.Id == fromPostId)
                : null;

            var posts = await GetPostsByFilter(sub, takeFilter, post => post.Created >= untilDate);

            return posts;
        }

        public async Task<IEnumerable<Post>> GetPostsFromDateUntilPostId(string subReddit, DateTime fromDate, string untilPostId)
        {
            var sub = await _client.GetSubreddit(subReddit);

            var posts = await GetPostsByFilter(sub, post => post.Created >= fromDate, post => post.Id == untilPostId);

            return posts;
        }

        private async Task<IEnumerable<Post>> GetPostsByFilter(Subreddit subreddit, Func<Post, bool> takeUntil,
            Func<Post, bool> skipUntil)
        {
            var afterFullname = "";
            var posts = new List<Post>();

            while (true)
            {
                var filteredPost = new List<Post>();
                var newPosts = await _client.GetNewPosts(subreddit, afterFullname);

                if (!newPosts.Any())
                {
                    break;
                }

                if (takeUntil != null)
                {
                    filteredPost = newPosts.TakeWhile(takeUntil).ToList();
                }

                if (filteredPost.Count == 0 && takeUntil != null)
                {
                    afterFullname = newPosts.LastOrDefault()?.Fullname ?? "";
                    await _client.Hold();
                    continue;
                }

                if (skipUntil != null)
                {
                    var tookPosts =  filteredPost.SkipWhile(skipUntil).ToList();
                    filteredPost = tookPosts;
                }

                if (filteredPost.Count == 0 || filteredPost.Last()?.Fullname == afterFullname)
                {
                    break;
                }

                posts.AddRange(filteredPost);
                afterFullname = posts.LastOrDefault()?.Fullname ?? "";
                await _client.Hold();
            }

            return posts;
        }
    }
}
