using Autofac;
using Radio.Core;

namespace Radio.Infrastructure.DbAccess.Configuration
{
    public static class DbInitializer
    {
        public static void Initialize(IContainer rootContainer)
        {
            var unitOfWorkFactory = rootContainer.Resolve<IUnitOfWorkFactory<Context>>();

            using (var unitOfWork = unitOfWorkFactory.Begin())
            {
                unitOfWork.Dependent.Database.EnsureCreated();
            }
        }
    }
}
