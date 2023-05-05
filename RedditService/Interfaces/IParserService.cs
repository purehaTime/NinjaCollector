using ModelsHelper.Models;
using Reddit.Controllers;
using RedditService.Model;
using Worker.Model;

namespace RedditService.Interfaces
{
    public interface IParserService
    {
        Task<Content> ParsePost(Post post, Filter filter);
    }
}
