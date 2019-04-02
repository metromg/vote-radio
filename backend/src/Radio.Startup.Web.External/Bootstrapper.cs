using Autofac;
using Autofac.Features.ResolveAnything;

namespace Radio.Startup.Web.External
{
    public static class Bootstrapper
    {
        public static IContainer BootstrapContainer()
        {
            var containerBuilder = new ContainerBuilder();
            Radio.Core.DependencyRegistry.Configure(containerBuilder);
            Radio.Infrastructure.DependencyRegistry.Configure(containerBuilder);
            Radio.Infrastructure.Api.DependencyRegistry.Configure(containerBuilder);
            Radio.Infrastructure.Api.External.DependencyRegistry.Configure(containerBuilder);
            Radio.Infrastructure.DbAccess.DependencyRegistry.Configure(containerBuilder);
            Radio.Infrastructure.Messaging.DependencyRegistry.Configure(containerBuilder);

            containerBuilder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

            return containerBuilder.Build();
        }
    }
}
