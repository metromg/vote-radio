using Autofac;
using Radio.Core;
using Radio.Core.Domain;
using Radio.Core.Domain.Voting;
using Radio.Infrastructure.DbAccess.Configuration;
using Radio.Infrastructure.DbAccess.Domain;
using Radio.Infrastructure.DbAccess.Domain.Voting;

namespace Radio.Infrastructure.DbAccess
{
    public static class DependencyRegistry
    {
        public static void Configure(ContainerBuilder builder)
        {
            // Configuration
            builder.RegisterType<ContextOptionsProvider>().As<IContextOptionsProvider>().InstancePerDependency();
            builder.RegisterType<ModelCreator>().As<IModelCreator>().InstancePerDependency();

            // Domain
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerDependency();
            builder.RegisterType<VotingCandidateRepository>().As<IVotingCandidateRepository>().InstancePerDependency();

            // UnitOfWork
            builder.RegisterGeneric(typeof(UnitOfWorkFactory<>)).As(typeof(IUnitOfWorkFactory<>)).InstancePerDependency();
            builder.RegisterGeneric(typeof(UnitOfWorkFactory<,>)).As(typeof(IUnitOfWorkFactory<,>)).InstancePerDependency();
            builder.RegisterGeneric(typeof(UnitOfWorkFactory<,,>)).As(typeof(IUnitOfWorkFactory<,,>)).InstancePerDependency();
        }
    }
}
