using Autofac;
using Autofac.Features.ResolveAnything;
using Microsoft.EntityFrameworkCore.Design;

namespace Radio.Infrastructure.DbAccess.Configuration
{
    public class DesignTimeMigrationsContextFactory : IDesignTimeDbContextFactory<Context>
    {
        public Context CreateDbContext(string[] args)
        {
            var containerBuilder = new ContainerBuilder();
            Radio.Infrastructure.DependencyRegistry.Configure(containerBuilder);
            Radio.Infrastructure.DbAccess.DependencyRegistry.Configure(containerBuilder);

            containerBuilder.RegisterType<MigrationsContextOptionsProvider>().As<IContextOptionsProvider>().InstancePerDependency();
            containerBuilder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

            var container = containerBuilder.Build();

            return container.Resolve<Context>();
        }
    }
}
