using Reddit.Controllers;

namespace RedditService.Interfaces
{
    public interface IRedditApiClient
    {
        public Task<Post> GetLastPost(string subReddit);

        /// <summary>
        /// Get posts between dates
        /// </summary>
        /// <param name="subReddit"></param>
        /// <param name="fromDate"></param>
        /// <returns></returns>
        public Task<IEnumerable<Post>> GetPostsUntilNow(string subReddit, DateTime fromDate);

    }
}
