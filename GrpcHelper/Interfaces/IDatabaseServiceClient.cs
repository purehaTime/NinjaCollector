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
        public Task<List<ParserSettingsModel>> GetParserSettings(ParserSettingsRequest request);
        public Task<bool> SaveParserSettings(ParserSettingsModel settings);
        public Task<List<PosterSettingsModel>> GetPosterSettings(PosterSettingsRequest request);
        public Task<bool> SavePosterSettings(PosterSettingsModel settings);
    }
}
