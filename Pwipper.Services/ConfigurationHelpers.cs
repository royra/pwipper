using System.IO;
using System.Xml;
using System.Xml.Serialization;
using Autofac;
using Pwipper.Core.Config;
using Pwipper.Data.Config;
using log4net.Config;

namespace Pwipper.Services
{
    public static class ConfigurationHelpers
    {
        private const string APP_CONFIG_FILE_NAME = "pwipperConfig.xml";
        private const string LOG4_NET_FILE_NAME = "log4net.xml";

        public static ContainerBuilder GetMinimalBuilder()
        {
            var builder = new ContainerBuilder();
            builder.Register(c => ReadAppConfigConfig()).SingleInstance();

            return builder;
        }

        public static void RegisterDatabaseModules(ContainerBuilder builder)
        {
            builder.RegisterModule(new NHibernateComponentModule());
            builder.RegisterModule(new ReposComponentModule());
        }

        public static void RegisterRealServices(ContainerBuilder builder)
        {
            RegisterFillDatabaseUtils(builder);
        }

        public static void RegisterFillDatabaseUtils(ContainerBuilder builder)
        {
            builder.RegisterType<RandomTweetsEngine>().AsImplementedInterfaces().InstancePerDependency();
            builder.RegisterType<FillDatabaseUtils>().AsSelf().InstancePerDependency();
        }

        public static void InitLogging()
        {
            var logConfigPath = DirectoryUtils.SearchUp(Path.Combine("config", LOG4_NET_FILE_NAME), throwIfNotExists: true);

            XmlConfigurator.ConfigureAndWatch(new FileInfo(logConfigPath));
        }

        public static AppConfig ReadAppConfigConfig()
        {
            var configPath = DirectoryUtils.SearchUp(Path.Combine("config", APP_CONFIG_FILE_NAME), throwIfNotExists: true);
            using (var xmlReader = XmlReader.Create(configPath))
            {
                var xs = new XmlSerializer(typeof(AppConfig));
                return (AppConfig)xs.Deserialize(xmlReader);
            }
        }
    }
}
