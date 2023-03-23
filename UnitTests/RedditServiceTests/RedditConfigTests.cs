using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using RedditService.Interfaces;
using RedditService.Services;

namespace UnitTests.RedditService
{
    [TestFixture]
    public class RedditConfigTests : BaseTest
    {
        private Mock<IConfiguration> _configurationMock;
        private Mock<IConfigurationSection> _sectionMock;

        private IRedditConfig _redditConfig;

        [OneTimeSetUp]
        public void Setup()
        {
            _sectionMock = new Mock<IConfigurationSection>();
            _configurationMock = new Mock<IConfiguration>();

            _redditConfig = new RedditConfigService(_configurationMock.Object);
        }

        [Test]
        public void GetRedditConfig_ShouldReturn_Configs()
        {
            var configValue = Fixture.Create<string>();

            _sectionMock.SetupGet(s => s.Value).Returns(configValue);
            _configurationMock.Setup(s => s.GetSection(It.IsAny<string>())).Returns(_sectionMock.Object);

            var config = _redditConfig.GetRedditConfig();

            config.Should().NotBeNull();
            config.AntiSpamTimeout.Should().Be(0);
            config.AppSecret.Should().Be(configValue);
            config.ClientId.Should().Be(configValue);
            config.Password.Should().Be(configValue);
            config.UserName.Should().Be(configValue);
        }
    }
}
