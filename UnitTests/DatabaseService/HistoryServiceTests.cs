using AutoFixture;
using DbService.Interfaces;
using DbService.Models;
using DbService.Services;
using FluentAssertions;
using MongoDB.Driver;
using Moq;
using Serilog;

namespace UnitTests.DatabaseService
{
    [TestFixture]
    public class HistoryServiceTests : BaseTest
    {

        private Mock<IRepository<History>> _historyRepositoryMock;
        private Mock<IPostService> _postServiceMock;
        private Mock<ILogger> _loggerMock;

        private IHistoryService _historyService;

        [SetUp]
        public void Setup()
        {
            _historyRepositoryMock = new Mock<IRepository<History>>();
            _postServiceMock  = new Mock<IPostService>();
            _loggerMock = new Mock<ILogger>();

            _historyService = new HistoryService(_historyRepositoryMock.Object, _postServiceMock.Object, _loggerMock.Object);
        }


        [Test]
        public void SaveHistory_ShouldReturn_True()
        {
            var history = Fixture.Create<History>();

            _historyRepositoryMock.Setup(s => s.Insert(history, It.IsAny<InsertOneOptions>(), CancellationToken.None)).ReturnsAsync(true);

            var result = _historyService.SaveHistory(history)
                .GetAwaiter()
                .GetResult();

            _loggerMock.Verify(v => v.Error(It.IsAny<string>()), Times.Never);

            result.Should().BeTrue();
        }

        [Test]
        public void SaveHistory_ShouldReturn_False()
        {
            var history = Fixture.Create<History>();

            _historyRepositoryMock.Setup(s => s.Insert(history, It.IsAny<InsertOneOptions>(), CancellationToken.None)).ReturnsAsync(false);

            var result = _historyService.SaveHistory(history)
                .GetAwaiter()
                .GetResult();

            _loggerMock.Verify(v => v.Error(It.IsAny<string>()), Times.Once);

            result.Should().BeFalse();
        }

        [Test]
        public void GetHistory_ShouldReturn_HistoryList()
        {
            var historyList = Fixture.CreateMany<History>(5);
            var entities = Fixture.CreateMany<string>();
            var serviceGroup = Fixture.Create<string>();

            _historyRepositoryMock
                .Setup(s => s.FindMany(It.IsAny<FilterDefinition<History>>(), It.IsAny<FindOptions>(), CancellationToken.None))
                .ReturnsAsync(historyList);

            var result = _historyService.GetHistory(entities, serviceGroup, serviceGroup)
                .GetAwaiter()
                .GetResult()
                .ToList();

            _loggerMock.Verify(v => v.Error(It.IsAny<string>()), Times.Never);

            result.Should().NotBeNull();
            result.Should().HaveCount(5);
        }
    }
}
