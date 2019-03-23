using Autofac;
using Autofac.Features.ResolveAnything;
using Radio.Infrastructure.DbAccess.Configuration;

namespace Radio.Startup.Console
{
    public static class Bootstrapper
    {
        public static IContainer BootstrapContainerForDbInitialization()
        {
            var containerBuilder = new ContainerBuilder();

            Radio.Infrastructure.DependencyRegistry.Configure(containerBuilder);
            Radio.Infrastructure.DbAccess.DependencyRegistry.Configure(containerBuilder);

            containerBuilder.RegisterType<MigrationsContextOptionsProvider>().As<IContextOptionsProvider>().InstancePerDependency();
            containerBuilder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

            return containerBuilder.Build();
        }

        public static IContainer BootstrapContainer()
        {
            var containerBuilder = new ContainerBuilder();
            Radio.Core.DependencyRegistry.Configure(containerBuilder);
            Radio.Infrastructure.DependencyRegistry.Configure(containerBuilder);
            Radio.Infrastructure.DbAccess.DependencyRegistry.Configure(containerBuilder);

            containerBuilder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

            return containerBuilder.Build();
        }
    }
}
