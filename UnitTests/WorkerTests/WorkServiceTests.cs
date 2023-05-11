using AutoFixture;
using FluentAssertions;
using ModelsHelper.Models;
using Moq;
using Serilog;
using Worker;
using Worker.Interfaces;

namespace UnitTests.WorkerTests
{
    [TestFixture]
    public class WorkServiceTests : BaseTest
    {
        private readonly Mock<ILogger> _loggerMock = new();
        private readonly Mock<IWorker> _workerMock = new();

        private IWorkService _workService;


        [SetUp]
        public void Setup()
        {
            _workService = new WorkService(new List<IWorker> { _workerMock.Object }, _loggerMock.Object);
        }

        [TearDown]
        public void Clear()
        {
            _workService.StopAll();
        }

        [Test]
        public void RunWorkers_Should_RunAllSettingsTask()
        {
            Init();
            var result = _workService.GetWorkers();

            result.Should().HaveCount(1);
            result.Should().AllSatisfy(a => a.Works.Should().HaveCount(5));
        }


        [Test]
        public void StopAll_ShouldCancelAllTasks()
        {
            Init();

            _workService.StopAll();
            var result = _workService.GetWorkers();

            var works = result.SelectMany(s => s.Works).ToList();
            works.Should().HaveCount(0);
        }

        [Test]
        public void RestartWorkers_Should_RestartAllTask()
        {
            Init();

            var isRestart = _workService.RestartWorkers().GetAwaiter().GetResult();
            var result = _workService.GetWorkers();

            result.Should().HaveCount(1);
            result.Should().AllSatisfy(a => a.Works.Should().HaveCount(5));
            isRestart.Should().BeTrue();
        }

        [Test]
        public void RestartWorker_Should_RestartTask()
        {
            Init();

            var workers = _workService.GetWorkers();
            var work = workers.SelectMany(s => s.Works).First();
            var isRestart = _workService.RestartWorker(work.TaskId, work.Settings.Id).GetAwaiter().GetResult();
            workers = _workService.GetWorkers();

            workers.Should().HaveCount(1);
            workers.Should().AllSatisfy(a => a.Works.Should().HaveCount(5));
            isRestart.Should().BeTrue();
        }

        private void Init()
        {
            var settings = Fixture.CreateMany<ParserSettings>(5).ToList();
            _workerMock.Setup(s => s.Init()).ReturnsAsync(() => settings);
            foreach (var setting in settings)
            {
                _workerMock.Setup(s => s.Run(setting)).ReturnsAsync(() => setting);
            }

            _workService.RunWorkers().GetAwaiter().GetResult();
        }
    }
}
