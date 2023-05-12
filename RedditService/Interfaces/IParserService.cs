using ModelsHelper.Models;
using Content = ModelsHelper.Models.Post;
using Post = Reddit.Controllers.Post;


namespace RedditService.Interfaces
{
    public interface IParserService
    {
        Task<Content> ParsePost(Post post, Filter filter);
    }
}
