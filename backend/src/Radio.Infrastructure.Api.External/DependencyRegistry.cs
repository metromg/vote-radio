using Autofac;
using AutoMapper;
using Radio.Infrastructure.Api.External.Hubs;
using Radio.Infrastructure.Api.External.Mapping;
using Radio.Infrastructure.Api.External.Mapping.Profiles;

namespace Radio.Infrastructure.Api.External
{
    public static class DependencyRegistry
    {
        public static void Configure(ContainerBuilder builder)
        {
            // Hubs
            builder.RegisterType<RadioHubMessageQueueBridge>().AsSelf().SingleInstance();
            builder.RegisterType<RadioHubMessageDispatcher>().AsSelf().SingleInstance();

            // Mapping
            builder.RegisterType<MapperFactory>().AsSelf().SingleInstance();
            builder.Register(c => c.Resolve<MapperFactory>().GetMapper(c.Resolve<ILifetimeScope>())).As<IMapper>().InstancePerLifetimeScope();

            // Mapping Profiles
            builder.RegisterType<PlaybackMappingProfile>().As<MappingProfileBase>().InstancePerDependency();
            builder.RegisterType<VotingMappingProfile>().As<MappingProfileBase>().InstancePerDependency();
        }
    }
}
