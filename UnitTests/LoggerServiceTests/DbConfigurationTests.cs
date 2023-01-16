using AutoFixture;
using FluentAssertions;
using LoggerService.Interfaces;
using LoggerService.Services;
using Microsoft.Extensions.Configuration;
using Moq;

namespace UnitTests.LoggerServiceTests
{
    public class DbConfigurationTests : BaseTest
    {
        private Mock<IConfiguration> _configurationMock = null!;
        private Mock<IConfigurationSection> _configSectionMock = null!;

        private IDbConfiguration _dbConfig = null!;


        [SetUp]
        public void Setup()
        {
            _configSectionMock = new Mock<IConfigurationSection>();
            _configurationMock = new Mock<IConfiguration>();
        }

        [Test]
        public void GetConnectionString_ShouldReturn_ValidString()
        {
            var configValue = Fixture.Create<string>();

            _configSectionMock.Setup(s => s.Value).Returns(configValue);
            _configurationMock.Setup(s => s.GetSection(It.IsAny<string>())).Returns(_configSectionMock.Object);

            _dbConfig = new DbConfiguration(_configurationMock.Object);

            var result = _dbConfig.GetConnectionString();

            result.Should().NotBeNullOrEmpty();
            result.Should().NotContain("log.data");
            result.Should().NotContain("ninjapass");

            result.Should().Be($"Filename={configValue};Connection=shared;Password={configValue}");
        }

        [Test]
        public void GetConnectionString_ShouldReturn_DefaultValue()
        {
            _configurationMock.Setup(s => s.GetSection(It.IsAny<string>())).Returns(() => null!);

            _dbConfig = new DbConfiguration(_configurationMock.Object);

            var result = _dbConfig.GetConnectionString();

            result.Should().NotBeNullOrEmpty();
            result.Should().Contain("log.data");
            result.Should().Contain("ninjapass");

            result.Should().Be($"Filename=log.data;Connection=shared;Password=ninjapass");
        }
    }
}
