using GrpcHelper.DbService;

namespace GrpcHelper.Interfaces
{
    public interface IDatabaseServiceClient
    {
        public Task<bool> WriteLogToDb(DbLogModel message);
        public Task<bool> AddPost(Post post);
        public Task<bool> AddPosts(List<Post> posts);
        public Task<Post> GetPost(EntityRequest postRequest);
        public Task<List<Image>> GetImages(ImagesRequest request);
        public Task<Image> GetImage(EntityRequest request);
        public Task<bool> AddImages(List<Image> images);
        public Task<List<ParserSettingsModel>> GetParserSettings(ParserSettingsRequest request);
        public Task<bool> SaveParserSettings(ParserSettingsModel settings);
        public Task<bool> DeleteParserSettings(string id);
        public Task<List<PosterSettingsModel>> GetPosterSettings(PosterSettingsRequest request);
        public Task<bool> SavePosterSettings(PosterSettingsModel settings);
        public Task<bool> DeletePosterSettings(string id);
        public Task<bool> CreateUser(string userName, string password);
        public Task<UserModel> GetUser(string userName);
    }
}
