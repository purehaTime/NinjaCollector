using GrpcHelper.DbService;

namespace GrpcHelper.Interfaces
{
    public interface IDatabaseServiceClient
    {
        public Task<bool> WriteLogToDb(DbLogModel message);
        public Task<bool> AddPost(Post post);
        public Task<bool> AddPosts(PostModel post);
        public Task<List<Post>> GetPosts(PostRequest request);
        public Task<List<Image>> GetImages(ImageRequest request);
        public Task<bool> AddImages(ImageModel images);
        public Task<List<ParserSettingsModel>> GetParserSettings(ParserSettingsRequest request);
        public Task<bool> SaveParserSettings(ParserSettingsModel settings);
        public Task<bool> DeleteParserSettings(string id);
        public Task<List<PosterSettingsModel>> GetPosterSettings(PosterSettingsRequest request);
        public Task<bool> SavePosterSettings(PosterSettingsModel settings);
        public Task<bool> CreateUser(string userName, string password);
        public Task<UserModel> GetUser(string userName);
    }
}
