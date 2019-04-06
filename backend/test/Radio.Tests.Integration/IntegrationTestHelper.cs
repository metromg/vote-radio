using Autofac;
using Autofac.Features.ResolveAnything;

namespace Radio.Tests.Integration
{
    public static class IntegrationTestHelper
    {
        public static ContainerBuilder SetUp()
        {
            var containerBuilder = new ContainerBuilder();
            Radio.Core.DependencyRegistry.Configure(containerBuilder);
            Radio.Infrastructure.DependencyRegistry.Configure(containerBuilder);
            Radio.Infrastructure.DbAccess.DependencyRegistry.Configure(containerBuilder);
            Radio.Infrastructure.Messaging.DependencyRegistry.Configure(containerBuilder);

            containerBuilder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

            return containerBuilder;
        }
    }
}
