using Reddit.Controllers;

namespace RedditService.Interfaces
{
    public interface IRedditApiClient
    {
        public Task<Post> GetLastPost(string subReddit);
        Task<IEnumerable<Post>> GetPostsBetweenDates(string subReddit, DateTime fromDate, DateTime toDate);
        Task<IEnumerable<Post>> GetPostsFromPostIdUntilPostId(string subReddit, string fromPostId, string untilPostId);
        Task<IEnumerable<Post>> GetPostsFromPostIdUntilDate(string subReddit, string fromPostId, DateTime untilDate);
        Task<IEnumerable<Post>> GetPostsFromDateUntilPostId(string subReddit, DateTime fromDate, string untilPostId);

    }
}
