using Autofac;
using Autofac.Features.ResolveAnything;

namespace Radio.Startup.Console
{
    public static class Bootstrapper
    {
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
