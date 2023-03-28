namespace AuthService.Interfaces
{
    public interface IJwtTokenService
    {
        public string GetJwtToken(string userName, string password);
    }
}
