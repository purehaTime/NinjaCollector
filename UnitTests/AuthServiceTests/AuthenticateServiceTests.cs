using AuthService.Interfaces;
using AuthService.Services;
using AutoFixture;
using FluentAssertions;
using GrpcHelper.DbService;
using GrpcHelper.Interfaces;
using Microsoft.AspNetCore.Identity;
using Moq;
using Newtonsoft.Json.Linq;
using Serilog;

namespace UnitTests.AuthServiceTests
{
    [TestFixture]
    public class AuthenticateServiceTests : BaseTest
    {
        private Mock<IJwtTokenService> _jwtTokenServiceMock = new();
        private Mock<IDatabaseServiceClient> _dbServiceClientMock = new();
        private Mock<IPasswordHasher<string>> _passwordHasherMock = new();
        private Mock<ILogger> _loggerMock = new();


        private IAuthenticateService _authService;

        [OneTimeSetUp]
        public void Setup()
        {
            _authService = new AuthenticateService(_jwtTokenServiceMock.Object, _dbServiceClientMock.Object, _passwordHasherMock.Object, _loggerMock.Object);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ValidateSession_ShouldReturn_bool(bool validateResult)
        {
            var token = Fixture.Create<string>();
            _jwtTokenServiceMock.Setup(s => s.Verify(token)).ReturnsAsync(() => validateResult);

            var result = _authService.ValidateSession(token).GetAwaiter().GetResult();

            result.Should().Be(validateResult);
        }

        [Test]
        public void ValidateUser_ShouldReturn_Token()
        {
            var userName = Fixture.Create<string>();
            var password = Fixture.Create<string>();
            var userModel = Fixture.Build<UserModel>()
                .With(w => w.UserName, userName)
                .Create();

            var token = Fixture.Create<string>();

            _dbServiceClientMock.Setup(s => s.GetUser(userName)).ReturnsAsync(() => userModel);
            _passwordHasherMock.Setup(s => s.VerifyHashedPassword(userName, userModel.Password, password))
                .Returns(PasswordVerificationResult.Success);
            _jwtTokenServiceMock.Setup(s => s.GetJwtToken(userName)).Returns(token);

            var result = _authService.ValidateUser(userName, password).GetAwaiter().GetResult();

            result.Should().NotBeNullOrEmpty();
            result.Should().Be(token);
        }

        [Test]
        public void ValidateUser_ShouldReturn_Empty()
        {
            var userName = Fixture.Create<string>();
            var password = Fixture.Create<string>();
            var userModel = Fixture.Build<UserModel>()
                .With(w => w.UserName, userName)
                .Create();

            var token = Fixture.Create<string>();

            _dbServiceClientMock.Setup(s => s.GetUser(userName)).ReturnsAsync(() => userModel);
            _passwordHasherMock.Setup(s => s.VerifyHashedPassword(userName, userModel.Password, password))
                .Returns(PasswordVerificationResult.Failed);
            _jwtTokenServiceMock.Setup(s => s.GetJwtToken(userName)).Returns(token);

            var result = _authService.ValidateUser(userName, password).GetAwaiter().GetResult();

            result.Should().BeEmpty();
            result.Should().NotBe(token);
        }
    }
}
