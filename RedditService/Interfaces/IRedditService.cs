using RedditService.Model;

namespace RedditService.Interfaces
{
    public interface IRedditService
    {
        public Task<Content> GetLastPost(string subReddit);

        Task<IEnumerable<Content>> GetPostsUntilNow(string subReddit, DateTime fromDate);
    }
}
