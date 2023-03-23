using RedditService.Interfaces;
using RedditService.Model;

namespace RedditService.Services
{
    public class RedditConfigService : IRedditConfig
    {
        private readonly IConfiguration _appConfig;

        private RedditConfig _cachedConfig;

        public RedditConfigService(IConfiguration appConfig)
        {
            _appConfig = appConfig;
        }

        public RedditConfig GetRedditConfig()
        {
            return _cachedConfig ??= new RedditConfig
            {
                AppSecret = Environment.GetEnvironmentVariable("Reddit_AppSecret") ??
                            _appConfig.GetSection("Reddit:AppSecret").Value,
                ClientId = Environment.GetEnvironmentVariable("Reddit_ClientId") ??
                           _appConfig.GetSection("Reddit:ClientId").Value,
                Password = Environment.GetEnvironmentVariable("Reddit_Password") ??
                           _appConfig.GetSection("Reddit:Password").Value,
                UserName = Environment.GetEnvironmentVariable("Reddit_UserName") ??
                           _appConfig.GetSection("Reddit:UserName").Value,
                AntiSpamTimeout = GetTimeout()
            };
        }

        private int GetTimeout()
        {
            return int.TryParse(Environment.GetEnvironmentVariable("Reddit_Timeout") ??
                         _appConfig.GetSection("Reddit:Timeout").Value, out var timeout) 
                ? timeout 
                : 0;
        }
    }
}
