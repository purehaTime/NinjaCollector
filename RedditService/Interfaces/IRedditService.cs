using ModelsHelper.Models;

namespace RedditService.Interfaces
{
    public interface IRedditService
    {
        public Task<Post> GetLastPost(string subReddit, Filter filter);

        Task<IEnumerable<Post>> GetPostsToDate(string subReddit, DateTime toDate, Filter filter);

        Task<IEnumerable<Post>> GetPostsBetweenDates(string subReddit, DateTime fromDate, DateTime untilDate, Filter filter);

        Task<IEnumerable<Post>> GetPostsUntilPostId(string subReddit, string postId, Filter filter);
    }
}
