using Grpc.Core;
using Grpc.Net.Client;
using GrpcHelper.DbService;
using GrpcHelper.Interfaces;
using Serilog;
using static GrpcHelper.DbService.Database;

namespace GrpcHelper.Clients
{
    public class DatabaseServiceClient : IDatabaseServiceClient
    {
        private readonly DatabaseClient _client;
        private readonly ILogger _logger;

        public DatabaseServiceClient(DatabaseClient client, ILogger logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<bool> WriteLogToDb(DbLogModel message)
        {
            try
            {
                var result = await _client.WriteLogAsync(message);
                return result.Success;
            }
            catch (Exception err)
            {
                _logger.Error(err.Message);
            }
            return false;
        }

        public async Task<bool> AddPost(Post post)
        {
            try 
            {
                var result = await _client.AddPostAsync(post);
                return result.Success;
            }
            catch (Exception err)
            {
                _logger.Error(err.Message);
            }
            return false;
        }

        public async Task<bool> AddPosts(PostModel posts)
        {
            try
            {
                var result = await _client.AddPostsAsync(posts);
                return result.Success;
            }
            catch (Exception err)
            {
                _logger.Error(err.Message);
            }
            return false;
        }

        public async Task<Post> GetPost(EntityRequest postRequest)
        {
            try
            {
                var result = await _client.GetPostAsync(postRequest);
                return result;
            }
            catch (Exception err)
            {
                _logger.Error(err.Message);
            }
            return null;
        }

        public async Task<List<Image>> GetImages(ImagesRequest request)
        {
            try
            {
                var result = await _client.GetImagesAsync(request);
                return result.Images.ToList();
            }
            catch (Exception err)
            {
                _logger.Error(err.Message);
            }
            return null;
        }

        public async Task<Image> GetImage(EntityRequest request)
        {
            try
            {
                var result = await _client.GetImageAsync(request);
                return result;
            }
            catch (Exception err)
            {
                _logger.Error(err.Message);
            }
            return null;
        }

        public async Task<bool> AddImages(ImageModel images)
        {
            try
            {
                var result = await _client.AddImagesAsync(images);
                return result.Success;
            }
            catch (Exception err)
            {
                _logger.Error(err.Message);
            }
            return false;
        }

        public async Task<List<ParserSettingsModel>> GetParserSettings(ParserSettingsRequest request)
        {
            try
            {
                var result = await _client.GetParserSettingsAsync(request);
                return result.ParserSetting.ToList();
            }
            catch (Exception err)
            {
                _logger.Error(err.Message);
            }
            return null;
        }

        public async Task<bool> SaveParserSettings(ParserSettingsModel settings)
        {
            try
            {
                var result = await _client.SaveParserSettingsAsync(settings);
                return result.Success;
            }
            catch (Exception err)
            {
                _logger.Error(err.Message);
            }
            return false;
        }

        public async Task<bool> DeleteParserSettings(string id)
        {
            try
            {
                var result = await _client.RemoveParserSettingsAsync(new ModelId
                {
                    Id = id ?? string.Empty
                });
                return result.Success;
            }
            catch (Exception err)
            {
                _logger.Error(err.Message);
            }
            return false;
        }

        public async Task<List<PosterSettingsModel>> GetPosterSettings(PosterSettingsRequest request)
        {
            try
            {
                var result = await _client.GetPosterSettingsAsync(request);
                return result.PosterSettings_.ToList();
            }
            catch (Exception err)
            {
                _logger.Error(err.Message);
            }
            return null;
        }

        public async Task<bool> SavePosterSettings(PosterSettingsModel settings)
        {
            try
            {
                var result = await _client.SavePosterSettingsAsync(settings);
                return result.Success;
            }
            catch (Exception err)
            {
                _logger.Error(err.Message);
            }
            return false;
        }

        public async Task<bool> DeletePosterSettings(string id)
        {
            try
            {
                var result = await _client.RemovePosterSettingsAsync(new ModelId
                {
                    Id = id ?? string.Empty
                });
                return result.Success;
            }
            catch (Exception err)
            {
                _logger.Error(err.Message);
            }
            return false;
        }

        public async Task<bool> CreateUser(string userName, string password)
        {
            try
            {
                var result = await _client.AddUserAsync(new AddUserModel { UserName = userName, Password = password });
                return result.Success;
            }
            catch (Exception err)
            {
                _logger.Error(err.Message);
            }
            return false;
        }

        public async Task<UserModel> GetUser(string userName)
        {
            try
            {
                var user = await _client.GetUserAsync(new UserRequest { UserName = userName });
                return user;
            }
            catch (Exception err)
            {
                _logger.Error(err.Message);
            }
            return null;
        }
    }
}
