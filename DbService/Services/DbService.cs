using DbService.Interfaces;
using DbService.Mapping;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcHelper.DbService;
using MongoDB.Bson;
using ILogger = Serilog.ILogger;
using ParserSettings = GrpcHelper.DbService.ParserSettings;
using Post = GrpcHelper.DbService.Post;
using PosterSettings = GrpcHelper.DbService.PosterSettings;
using Status = GrpcHelper.DbService.Status;

namespace DbService.Services
{
    public class DbService : Database.DatabaseBase
    {
        private readonly ISettingsService _settings;
        private readonly IPostService _post;
        private readonly IImageService _image;
        private readonly IHistoryService _history;
        private readonly IUserService _userService;
        private readonly ILogger _logger;

        public DbService(ISettingsService settings, IPostService post, IImageService image, IHistoryService history, IUserService userService, ILogger logger)
        {
            _settings = settings;
            _post = post;
            _image = image;
            _history = history;
            _userService = userService;
            _logger = logger;
        }

        public override async Task<Status> AddUser(AddUserModel request, ServerCallContext context)
        {
            var result = await _userService.CreateUser(request.UserName, request.Password);
            return new Status { Success = result };
        }

        public override async Task<UserModel> GetUser(UserRequest request, ServerCallContext context)
        {
            var user = await _userService.GetUser(request.UserName);
            if (user != null)
            {
                return new UserModel
                {
                    Id = user.Id.ToString(),
                    UserName = user.UserName,
                    Created = user.Created.ToTimestamp(),
                    Password = user.HashPassword
                };
            }
            return new UserModel();
        }

        public override async Task<Status> AddPost(Post post, ServerCallContext context)
        {
            var result = await _post.SavePost(post);
            return new Status { Success = result };
        }

        public override async Task<Status> AddPosts(IAsyncStreamReader<Post> requestStream, ServerCallContext context)
        {
            var result = true;
            _logger.Information("Start receive posts");

            await foreach (var message in requestStream.ReadAllAsync())
            {
                result &= await _post.SavePost(message);
            }

            _logger.Information("End receive posts");
            return new Status { Success = result };
        }

        public override async Task<Post> GetPost(EntityRequest request, ServerCallContext context)
        {
            var result = await _post.GetPostBySettingId(request.SettingsId);
            return result;
        }

        public override async Task<Status> SaveParserSettings(ParserSettingsModel request, ServerCallContext context)
        {
            var model = request.ToDatabase();
            var result = await _settings.SaveParserSettings(model);
            
            return new Status { Success = result };
        }

        public override async Task<ParserSettings> GetParserSettings(ParserSettingsRequest request, ServerCallContext context)
        {
            ObjectId? settingsId = string.IsNullOrEmpty(request.SettingsId)
                ? null
                : ObjectId.Parse(request.SettingsId);

            var response = await _settings.GetParserSettings(request.Source, settingsId);

            return new ParserSettings
            {
                 ParserSetting = { response.Select(s => s.ToGrpcData()) }
            };
        }

        public override async Task<Status> SavePosterSettings(PosterSettingsModel request, ServerCallContext context)
        {
            var model = request.ToDatabase();
            var result = await _settings.SavePosterSettings(model);

            return new Status
            {
                Success = result
            };
        }

        public override async Task<PosterSettings> GetPosterSettings(PosterSettingsRequest request, ServerCallContext context)
        {
            ObjectId? settingsId = string.IsNullOrEmpty(request.SettingsId)
                ? null
                : ObjectId.Parse(request.SettingsId);

            var response = await _settings.GetPosterSettings(request.Service, settingsId);

            return new PosterSettings
            {
                PosterSettings_ = { response.Select(s => s.ToGrpcData()) }
            };
        }

        public override async Task<Image> GetImage(EntityRequest request, ServerCallContext context)
        {
            var image = await _image.GetImageBySettingId(request.SettingsId);
            var result = image.image.ToGrpcData(image.steam.ToArray());
            return result;
        }

        public override async Task GetImages(ImagesRequest request, IServerStreamWriter<Image> responseStream, ServerCallContext context)
        {
            var images = await _image.GetImagesByTags(request.Tags.ToList());
            foreach (var image in images)
            {
                var result = image.image.ToGrpcData(image.stream.ToArray());
                await responseStream.WriteAsync(result);
            }
        }

        public override async Task<Status> AddImages(IAsyncStreamReader<Image> requestStream, ServerCallContext context)
        {
            var result = true;
            _logger.Information("Start receive posts");

            await foreach (var message in requestStream.ReadAllAsync())
            {
                var saved = await _image.SaveImage(message);
                result &= saved.Status;
            }

            _logger.Information("End receive posts");
            return new Status { Success = result };
        }

        public override async Task<Status> SaveHistory(HistoryModel request, ServerCallContext context)
        {
           var result = await _history.SaveHistory(request.ToDatabase());
           return new Status { Success = result };
        }
    }
}
