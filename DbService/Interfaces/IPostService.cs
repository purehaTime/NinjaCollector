using DbService.Models;

namespace DbService.Interfaces
{
    public interface IPostService
    {
        public Task<Post> GetPostByTags(List<string> tags, PosterSettings poster);

        public Task<bool> SavePost(Post post);
    }
}
