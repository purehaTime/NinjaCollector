using AutoFixture;
using Grpc.Core;
using GrpcHelper.LogService;
using MongoDB.Bson;
using NUnit.Framework.Internal.Commands;

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
    }
}