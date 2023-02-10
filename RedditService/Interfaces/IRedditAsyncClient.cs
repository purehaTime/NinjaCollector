using Reddit.Controllers;

namespace RedditService.Interfaces
{
    public interface IRedditAsyncClient
    {
        Task<Subreddit> GetSubreddit(string subreddit);
        Task<IEnumerable<Post>> GetPostsBetweenDates(Subreddit subreddit, DateTime fromDate, DateTime toDate);
        Task<Post> GetLastPost(Subreddit subreddit);
    }
}
