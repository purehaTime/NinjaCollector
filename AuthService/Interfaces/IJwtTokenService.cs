namespace AuthService.Interfaces
{
    public interface IJwtTokenService
    {
        public string GetJwtToken(string userName);

        public Task<bool> Verify(string token);
    }
}
