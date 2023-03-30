namespace AuthService.Interfaces
{
    public interface IAuthorizeService
    {
        public Task<string> AuthorizeUser(string userName, string password, string invite);
    }
}
