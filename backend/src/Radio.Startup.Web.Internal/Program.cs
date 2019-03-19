using System.IO;
using Autofac;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Radio.Startup.Web.Internal
{
    public class Program
    {
        private static IContainer s_rootContainer;

        public static void Main(string[] args)
        {
            s_rootContainer = Bootstrapper.BootstrapContainer();

            CreateDefaultBuilder(args)
                .ConfigureServices(services => services.AddSingleton(s_rootContainer))
                .UseStartup<Startup>()
                .Build()
                .Run();
        }

        public static IWebHostBuilder CreateDefaultBuilder(string[] args)
        {
            var builder = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseDefaultServiceProvider((context, options) =>
                {
                    options.ValidateScopes = context.HostingEnvironment.IsDevelopment();
                });

            return builder;
        }
    }
}
