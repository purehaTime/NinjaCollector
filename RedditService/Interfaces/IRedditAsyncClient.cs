using Reddit.Controllers;

namespace RedditService.Interfaces
{
    public interface IRedditAsyncClient
    {
        Task<Subreddit> GetSubreddit(string subreddit);
        Task<Post> GetLastPost(Subreddit subreddit);

        Task<IEnumerable<Post>> GetPostsByFilter(Subreddit subreddit, Func<Post, bool> skipFilter,
            Func<Post, bool> takeFilter);
    }
}
