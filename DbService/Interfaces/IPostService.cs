using GrpcHelper.DbService;
using Post = DbService.Models.Post;
using PosterSettings = DbService.Models.PosterSettings;

namespace DbService.Interfaces
{
    public interface IPostService
    {
        public Task<Post> GetPostByTags(List<string> tags, PosterSettings poster);
        public Task<bool> SavePost(Post post);
        public Task<bool> SavePosts(PostModel posts);
    }
}
