namespace AuthService.Interfaces
{
    public interface IAuthenticateService
    {
        public Task<string> ValidateUser(string userName, string password);

        Task<bool> ValidateSession(string token);
    }
}
