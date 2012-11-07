using Autofac;
using NUnit.Framework;
using Pwipper.Services;

namespace Pwipper.Tests.Core
{
    [TestFixture]
    public abstract class BaseFixture
    {
        protected IContainer Container;
        protected ILifetimeScope TestScope;

        [TestFixtureSetUp]
        public virtual void TestFixtureSetUp()
        {
            ConfigurationHelpers.InitLogging();
            var containerBuilder = GetContainerBuilderForFixture();
            Container = containerBuilder.Build();
        }

        [SetUp]
        public virtual void SetUp()
        {
            TestScope = Container.BeginLifetimeScope();
        }

        [TearDown]
        public virtual void TearDown()
        {
            if (TestScope != null)
            {
                TestScope.Dispose();
                TestScope = null;
            }
        }

        protected virtual ContainerBuilder GetContainerBuilderForFixture()
        {
            return ConfigurationHelpers.GetMinimalBuilder();
        }
    }
}
