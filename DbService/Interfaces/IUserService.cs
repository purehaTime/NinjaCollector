using DbService.Models;

namespace DbService.Interfaces
{
    public interface IUserService
    {
        public Task<User> GetUser(string userName);
        public Task<bool> CreateUser(User user);
    }
}
