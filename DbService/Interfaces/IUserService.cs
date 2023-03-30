using DbService.Models;

namespace DbService.Interfaces
{
    public interface IUserService
    {
        public Task<User> GetUser(string userName);
        public Task<bool> CreateUser(string userName, string password);
    }
}
