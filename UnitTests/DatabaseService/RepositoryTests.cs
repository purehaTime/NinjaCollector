using DbService.Interfaces;
using DbService.Models;
using DbService.Repositories;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;

namespace UnitTests.DatabaseService
{

    /*
    [TestFixture]
    public class RepositoryTests
    {
        private IRepository<Post> _repository;

    }

    public class CustomGenericAttribute<T> : Attribute { }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class GenericTestCaseAttribute<T> : TestCaseAttribute, ITestBuilder
    {
        private readonly Type _type;
        public GenericTestCaseAttribute(Type type, params object[] arguments) : base(arguments)
        {
            _type = type;
        }

        IEnumerable<TestMethod> ITestBuilder.BuildFrom(IMethodInfo method, Test suite)
        {
            if (method.IsGenericMethodDefinition && _type != null)
            {
                var gm = method.MakeGenericMethod(_type);
                return BuildFrom(gm, suite);
            }
            return BuildFrom(method, suite);
        }
    }*/
}