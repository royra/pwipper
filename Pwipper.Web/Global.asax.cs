using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Pwipper.Core.Config;
using Pwipper.Services;
using log4net;

namespace Pwipper
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                //"{controller}/{action}/{id}", // URL with parameters
                "{action}/{id}", // Only one controller
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }
        // ReSharper disable InconsistentNaming
        protected virtual void Application_Start()
        // ReSharper restore InconsistentNaming
        {
            ConfigurationHelpers.InitLogging();

            log.DebugFormat("Application_Start starting");

            var builder = ConfigurationHelpers.GetMinimalBuilder();
            ConfigurationHelpers.RegisterDatabaseModules(builder);
            ConfigurationHelpers.RegisterRealServices(builder);
            builder.RegisterModule(new AutofacWebTypesModule());
            builder.RegisterControllers(Assembly.GetExecutingAssembly());

            var container = builder.Build();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            var fillDatabaseUtils = container.Resolve<FillDatabaseUtils>();
            var config = container.Resolve<AppConfig>();

            if (config.RecreateSchema)
            {
                log.Debug("recreating schema");
                fillDatabaseUtils.RecreateSchema();
            }

            log.Debug("ensuring user");
            fillDatabaseUtils.EnsureUser();

            log.Debug("ensuring posts");
            fillDatabaseUtils.EnsurePosts(50);

            log.Debug("Application_Start ending");
        }
    }
}