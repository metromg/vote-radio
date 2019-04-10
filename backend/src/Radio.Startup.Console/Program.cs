using System.Threading.Tasks;
using Autofac;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Radio.Infrastructure.DbAccess.Configuration;

namespace Radio.Startup.Console
{
    public class Program
    {
        private static IContainer s_rootContainer;

        public static void Main(string[] args)
        {
            var containerForDbInitialization = Bootstrapper.BootstrapContainerForDbInitialization();
            DbInitializer.Initialize(containerForDbInitialization);

            s_rootContainer = Bootstrapper.BootstrapContainer();

            var builder = new HostBuilder()
                .ConfigureServices(services => 
                {
                    services.AddSingleton(s_rootContainer);
                    services.AddSingleton<IHostedService, SynchronizationService>();
                });

            builder.RunConsoleAsync().GetAwaiter().GetResult();
        }
    }
}
