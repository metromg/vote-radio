using Autofac;
using Autofac.Features.ResolveAnything;
using NUnit.Framework;
using Radio.Infrastructure.DbAccess.Configuration;

namespace Radio.Tests.Integration
{
    [SetUpFixture]
    public class Setup
    {
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var containerBuilder = new ContainerBuilder();

            Radio.Infrastructure.DependencyRegistry.Configure(containerBuilder);
            Radio.Infrastructure.DbAccess.DependencyRegistry.Configure(containerBuilder);

            containerBuilder.RegisterType<MigrationsContextOptionsProvider>().As<IContextOptionsProvider>().InstancePerDependency();
            containerBuilder.RegisterSource(new AnyConcreteTypeNotAlreadyRegisteredSource());

            var rootContainer = containerBuilder.Build();

            DbInitializer.Initialize(rootContainer);
        }
    }
}
