using RedditService.Model;

namespace RedditService.Interfaces
{
    public interface IRedditService
    {
        public Task<Content> GetLastPost(string subReddit);

        Task<IEnumerable<Content>> GetPostsToDate(string subReddit, DateTime toDate);

        Task<IEnumerable<Content>> GetPostsBetweenDates(string subReddit, DateTime fromDate, DateTime untilDate);

        Task<IEnumerable<Content>> GetPostsUntilPostId(string subReddit, string postId);
    }
}
