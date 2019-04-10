using Autofac;
using FluentScheduler;
using Radio.Infrastructure.Synchronization.Jobs;

namespace Radio.Infrastructure.Synchronization
{
    public class JobsBootstrapper
    {
        private readonly ILifetimeScope _rootLifetimeScope;

        public JobsBootstrapper(ILifetimeScope rootLifetimeScope)
        {
            _rootLifetimeScope = rootLifetimeScope;
        }

        public void Bootstrap()
        {
            var jobsRegistry = new Registry();
            jobsRegistry.Schedule(_rootLifetimeScope.Resolve<SongImportJob>()).NonReentrant().ToRunNow().AndEvery(10).Minutes();

            JobManager.Initialize(jobsRegistry);
        }
    }
}
