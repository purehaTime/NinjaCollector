using GrpcHelper.DbService;
using GrpcHelper.Interfaces;
using static GrpcHelper.DbService.Database;

namespace GrpcHelper.Clients
{
    public class DatabaseServiceClient : IDatabaseServiceClient
    {
        private readonly DatabaseClient _client;

        public DatabaseServiceClient(DatabaseClient client)
        {
            _client = client;
        }

        public async Task<bool> WriteLogToDb(DbLogModel message)
        {
            var result = await _client.WriteLogAsync(message);
            return result.Success;
        }

        public async Task<bool> AddPost(Post post)
        {
            var result = await _client.AddPostAsync(post);
            return result.Success;
        }

        public async Task<bool> AddPosts(PostModel posts)
        {
            var result = await _client.AddPostsAsync(posts);
            return result.Success;
        }

        public async Task<List<Post>> GetPosts(PostRequest request)
        {
            var result = await _client.GetPostsAsync(request);
            return result.Posts.ToList();
        }

        public async Task<List<Image>> GetImages(ImageRequest request)
        {
            var result = await _client.GetImagesAsync(request);
            return result.Images.ToList();
        }

        public async Task<bool> AddImages(ImageModel images)
        {
            var result = await _client.AddImagesAsync(images);
            return result.Success;
        }

        public async Task<List<ParserSettingsModel>> GetParserSettings(ParserSettingsRequest request)
        {
            var result = await _client.GetParserSettingsAsync(request);
            return result.ParserSetting.ToList();
        }

        public async Task<bool> SaveParserSettings(ParserSettingsModel settings)
        {
            var result = await _client.SaveParserSettingsAsync(settings);
            return result.Success;
        }

        public async Task<List<PosterSettingsModel>> GetPosterSettings(PosterSettingsRequest request)
        {
            var result = await _client.GetPosterSettingsAsync(request);
            return result.PosterSettings_.ToList();
        }

        public async Task<bool> SavePosterSettings(PosterSettingsModel settings)
        {
            var result = await _client.SavePosterSettingsAsync(settings);
            return result.Success;
        }
    }
}
