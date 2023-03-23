using Reddit.Controllers;

namespace RedditService.Interfaces
{
    public interface IRedditAsyncClient
    {
        Task<Subreddit> GetSubreddit(string subreddit);
        Task<Post> GetLastPost(Subreddit subreddit);
        Task<List<Post>> GetNewPosts(Subreddit subreddit, string afterFullName);

        Task Hold();
    }
}
