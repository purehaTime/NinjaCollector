using RedditService.Interfaces;
using RedditService.Model;

namespace RedditService.Services
{
    public class RedditConfigService : IRedditConfig
    {
        private readonly IConfiguration _appConfig;

        public RedditConfigService(IConfiguration appConfig)
        {
            _appConfig = appConfig;
        }

        public RedditConfig GetRedditConfig()
        {
            var config = new RedditConfig
            {
                AppSecret = Environment.GetEnvironmentVariable("Reddit_AppSecret") ??
                            _appConfig.GetSection("Reddit:AppSecret").Value,
                ClientId = Environment.GetEnvironmentVariable("Reddit_ClientId") ??
                           _appConfig.GetSection("Reddit:ClientId").Value,
                Password = Environment.GetEnvironmentVariable("Reddit_Password") ??
                           _appConfig.GetSection("Reddit:Password").Value,
                UserName = Environment.GetEnvironmentVariable("Reddit_UserName") ??
                           _appConfig.GetSection("Reddit:UserName").Value
            };

            return config;
        }
    }
}
