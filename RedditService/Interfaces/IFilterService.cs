using Models.Models;
using Worker.Model;
using Post = Reddit.Controllers.Post;

namespace RedditService.Interfaces
{
    public interface IFilterService
    {
        public bool IsValid(Post post, Filter filter);
    }
}
