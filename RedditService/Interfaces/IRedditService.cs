using RedditService.Model;

namespace RedditService.Interfaces
{
    public interface IRedditService
    {
        public Task<Content> GetLastPost(string subReddit);
    }
}
