using AutoFixture;
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
             // Empty by design ;)
        }
    }
}