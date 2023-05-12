using DbService.Interfaces;
using DbService.Mapping;
using DbService.Models;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcHelper.DbService;
using MongoDB.Bson;
using Status = GrpcHelper.DbService.Status;
using ParserSettings = GrpcHelper.DbService.ParserSettings;
using Post = GrpcHelper.DbService.Post;

namespace DbService.Services
{
    public class DbService : Database.DatabaseBase
    {
        private ISettingsService _settings;
        private IPostService _post;
        private IImageService _image;
        private IHistoryService _history;
        private IUserService _userService;

        public DbService(ISettingsService settings, IPostService post, IImageService image, IHistoryService history, IUserService userService)
        {
            _settings = settings;
            _post = post;
            _image = image;
            _history = history;
            _userService = userService;
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

        public override async Task<Status> AddPosts(PostModel posts, ServerCallContext context)
        {
            var result = await _post.SavePosts(posts);

            return new Status { Success = result };
        }

        public override async Task<Status> SaveParserSettings(ParserSettingsModel request, ServerCallContext context)
        {
            var model = request.ToDatabase();
            var result = await _settings.SaveParserSettings(model);
            
            return new Status
            {
                Success = result
            };
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
    }
}
