using GrpcHelper.DbService;
using Post = GrpcHelper.DbService.Post;
using PosterSettings = DbService.Models.PosterSettings;

namespace DbService.Interfaces
{
    public interface IPostService
    {
        public Task<Post> GetPostBySettingId(string settingId);
        public Task<bool> SavePost(Post post);
        public Task<bool> SavePosts(PostModel posts);
    }
}
