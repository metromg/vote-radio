using Autofac;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Radio.Core;
using Radio.Core.Services;

namespace Radio.Infrastructure.Messaging
{
    public static class DependencyRegistry
    {
        public static void Configure(ContainerBuilder builder)
        {
            // RabbitMQ Connection and Model
            builder.Register(c => BuildConnectionFactory(c.Resolve<IOptions<EnvironmentOptions>>())).As<IConnectionFactory>().InstancePerDependency();
            builder.Register(c => c.Resolve<IConnectionFactory>().CreateConnection()).As<IConnection>().SingleInstance();
            builder.Register(c => c.Resolve<IConnection>().CreateModel()).As<IModel>().SingleInstance();

            // Service
            builder.RegisterType<MessageQueueService>().As<IMessageQueueService>().SingleInstance();
        }

        private static IConnectionFactory BuildConnectionFactory(IOptions<EnvironmentOptions> options)
        {
            return new ConnectionFactory
            {
                HostName = options.Value.MessagingHost,
                AutomaticRecoveryEnabled = true
            };
        }
    }
}
