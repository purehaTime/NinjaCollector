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
            var posts = await _client.GetPostsByFilter(sub, post => post.Created >= fromDate, post => post.Created >= toDate);

            return posts;
        }

        public async Task<IEnumerable<Post>> GetPostsFromPostIdUntilPostId(string subReddit, string fromPostId, string untilPostId)
        {
            var sub = await _client.GetSubreddit(subReddit);

            var skipFilter = fromPostId != null ?
                new Func<Post, bool>(post => post.Id != fromPostId) 
                : null;

            var posts = await _client.GetPostsByFilter(sub, skipFilter, post => post.Id != untilPostId);

            return posts;
        }

        public async Task<IEnumerable<Post>> GetPostsFromPostIdUntilDate(string subReddit, string fromPostId, DateTime untilDate)
        {
            var sub = await _client.GetSubreddit(subReddit);

            var skipFilter = fromPostId != null ?
                new Func<Post, bool>(post => post.Id != fromPostId)
                : null;

            var posts = await _client.GetPostsByFilter(sub, skipFilter, post => post.Created != untilDate);

            return posts;
        }

        public async Task<IEnumerable<Post>> GetPostsFromDateUntilPostId(string subReddit, DateTime fromDate, string untilPostId)
        {
            var sub = await _client.GetSubreddit(subReddit);

            var posts = await _client.GetPostsByFilter(sub, post => post.Created <= fromDate, post => post.Id != untilPostId);

            return posts;
        }
    }
}
