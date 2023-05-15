using AutoFixture;
using DbService.Interfaces;
using DbService.Models;
using DbService.Services;
using FluentAssertions;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using Serilog;

namespace UnitTests.DatabaseService
{
    [TestFixture]
    public class PostServiceTests : BaseTest
    {
        private Mock<ILogger> _loggerMock;
        private Mock<IRepository<Post>> _postRepositoryMock;
        private Mock<IHistoryService> _historyServiceMock;
        private Mock<IImageService> _imageServiceMock;

        private IPostService _postService;

        [SetUp]
        public void Setup()
        {
            _postRepositoryMock = new Mock<IRepository<Post>>();
            _historyServiceMock = new Mock<IHistoryService>();
            _imageServiceMock = new Mock<IImageService>();
            _loggerMock = new Mock<ILogger>();

            _postService = new PostService(_postRepositoryMock.Object, _imageServiceMock.Object, _historyServiceMock.Object, _loggerMock.Object);
        }

        [Test]
        [TestCase(true, 0)]
        [TestCase(false, 1)]
        public void SaveHistory_ShouldReturn_Bool(bool insertResult, int errorCount)
        {
            var post = Fixture.Create<GrpcHelper.DbService.Post>();

            _postRepositoryMock.Setup(s => s.Insert(It.IsAny<Post>(), It.IsAny<InsertOneOptions>(), CancellationToken.None)).ReturnsAsync(insertResult);

            _postService = new PostService(_postRepositoryMock.Object, _imageServiceMock.Object, _historyServiceMock.Object, _loggerMock.Object);

            var result = _postService.SavePost(post)
                .GetAwaiter()
                .GetResult();

            _loggerMock.Verify(v => v.Error(It.IsAny<string>()), Times.Exactly(errorCount));

            result.Should().Be(insertResult);
        }

        [Test]
        public void GetHistory_ShouldReturn_HistoryList()
        {
            var postObjectId = Fixture.Create<ObjectId>();
            
            var postList = Fixture.Build<Post>()
                .With(w => w.Id, postObjectId)
                .With(w => w.PostDate, new DateTime().ToUniversalTime())
                .CreateMany(5)
                .ToList();

            var imageStream = Fixture.Create<MemoryStream>();
            var image = Fixture.Build<Image>()
                .With(w => w.GridFsId, postObjectId)
                .Create();

            postList[0].Id = postObjectId; // one should be not filtered by history

            var tags = Fixture.CreateMany<string>(5).ToList();
            var settings = Fixture.Create<PosterSettings>();
            var histories = Fixture.Build<History>()
                .With(w => w.EntityId, postList.First().Id)
                .CreateMany()
                .ToList();

            _postRepositoryMock
                .Setup(s => s.FindMany(It.IsAny<FilterDefinition<Post>>(), It.IsAny<FindOptions>(), CancellationToken.None))
                .ReturnsAsync(postList);

            _historyServiceMock
                .Setup(s => s.GetHistory(postList.Select(p => p.Id), settings.Source, settings.Group))
                .ReturnsAsync(histories);

            _imageServiceMock
                .Setup(s => s.GetImagesForPost(It.IsAny<ObjectId>()))
                .ReturnsAsync(() => new List<(Image image, MemoryStream stream)> {(image, imageStream)});

            var result = _postService.GetPostByTags(tags, settings)
                .GetAwaiter()
                .GetResult();

            _loggerMock.Verify(v => v.Error(It.IsAny<string>()), Times.Never);

            result.Should().NotBeNull();
            result.PostId.Should().Be(postList.First().PostId);
            result.Source.Should().Be(postList.First().Source);
        }
    }
}
