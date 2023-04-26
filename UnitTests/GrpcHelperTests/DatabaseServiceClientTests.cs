using AutoFixture;
using FluentAssertions;
using GrpcHelper.Clients;
using GrpcHelper.DbService;
using GrpcHelper.Interfaces;
using Moq;
using Serilog;

namespace UnitTests.GrpcHelperTests
{
    [TestFixture]
    public class DatabaseServiceClientTests : BaseTest
    {
        private Mock<Database.DatabaseClient> _dbClientMock;
        private Mock<ILogger> _loggerMock;

        private IDatabaseServiceClient _dbServiceClient;

        [SetUp]
        public void Setup()
        {
            _dbClientMock = new Mock<Database.DatabaseClient>();
            _loggerMock = new Mock<ILogger>();
        }


        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void WriteLogToDb_ShouldReturn_Bool(bool responseTestCase)
        {
            var dbLogsModel = Fixture.Create<DbLogModel>();
            var response = new WriteLogResponse { Success = responseTestCase };

            var returnResponse = GetAsyncUnaryCallSuccess(response);

            _dbClientMock.Setup(s => s.WriteLogAsync(dbLogsModel, null, null, CancellationToken.None)).Returns(returnResponse);

            _dbServiceClient = new DatabaseServiceClient(_dbClientMock.Object, _loggerMock.Object);
            
            var result = _dbServiceClient.WriteLogToDb(dbLogsModel)
                .GetAwaiter()
                .GetResult();

            result.Should().Be(responseTestCase);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void SaveParserSettings_ShouldReturn_Bool(bool responseTestCase)
        {
            var parserSettings = Fixture.Create<ParserSettingsModel>();
            var response = new Status { Success = responseTestCase };

            var returnResponse = GetAsyncUnaryCallSuccess(response);

            _dbClientMock.Setup(s => s.SaveParserSettingsAsync(parserSettings, null, null, CancellationToken.None)).Returns(returnResponse);

            _dbServiceClient = new DatabaseServiceClient(_dbClientMock.Object, _loggerMock.Object);

            var result = _dbServiceClient.SaveParserSettings(parserSettings)
                .GetAwaiter()
                .GetResult();

            result.Should().Be(responseTestCase);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void SavePosterSettings_ShouldReturn_Bool(bool responseTestCase)
        {
            var posterSettings = Fixture.Create<PosterSettingsModel>();
            var response = new Status { Success = responseTestCase };

            var returnResponse = GetAsyncUnaryCallSuccess(response);

            _dbClientMock.Setup(s => s.SavePosterSettingsAsync(posterSettings, null, null, CancellationToken.None)).Returns(returnResponse);

            _dbServiceClient = new DatabaseServiceClient(_dbClientMock.Object, _loggerMock.Object);

            var result = _dbServiceClient.SavePosterSettings(posterSettings)
                .GetAwaiter()
                .GetResult();

            result.Should().Be(responseTestCase);
        }
    }
}
