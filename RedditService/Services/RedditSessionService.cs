using RedditService.Interfaces;
using RestSharp.Authenticators;
using RestSharp;
using RedditService.Model;
using ILogger = Serilog.ILogger;

namespace RedditService.Services
{
    public class RedditSessionService : IRedditSession
    {
        private DateTime _sessionDateStart;
        private string _accessToken;
        private IRedditConfig _config;
        private IRestClient _restClient;
        private IRestRequest _restRequest;
        private ILogger _logger;

        private readonly string _url = "https://www.reddit.com/api/v1/access_token";

        public RedditSessionService(IRedditConfig config, IRestClient restClient, IRestRequest request, ILogger logger)
        {
            _config = config;
            _restClient = restClient;
            _restRequest = request;
            _restClient.BaseUrl = new Uri(_url);
            _logger = logger;
        }
        /// <summary>
        /// For script mode - token automatically expires after one day
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAccessToken()
        {
            if (DateTime.UtcNow - _sessionDateStart > TimeSpan.FromDays(0.99))
            {
                var config = _config.GetRedditConfig();
                _restClient.Authenticator = new HttpBasicAuthenticator(config.ClientId, config.AppSecret);

                _restRequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");

                _restRequest.AddParameter("grant_type", "password");
                _restRequest.AddParameter("username", config.UserName);
                _restRequest.AddParameter("password", config.Password);

                _logger.Information("start retrieval new access token");
                var response = await _restClient.PostAsync<OAuthToken>(_restRequest);
                _sessionDateStart = DateTime.UtcNow;
                _accessToken = response.access_token;
                _logger.Information($"access token retrieval expires: {response.expires_in}");
            }

            return _accessToken;
        }
    }
}
