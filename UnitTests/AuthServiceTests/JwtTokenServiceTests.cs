using AuthService.Interfaces;
using AuthService.Services;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;

namespace UnitTests.AuthServiceTests
{
    public class JwtTokenServiceTests : BaseTest
    {
        private Mock<IConfiguration> _configMock = new();

        private IJwtTokenService _jwtTokenService;


        [SetUp]
        public void Setup()
        {
            _configMock.SetupGet(s => s["Security:Key"]).Returns(Guid.NewGuid().ToString());
            _jwtTokenService = new JwtTokenService(_configMock.Object);
        }

        [Test]
        public void GetJwtToken_ShouldReturn_Token()
        {
            var userName = Fixture.Create<string>();

            var token = _jwtTokenService.GetJwtToken(userName);

            token.Should().NotBeNullOrEmpty();
        }
    }
}
