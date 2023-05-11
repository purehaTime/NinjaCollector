using AuthService.Interfaces;
using AuthService.Services;
using AutoFixture;
using FluentAssertions;
using GrpcHelper.DbService;
using GrpcHelper.Interfaces;
using Microsoft.AspNetCore.Identity;
using Moq;
using Serilog;

namespace UnitTests.AuthServiceTests
{
    public class AuthorizeServiceTests : BaseTest
    {
        private Mock<IJwtTokenService> _jwtTokenServiceMock = new();
        private Mock<IDatabaseServiceClient> _dbServiceClientMock = new();
        private Mock<IPasswordHasher<string>> _passwordHasherMock = new();
        private Mock<IInviteService> _InviteService = new();
        private Mock<ILogger> _loggerMock = new();


        private IAuthorizeService _authorizeService;


        [SetUp]
        public void Setup()
        {
            _authorizeService = new AuthorizeService(_dbServiceClientMock.Object, _loggerMock.Object,
                _InviteService.Object, _jwtTokenServiceMock.Object, _passwordHasherMock.Object);
        }

        [Test]
        public void AuthorizeUser_ShouldReturn_Token()
        {
            var userName = Fixture.Create<string>();
            var password = Fixture.Create<string>();
            var invite = Fixture.Create<string>();
            var token = Fixture.Create<string>();
            var hash = Fixture.Create<string>();

            var userModel = Fixture.Create<UserModel>();

            _dbServiceClientMock.Setup(s => s.GetUser(userName)).ReturnsAsync(() => userModel);
            _dbServiceClientMock.Setup(s => s.CreateUser(userName, hash)).ReturnsAsync(() => true);
            _passwordHasherMock.Setup(s => s.HashPassword(userName, password)).Returns(hash);
            _jwtTokenServiceMock.Setup(s => s.GetJwtToken(userName)).Returns(token);
            _InviteService.Setup(s => s.ValidateInvite(invite)).Returns(true);

            var result = _authorizeService.AuthorizeUser(userName, password, invite).GetAwaiter().GetResult();

            result.Should().Be(token);
        }

        [Test]
        public void AuthorizeUser_ShouldReturn_Null()
        {
            var userName = Fixture.Create<string>();
            var password = Fixture.Create<string>();
            var invite = Fixture.Create<string>();
            var hash = Fixture.Create<string>();

            var userModel = Fixture.Build<UserModel>()
                .With(w => w.UserName, userName)
                .Create();

            _dbServiceClientMock.Setup(s => s.GetUser(userName)).ReturnsAsync(() => userModel);
            _passwordHasherMock.Setup(s => s.HashPassword(userName, password)).Returns(hash);
            _InviteService.Setup(s => s.ValidateInvite(invite)).Returns(true);

            var result = _authorizeService.AuthorizeUser(userName, password, invite).GetAwaiter().GetResult();

            result.Should().BeNull();
        }
    }
}
