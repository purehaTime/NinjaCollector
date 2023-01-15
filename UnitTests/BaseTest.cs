using AutoFixture;
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
    }
}