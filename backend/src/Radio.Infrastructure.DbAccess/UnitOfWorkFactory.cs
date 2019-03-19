using System;
using Autofac;
using Microsoft.EntityFrameworkCore;
using Radio.Core;
using Radio.Infrastructure.DbAccess.Configuration;
using Radio.Infrastructure.DbAccess.Extensions;

namespace Radio.Infrastructure.DbAccess
{
    public abstract class UnitOfWorkFactoryBase<TUnitOfWork>
        where TUnitOfWork : IUnitOfWork
    {
        private readonly ILifetimeScope _lifetimeScope;

        protected UnitOfWorkFactoryBase(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public TUnitOfWork Begin()
        {
            var childLifetimeScope = BeginChildLifetimeScope(_lifetimeScope);
            var context = CreateContextInLifetimeScope(childLifetimeScope);

            RegisterContextSpecificComponents(context, childLifetimeScope);

            var unitOfWork = CreateUnitOfWork(context, childLifetimeScope);

            RegisterUnitOfWorkComponent(unitOfWork, childLifetimeScope);

            return unitOfWork;
        }

        private static ILifetimeScope BeginChildLifetimeScope(ILifetimeScope parentLifetimeScope)
        {
            var isTopLevelUnitOfWork = parentLifetimeScope.ResolveOptional<IUnitOfWork>() == null;

            return isTopLevelUnitOfWork
                ? parentLifetimeScope.BeginLifetimeScope(Constants.UnitOfWork.TOP_LEVEL_LIFETIME_SCOPE_TAG)
                : parentLifetimeScope.BeginLifetimeScope();
        }

        private static DbContext CreateContextInLifetimeScope(ILifetimeScope childLifetimeScope)
        {
            var context = new Context(
                childLifetimeScope.Resolve<IContextOptionsProvider>(),
                childLifetimeScope.Resolve<IModelCreator>()
            );

            return context;
        }

        private static void RegisterContextSpecificComponents(DbContext context, ILifetimeScope childLifetimeScope)
        {
            var dbContextRegistrationSource = new AutofacDbContextRegistrationSource(context);
            childLifetimeScope.ComponentRegistry.AddRegistrationSource(dbContextRegistrationSource);
        }

        protected abstract TUnitOfWork CreateUnitOfWork(DbContext context, ILifetimeScope lifetimeScope);

        private static void RegisterUnitOfWorkComponent(IUnitOfWork context, ILifetimeScope childLifetimeScope)
        {
            var unitOfWorkRegistrationSource = new AutofacUnitOfWorkRegistrationSource(context);
            childLifetimeScope.ComponentRegistry.AddRegistrationSource(unitOfWorkRegistrationSource);
        }
    }

    public class UnitOfWorkFactory<T> : UnitOfWorkFactoryBase<IUnitOfWork<T>>, IUnitOfWorkFactory<T>
    {
        public UnitOfWorkFactory(ILifetimeScope lifetimeScope)
            : base(lifetimeScope)
        {
        }

        protected override IUnitOfWork<T> CreateUnitOfWork(DbContext context, ILifetimeScope lifetimeScope)
        {
            return new UnitOfWork<T>(context, lifetimeScope, lifetimeScope.Resolve<Lazy<T>>());
        }
    }

    public class UnitOfWorkFactory<T1, T2> : UnitOfWorkFactoryBase<IUnitOfWork<T1, T2>>, IUnitOfWorkFactory<T1, T2>
    {
        public UnitOfWorkFactory(ILifetimeScope lifetimeScope)
            : base(lifetimeScope)
        {
        }

        protected override IUnitOfWork<T1, T2> CreateUnitOfWork(DbContext context, ILifetimeScope lifetimeScope)
        {
            return new UnitOfWork<T1, T2>(context, lifetimeScope, lifetimeScope.Resolve<Lazy<T1>>(), lifetimeScope.Resolve<Lazy<T2>>());
        }
    }

    public class UnitOfWorkFactory<T1, T2, T3> : UnitOfWorkFactoryBase<IUnitOfWork<T1, T2, T3>>, IUnitOfWorkFactory<T1, T2, T3>
    {
        public UnitOfWorkFactory(ILifetimeScope lifetimeScope)
            : base(lifetimeScope)
        {
        }

        protected override IUnitOfWork<T1, T2, T3> CreateUnitOfWork(DbContext context, ILifetimeScope lifetimeScope)
        {
            return new UnitOfWork<T1, T2, T3>(context, lifetimeScope, lifetimeScope.Resolve<Lazy<T1>>(), lifetimeScope.Resolve<Lazy<T2>>(), lifetimeScope.Resolve<Lazy<T3>>());
        }
    }
}
