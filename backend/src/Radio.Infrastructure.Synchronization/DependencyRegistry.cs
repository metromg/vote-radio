using Autofac;
using Radio.Infrastructure.Synchronization.Jobs;
using Radio.Infrastructure.Synchronization.Services;

namespace Radio.Infrastructure.Synchronization
{
    public static class DependencyRegistry
    {
        public static void Configure(ContainerBuilder builder)
        {
            builder.RegisterType<JobsBootstrapper>().AsSelf().SingleInstance();

            // Jobs
            builder.RegisterType<SongImportJob>().AsSelf().SingleInstance();

            // Services
            builder.RegisterType<SongImportService>().As<ISongImportService>().InstancePerDependency();
        }
    }
}
