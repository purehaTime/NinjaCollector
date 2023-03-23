using AutoFixture;
using FluentAssertions;
using Moq;
using Moq.Contrib.HttpClient;
using Newtonsoft.Json;
using RedditService.Interfaces;
using RedditService.Model;
using RedditService.Services;
using Serilog;
using System.Net;

namespace UnitTests.RedditServiceTests
{
    [TestFixture]
    public class RedditSessionServiceTests : BaseTest
    {
        private Mock<HttpMessageHandler> _httpHandlerMock;
        private Mock<IRedditConfig> _redditConfigMock;
        private Mock<ILogger> _loggerMock;

        private IRedditSession _redditSession;

        [OneTimeSetUp]
        public void Setup()
        {
            _httpHandlerMock = new Mock<HttpMessageHandler>();
            _redditConfigMock = new Mock<IRedditConfig>();
            _loggerMock = new Mock<ILogger>();

            _redditSession = new RedditSessionService(_httpHandlerMock.CreateClient(), _redditConfigMock.Object, _loggerMock.Object);
        }

        [Test]
        public void GetAccessToken_ShouldReturn_TokenObject()
        {
            var redditConfig = Fixture.Create<RedditConfig>();

            var token = Fixture.Create<OAuthToken>();
            var data = JsonConvert.SerializeObject(token);
            _httpHandlerMock.SetupAnyRequest()
                .ReturnsResponse(HttpStatusCode.OK, data);

            _redditConfigMock.Setup(s => s.GetRedditConfig()).Returns(redditConfig);
            var result = _redditSession.GetAccessToken()
                .GetAwaiter()
                .GetResult();

            result.Should().NotBeNull();
            result.Should().Be(token.access_token);
        }
    }
}
