using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.Hosting;
using Radio.Infrastructure.Synchronization;

namespace Radio.Startup.Console
{
    public class SynchronizationService : IHostedService
    {
        private readonly IContainer _rootContainer;

        public SynchronizationService(IContainer rootContainer)
        {
            _rootContainer = rootContainer;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _rootContainer.Resolve<JobsBootstrapper>().Bootstrap();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
