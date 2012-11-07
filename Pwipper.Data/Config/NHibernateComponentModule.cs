using System.Configuration;
using Autofac;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Dialect;
using Pwipper.Core.Config;
using Configuration = NHibernate.Cfg.Configuration;
using Module = Autofac.Module;

namespace Pwipper.Data.Config
{
    public class NHibernateComponentModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => CreateNHibernateConfig(c.Resolve<AppConfig>())).As<Configuration>().SingleInstance();
            builder.Register(c => c.Resolve<Configuration>().BuildSessionFactory()).As<ISessionFactory>().SingleInstance();
            builder.Register(c => c.Resolve<ISessionFactory>().OpenSession()).As<ISession>().InstancePerLifetimeScope();
        }

        private Configuration CreateNHibernateConfig(AppConfig config)
        {
            var nhConfig = new Configuration();
            nhConfig.SetProperty(Environment.Dialect, typeof(MsSql2008Dialect).AssemblyQualifiedName);
            var connectionString = ConfigurationManager.AppSettings["SQLSERVER_CONNECTION_STRING"] ?? config.ConnectionString;
            nhConfig.SetProperty(Environment.ConnectionString, connectionString);
            nhConfig.AddAssembly(ThisAssembly);

            return nhConfig;
        }
    }
}
