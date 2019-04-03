using Autofac;
using Radio.Infrastructure.Api.Services;

namespace Radio.Infrastructure.Api
{
    public static class DependencyRegistry
    {
        public static void Configure(ContainerBuilder builder)
        {
            builder.RegisterType<SimpleUserIdentificationService>().As<ISimpleUserIdentificationService>().InstancePerDependency();
        }
    }
}
