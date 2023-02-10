using Reddit.Controllers;
using RedditService.Interfaces;
using RedditService.Model;

namespace RedditService.API
{
    public class RedditApiClient : IRedditApiClient
    {
        private IRedditAsyncClient _client;

        public RedditApiClient(IRedditAsyncClient client)
        {
            _client = client;
        }

        public async Task<Content> GetLastPost(string subReddit)
        {
            var sub = await _client.GetSubreddit(subReddit);
            var post = await _client.GetLastPost(sub);

            return new Content
            {
                Description = post.Listing.SelfText,
                Title = post.Title
            };
        }

        public async Task<List<Content>> GetPostsUntilNow(string subReddit, DateTime fromDate)
        {
            var sub = await _client.GetSubreddit(subReddit);

            var posts = await _client.GetPostsBetweenDates(sub, fromDate, DateTime.UtcNow);

            var comments = posts.Take(50).Select(s => s.Comments.Comment?.About());

            return comments.Select(s => new Content
            {
                Title = s.Author,
            }).ToList();
        }
    }
}
