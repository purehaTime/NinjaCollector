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

            _dbServiceClient = new DatabaseServiceClient(_dbClientMock.Object, _loggerMock.Object);
        }


        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void WriteLogToDb_ShouldReturn_Bool(bool responseTestCase)
        {
            var dbLogsModel = Fixture.Create<DbLogModel>();
            var response = new Status { Success = responseTestCase };

            var returnResponse = GetAsyncUnaryCallSuccess(response);

            _dbClientMock.Setup(s => s.WriteLogAsync(dbLogsModel, null, null, CancellationToken.None)).Returns(returnResponse);

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

            var result = _dbServiceClient.SavePosterSettings(posterSettings)
                .GetAwaiter()
                .GetResult();

            result.Should().Be(responseTestCase);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void AddPost_ShouldReturn_Bool(bool responseTestCase)
        {
            var post = Fixture.Create<Post>();
            var response = new Status { Success = responseTestCase };

            var returnResponse = GetAsyncUnaryCallSuccess(response);
            _dbClientMock.Setup(s => s.AddPostAsync(post, null, null, CancellationToken.None)).Returns(returnResponse);

            var result = _dbServiceClient.AddPost(post)
                .GetAwaiter()
                .GetResult();

            result.Should().Be(responseTestCase);
        }

        [Test]
        public void AddPost_ShouldReturn_FalseException()
        {
            var post = Fixture.Create<Post>();

            _dbClientMock.Setup(s => s.AddPostAsync(post, null, null, CancellationToken.None)).Throws<Exception>();

            var result = _dbServiceClient.AddPost(post)
                .GetAwaiter()
                .GetResult();

            _loggerMock.Verify(v => v.Error(It.IsAny<string>()), Times.Once);
            result.Should().Be(false);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void AddPosts_ShouldReturn_Bool(bool responseTestCase)
        {
            var posts = Fixture.CreateMany<Post>(5).ToList();
            var response = new Status { Success = responseTestCase };

            foreach (var post in posts)
            {
                var returnResponse = GetAsyncClientStreamingCallSuccess(post, response);
                _dbClientMock.SetupSequence(s => s.AddPosts(null, null, CancellationToken.None)).Returns(returnResponse);
            }

            var result = _dbServiceClient.AddPosts(posts)
                .GetAwaiter()
                .GetResult();

            result.Should().Be(responseTestCase);
        }
        
        [Test]
        public void AddPosts_ShouldReturn_FalseException()
        {
            var posts = Fixture.CreateMany<Post>(5).ToList();

            _dbClientMock.Setup(s => s.AddPosts(null, null, CancellationToken.None)).Throws<Exception>();

            var result = _dbServiceClient.AddPosts(posts)
                .GetAwaiter()
                .GetResult();

            _loggerMock.Verify(v => v.Error(It.IsAny<string>()), Times.Once);
            result.Should().Be(false);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void AddImages_ShouldReturn_Bool(bool responseTestCase)
        {
            var images = Fixture.CreateMany<Image>(5).ToList();
            var response = new Status { Success = responseTestCase };

            foreach (var image in images)
            {
                var returnResponse = GetAsyncClientStreamingCallSuccess(image, response);
                _dbClientMock.SetupSequence(s => s.AddImages(null, null, CancellationToken.None)).Returns(returnResponse);
            }

            var result = _dbServiceClient.AddImages(images)
                .GetAwaiter()
                .GetResult();

            result.Should().Be(responseTestCase);
        }

        [Test]
        public void AddImages_ShouldReturn_FalseException()
        {
            var images = Fixture.CreateMany<Image>(5).ToList();

            _dbClientMock.Setup(s => s.AddImages(null, null, CancellationToken.None)).Throws<Exception>();

            var result = _dbServiceClient.AddImages(images)
                .GetAwaiter()
                .GetResult();

            _loggerMock.Verify(v => v.Error(It.IsAny<string>()), Times.Once);
            result.Should().Be(false);
        }
    }
}
