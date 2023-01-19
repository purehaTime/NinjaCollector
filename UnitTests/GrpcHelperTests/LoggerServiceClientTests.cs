using AutoFixture;
using FluentAssertions;
using GrpcHelper.Clients;
using GrpcHelper.Interfaces;
using GrpcHelper.LogService;
using Moq;

namespace UnitTests.GrpcHelperTests
{
    [TestFixture]
    public class LoggerServiceClientTests : BaseTest
    {
        private Mock<GrpcHelper.LogService.Logger.LoggerClient> _loggerClientMock;

        private ILoggerServiceClient _loggerServiceClient;

        [SetUp]
        public void Setup()
        {
            _loggerClientMock = new Mock<GrpcHelper.LogService.Logger.LoggerClient>();
        }


        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void WriteLog_ShouldCall_WriteLogAsync_WithSuccess(bool responseTestCase)
        {
            var logModel = Fixture.Create<LogModel>();
            var response = new WriteResponse { Success = responseTestCase };

            var returnResponse = GetAsyncUnaryCallSuccess(response);

            _loggerClientMock.Setup(s => s.WriteLogAsync(It.IsAny<LogModel>(), null, null, CancellationToken.None))
                .Returns(returnResponse);

            _loggerServiceClient = new LoggerServiceClient(_loggerClientMock.Object);
            
            var result = _loggerServiceClient.WriteLog(logModel.Message, logModel.Id, logModel.Application)
                .GetAwaiter()
                .GetResult();

            result.Should().Be(responseTestCase);
        }
    }
}
