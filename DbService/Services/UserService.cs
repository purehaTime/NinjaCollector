using DbService.Interfaces;
using DbService.Models;
using DbService.Repositories;
using MongoDB.Driver;
using ILogger = Serilog.ILogger;

namespace DbService.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly ILogger _logger;

        public UserService(IRepository<User> userRepository, ILogger logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<User> GetUser(string userName)
        {
            var filter = Builders<User>.Filter.Eq(x => x.UserName, userName);
            var user = await _userRepository.Find(filter, null!, CancellationToken.None);

            return user;
        }

        public async Task<bool> CreateUser(User user)
        {
            var result = await _userRepository.Insert(user, null!, CancellationToken.None);
            if (!result)
            {
                _logger.Error($"Cant add user {user.UserName}");
            }

            return result;
        }
    }
}
