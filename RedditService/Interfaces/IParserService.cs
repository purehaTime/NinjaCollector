using Reddit.Controllers;
using RedditService.Model;

namespace RedditService.Interfaces
{
    public interface IParserService
    {
        Task<Content> ParsePost(Post post);
    }
}
