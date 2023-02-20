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

        public async Task<IEnumerable<Content>> GetPostsUntilNow(string subReddit, DateTime fromDate)
        {
            var posts = await _apiClient.GetPostsBetweenDates(subReddit, fromDate, DateTime.UtcNow);

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
