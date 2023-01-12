using GrpcHelper.DbService;

namespace GrpcHelper.Interfaces
{
    public interface IDatabaseServiceClient
    {
        public Task<bool> WriteLogToDb(DbLogModel? message);
        public Task<bool> AddPost(PostModel post);
        public Task<List<PostModel>> GetPosts(PostRequest request);
        public Task<List<Image>> GetImages(ImageRequest request);
        public Task<bool> AddImages(List<Image> images);
        public Task<List<ParserSetting>> GetParserSettings(SettingsRequest request);
        public Task<bool> SaveParserSettings(List<ParserSetting> settings);
    }
}
