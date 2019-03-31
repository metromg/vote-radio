using Autofac;
using AutoMapper;
using Radio.Infrastructure.Api.Mapping;
using Radio.Infrastructure.Api.Mapping.Profiles;

namespace Radio.Infrastructure.Api
{
    public static class DependencyRegistry
    {
        public static void Configure(ContainerBuilder builder)
        {
            // Mapping
            builder.RegisterType<MapperFactory>().AsSelf().SingleInstance();
            builder.Register(c => c.Resolve<MapperFactory>().GetMapper(c.Resolve<ILifetimeScope>())).As<IMapper>().InstancePerLifetimeScope();

            // Mapping Profiles
            builder.RegisterType<VotingMappingProfile>().As<MappingProfileBase>().InstancePerDependency();
        }
    }
}
