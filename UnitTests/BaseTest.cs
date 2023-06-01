using AutoFixture;
using AutoFixture.AutoMoq;
using Grpc.Core;
using MongoDB.Bson;
using Reddit.Controllers;


namespace UnitTests
{
    public class BaseTest
    {
        protected Fixture Fixture;

        public BaseTest()
        {
            Fixture = new Fixture();
        }

        [SetUp]
        public void BaseSetup()
        {
            Fixture.Register<byte[], MemoryStream>(data => new MemoryStream(data));
            Fixture.Register<ObjectId>(ObjectId.GenerateNewId);
            Fixture.Register<Dispatch>(() => null);

            Fixture.Customize(new AutoMoqCustomization());
        }

        protected AsyncUnaryCall<T> GetAsyncUnaryCallSuccess<T>(T response)
        {
            return new AsyncUnaryCall<T>(
                Task.FromResult(response),
                Task.FromResult(new Metadata()),
                () => Status.DefaultSuccess,
                () => new Metadata(),
                () => { });
        }

        protected AsyncUnaryCall<T> GetAsyncUnaryCallFailed<T>(T response)
        {
            return new AsyncUnaryCall<T>(
                Task.FromResult(response),
                Task.FromResult(new Metadata()),
                () => Status.DefaultCancelled,
                () => new Metadata(),
                () => { });
        }

        protected AsyncClientStreamingCall<TRequest, TResponse> GetAsyncClientStreamingCallSuccess<TRequest, TResponse>(TRequest request, TResponse response)
        {
           return new AsyncClientStreamingCall<TRequest, TResponse>(
               new ClientStreamWriter<TRequest>(request),
               Task.FromResult(response),
               Task.FromResult(Metadata.Empty),
               () => Status.DefaultSuccess,
               () => Metadata.Empty,
               () => { });
        }

        protected AsyncServerStreamingCall<TResponse> GetAsyncServerStreamingCallSuccess<TResponse>(TResponse response)
        {
            return new AsyncServerStreamingCall<TResponse>(new AsyncStreamReader<TResponse>(response),
                Task.FromResult(Metadata.Empty),
                () => Status.DefaultSuccess,
                () => Metadata.Empty,
                () => { });
        }

    }

    public class ClientStreamWriter<T> : IClientStreamWriter<T>
    {
        private T _request;

        public ClientStreamWriter(T request)
        {
            _request = request;
        }

        public Task WriteAsync(T message)
        {
            return Task.CompletedTask;
        }

        public WriteOptions WriteOptions { get; set; }
        public Task CompleteAsync()
        {
            return Task.CompletedTask;
        }
    }

    public class AsyncStreamReader<T> : IAsyncStreamReader<T>
    {
        public AsyncStreamReader(T data)
        {
            Current = data;
        }

        public Task<bool> MoveNext(CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        public T Current { get; }
    }
}