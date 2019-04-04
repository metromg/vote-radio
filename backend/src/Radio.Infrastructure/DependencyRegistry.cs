using System.IO;
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NLog.Extensions.Logging;
using Radio.Core;
using Radio.Core.Services;
using Radio.Infrastructure.Services;

namespace Radio.Infrastructure
{
    public static class DependencyRegistry
    {
        public static void Configure(ContainerBuilder builder)
        {
            // Configuration
            builder.Register(_ => BuildConfiguration()).As<IConfiguration>().SingleInstance();
            builder.RegisterGeneric(typeof(OptionsManager<>)).As(typeof(IOptions<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(OptionsManager<>)).As(typeof(IOptionsSnapshot<>)).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(OptionsMonitor<>)).As(typeof(IOptionsMonitor<>)).SingleInstance();
            builder.RegisterGeneric(typeof(OptionsFactory<>)).As(typeof(IOptionsFactory<>)).InstancePerDependency();
            builder.RegisterGeneric(typeof(OptionsCache<>)).As(typeof(IOptionsMonitorCache<>)).SingleInstance();
            builder.AddOption<EnvironmentOptions>(EnvironmentOptions.NAME);

            // Logger
            builder.RegisterType<LoggerFactory>().UsingConstructor(typeof(System.Collections.Generic.IEnumerable<ILoggerProvider>), typeof(IOptionsMonitor<LoggerFilterOptions>)).As<ILoggerFactory>().SingleInstance();
            builder.RegisterInstance(new NLogLoggerProvider()).As<ILoggerProvider>().SingleInstance();
            builder.RegisterGeneric(typeof(Logger<>)).As(typeof(ILogger<>));
            builder.RegisterType<Logger<ILogger>>().As(typeof(ILogger));

            // Services
            builder.RegisterType<JsonSerializationService>().As<ISerializationService>().InstancePerDependency();
        }

        public static void AddOption<TOptions>(this ContainerBuilder builder, string name)
            where TOptions : class
        {
            builder.Register(c => new ConfigurationChangeTokenSource<TOptions>(name, c.Resolve<IConfiguration>())).As(typeof(IOptionsChangeTokenSource<TOptions>)).SingleInstance();
            builder.Register(c => new ConfigureOptionsFromConfiguration<TOptions>(name, c.Resolve<IConfiguration>())).As(typeof(IConfigureOptions<TOptions>)).SingleInstance();
        }

        private static IConfiguration BuildConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
