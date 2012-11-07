using System.Linq;
using Autofac;
using Pwipper.Core.DataInterfaces;

namespace Pwipper.Data.Config
{
    public class ReposComponentModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            foreach (var t in ThisAssembly.GetExportedTypes().Where(t => !t.IsAbstract && t.IsClass && typeof(IRepos).IsAssignableFrom(t)))
                builder.RegisterType(t).AsSelf().AsImplementedInterfaces();
        }
    }
}