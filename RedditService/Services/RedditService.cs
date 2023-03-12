using Reddit.Controllers;
using RedditService.Interfaces;
using RedditService.Model;

namespace RedditService.Services
{
    public class RedditService : IRedditService
    {
        private readonly IRedditApiClient _apiClient;
        private readonly IParserService _parserService;

        public RedditService(IRedditApiClient apiClient, IParserService parserService)
        {
            _apiClient = apiClient;
            _parserService = parserService;
        }

        public async Task<Content> GetLastPost(string subReddit)
        {
            var post = await _apiClient.GetLastPost(subReddit);
            var content = await _parserService.ParsePost(post);

            return content;
        }

        public async Task<IEnumerable<Content>> GetPostsToDate(string subReddit, DateTime toDate)
        {
            var posts = await _apiClient.GetPostsBetweenDates(subReddit, DateTime.UtcNow, toDate);
            var contents = await ContentMapper(posts);

            return contents;
        }

        public async Task<IEnumerable<Content>> GetPostsBetweenDates(string subReddit, DateTime fromDate, DateTime untilDate)
        {
            var posts = await _apiClient.GetPostsBetweenDates(subReddit, fromDate, untilDate);
            var contents = await ContentMapper(posts);

            return contents;
        }

        public async Task<IEnumerable<Content>> GetPostsUntilPostId(string subReddit, string postId)
        {
            var posts = await _apiClient.GetPostsFromPostIdUntilPostId(subReddit, null, postId);
            var contents = await ContentMapper(posts);

            return contents;
        }

        private async Task<IEnumerable<Content>> ContentMapper(IEnumerable<Post> posts)
        {
            var contents = new List<Content>();
            foreach (var post in posts) //don't wanna spam reddit, so what it is not a parallel
            {
                var parsed = await _parserService.ParsePost(post);
                contents.Add(parsed);
            }

            return contents;
        }
    }
}
