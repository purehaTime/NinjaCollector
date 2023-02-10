using RedditService.Model;

namespace RedditService.Interfaces
{
    public interface IRedditApiClient
    {
        public Task<Content> GetLastPost(string subReddit);

        /// <summary>
        /// Get posts between dates
        /// </summary>
        /// <param name="subReddit"></param>
        /// <param name="fromDate"></param>
        /// <returns></returns>
        public Task<List<Content>> GetPostsUntilNow(string subReddit, DateTime fromDate);

    }
}
