using System.Net.Http.Headers;
using RedditService.Interfaces;
using RedditService.Model;
using ILogger = Serilog.ILogger;
using System.Text;
using Newtonsoft.Json;

namespace RedditService.Services
{
    public class RedditSessionService : IRedditSession
    {
        private DateTime _sessionDateStart;
        private string _accessToken;
        private IRedditConfig _config;
        private ILogger _logger;
        private HttpClient _httpClient;

        private readonly string _url = "https://www.reddit.com/api/v1/access_token";

        public RedditSessionService(HttpClient httpClient, IRedditConfig config, ILogger logger)
        {
            _config = config;
            _logger = logger;
            _httpClient = httpClient;
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

                var basicAuthHeader = Convert.ToBase64String(Encoding.Default.GetBytes(config.ClientId + ":" + config.AppSecret));
                var formContent = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", config.UserName),
                    new KeyValuePair<string, string>("password", config.Password)
                });

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", basicAuthHeader);

                _logger.Information("start retrieval new access token");
                var tokenResponse = await _httpClient.PostAsync(_url, formContent);
                var tokenJson = await tokenResponse.Content.ReadAsStringAsync();

                var token = JsonConvert.DeserializeObject<OAuthToken>(tokenJson);

                _sessionDateStart = DateTime.UtcNow;
                _accessToken = token.access_token;
                _logger.Information($"access token retrieval expires: {token.expires_in}");
            }

            return _accessToken;
        }
    }
}
