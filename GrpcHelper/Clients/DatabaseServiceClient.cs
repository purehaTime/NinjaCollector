using Grpc.Core;
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

        public async Task<bool> WriteLogToDb(DbLogModel? message)
        {
            var result = await _client.WriteLogAsync(message);

            return result.Success;
        }

        public async Task<bool> AddPost(PostModel post)
        {
            var stream = _client.AddPost();
            await stream.RequestStream.WriteAsync(post);
            var result = await stream.ResponseAsync;
            return result.Success;
        }

        public async Task<List<PostModel>> GetPosts(PostRequest request)
        {
            var stream = _client.GetPost(request);

            var result = new List<PostModel>();
            await foreach (var post in stream.ResponseStream.ReadAllAsync())
            {
                result.Add(post);
            }

            return result;
        }

        public async Task<List<Image>> GetImages(ImageRequest request)
        {
            var stream = _client.GetImages(request);

            var result = new List<Image>();
            await foreach (var post in stream.ResponseStream.ReadAllAsync())
            {
                result.Add(post);
            }

            return result;
        }

        public async Task<bool> AddImages(List<Image> images)
        {
            var stream = _client.AddImages();
            await stream.RequestStream.WriteAsync(new ImageModel
            {
                Images = { images }
            });
            var result = await stream.ResponseAsync;
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
