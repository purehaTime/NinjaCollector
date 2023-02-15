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

        public async Task<IEnumerable<Post>> GetPostsUntilNow(string subReddit, DateTime fromDate)
        {
            var sub = await _client.GetSubreddit(subReddit);
            var posts = await _client.GetPostsBetweenDates(sub, fromDate, DateTime.UtcNow);

            return posts;
        }
    }
}
