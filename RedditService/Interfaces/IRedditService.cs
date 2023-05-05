using ModelsHelper.Models;
using RedditService.Model;
using Worker.Model;

namespace RedditService.Interfaces
{
    public interface IRedditService
    {
        public Task<Content> GetLastPost(string subReddit, Filter filter);

        Task<IEnumerable<Content>> GetPostsToDate(string subReddit, DateTime toDate, Filter filter);

        Task<IEnumerable<Content>> GetPostsBetweenDates(string subReddit, DateTime fromDate, DateTime untilDate, Filter filter);

        Task<IEnumerable<Content>> GetPostsUntilPostId(string subReddit, string postId, Filter filter);
    }
}
