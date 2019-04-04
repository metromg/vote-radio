using Autofac;
using Radio.Infrastructure.Api.Services;

namespace Radio.Infrastructure.Api
{
    public static class DependencyRegistry
    {
        public static void Configure(ContainerBuilder builder)
        {
            builder.RegisterType<PrimitiveUserIdentificationService>().As<IPrimitiveUserIdentificationService>().InstancePerDependency();
        }
    }
}
