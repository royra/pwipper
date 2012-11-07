using System;
using System.Configuration;
using Autofac;
using NHibernate;
using NUnit.Framework;
using Pwipper.Services;
using Pwipper.Tests.Core;

namespace Pwipper.Tests.Data
{
    public abstract class DbTransactionTest : BaseFixture
    {
        protected override ContainerBuilder GetContainerBuilderForFixture()
        {
            var builder = base.GetContainerBuilderForFixture();
            ConfigurationHelpers.RegisterDatabaseModules(builder);
            ConfigurationHelpers.RegisterFillDatabaseUtils(builder);
            return builder;
        }

        public override void TestFixtureSetUp()
        {
            base.TestFixtureSetUp();
            var fillDatabaseUtils = Container.Resolve<FillDatabaseUtils>();
            fillDatabaseUtils.RecreateSchema();
        }

        protected ITransaction TestTransaction;
        protected ISession TestSession;

        public override void SetUp()
        {
            base.SetUp();
            TestSession = TestScope.Resolve<ISession>();
            TestTransaction = TestSession.BeginTransaction();
        }

        public override void TearDown()
        {
            if (TestTransaction != null && TestTransaction.IsActive)
                TestTransaction.Rollback();

            TestSession.Close();
            base.TearDown();
        }
    }
}