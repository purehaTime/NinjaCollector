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
        }


        [Test]
        public void SavePost_ShouldReturn_True()
        {
            var post = Fixture.Create<Post>();

            _postRepositoryMock.Setup(s => s.Insert(post, It.IsAny<InsertOneOptions>(), CancellationToken.None)).ReturnsAsync(true);

            _postService = new PostService(_postRepositoryMock.Object, _imageServiceMock.Object, _historyServiceMock.Object, _loggerMock.Object);

            var result = _postService.SavePost(post)
                .GetAwaiter()
                .GetResult();

            _loggerMock.Verify(v => v.Error(It.IsAny<string>()), Times.Never);

            result.Should().BeTrue();
        }

        [Test]
        public void SaveHistory_ShouldReturn_False()
        {
            var post = Fixture.Create<Post>();

            _postRepositoryMock.Setup(s => s.Insert(post, It.IsAny<InsertOneOptions>(), CancellationToken.None)).ReturnsAsync(false);

            _postService = new PostService(_postRepositoryMock.Object, _imageServiceMock.Object, _historyServiceMock.Object, _loggerMock.Object);

            var result = _postService.SavePost(post)
                .GetAwaiter()
                .GetResult();

            _loggerMock.Verify(v => v.Error(It.IsAny<string>()), Times.Once);

            result.Should().BeFalse();
        }

        [Test]
        public void GetHistory_ShouldReturn_HistoryList()
        {
            var historyObjectId = Fixture.Create<ObjectId>();
            var postObjectId = Fixture.Create<ObjectId>();
            
            var postList = Fixture.Build<Post>()
                .With(w => w.Id, historyObjectId)
                .CreateMany(5)
                .ToList();

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
                .Setup(s => s.GetHistory(postList.Select(s => s.Id), settings.Service, settings.ForGroup))
                .ReturnsAsync(histories);

            _postService = new PostService(_postRepositoryMock.Object, _imageServiceMock.Object, _historyServiceMock.Object, _loggerMock.Object);

            var result = _postService.GetPostByTags(tags, settings)
                .GetAwaiter()
                .GetResult();

            _loggerMock.Verify(v => v.Error(It.IsAny<string>()), Times.Never);

            result.Should().NotBeNull();
            result.Id.Should().Be(postObjectId);
        }
    }
}
