using System.Collections.Concurrent;
using Reddit.Controllers;
using RedditService.Interfaces;
using RedditService.Model;
using Worker.Model;

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

        public async Task<Content> GetLastPost(string subReddit, Filter filter)
        {
            var post = await _apiClient.GetLastPost(subReddit);
            var content = await _parserService.ParsePost(post, filter);

            return content;
        }

        public async Task<IEnumerable<Content>> GetPostsToDate(string subReddit, DateTime toDate, Filter filter)
        {
            var posts = await _apiClient.GetPostsBetweenDates(subReddit, DateTime.UtcNow, toDate);
            var contents = await ContentMapper(posts, filter);

            return contents;
        }

        public async Task<IEnumerable<Content>> GetPostsBetweenDates(string subReddit, DateTime fromDate, DateTime untilDate, Filter filter)
        {
            var posts = await _apiClient.GetPostsBetweenDates(subReddit, fromDate, untilDate);
            var contents = await ContentMapper(posts, filter);

            return contents;
        }

        public async Task<IEnumerable<Content>> GetPostsUntilPostId(string subReddit, string postId, Filter filter)
        {
            var posts = await _apiClient.GetPostsFromPostIdUntilPostId(subReddit, null, postId);
            var contents = await ContentMapper(posts, filter);

            return contents;
        }

        private async Task<IEnumerable<Content>> ContentMapper(IEnumerable<Post> posts, Filter filter)
        {
            var contents = new ConcurrentBag<Content>();

            var options = new ParallelOptions
            {
                MaxDegreeOfParallelism = 5
            };

            await Parallel.ForEachAsync(posts, options, async (post, token) =>
            {
                var parsed = await _parserService.ParsePost(post, filter);
                contents.Add(parsed);
            });

            return contents;
        }
    }
}
