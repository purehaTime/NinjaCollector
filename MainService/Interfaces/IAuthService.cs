namespace MainService.Interfaces
{
    public interface IAuthService
    {
        public Task<bool> Register(string userLogin, string password, string invite);
        public Task<bool> Login(string userLogin, string password);
        public Task<bool> Logout();
        public Task<bool> Verify(string token);
    }
}
