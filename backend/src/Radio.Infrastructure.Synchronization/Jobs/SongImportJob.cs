using System;
using Autofac;
using FluentScheduler;
using Microsoft.Extensions.Logging;
using Radio.Infrastructure.Synchronization.Services;

namespace Radio.Infrastructure.Synchronization.Jobs
{
    public class SongImportJob : IJob
    {
        private readonly ILifetimeScope _rootLifetimeScope;

        public SongImportJob(ILifetimeScope rootLifetimeScope)
        {
            _rootLifetimeScope = rootLifetimeScope;
        }

        public void Execute()
        {
            using (var childLifetimeScope = _rootLifetimeScope.BeginLifetimeScope())
            {
                try
                {
                    childLifetimeScope.Resolve<ISongImportService>().Import();
                }
                catch (Exception ex)
                {
                    childLifetimeScope.Resolve<ILogger>().LogCritical("Unhandled exception in SongImportJob. {0}", ex);
                }
            }
        }
    }
}
