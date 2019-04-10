using System;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using NUnit.Framework;
using Radio.Core.Services;

namespace Radio.Tests.Integration.Infrastructure.Messaging
{
    [TestFixture]
    public class MessageQueueServiceTests
    {
        private IContainer _rootContainer;
        private IMessageQueueService _messageQueueService;

        [SetUp]
        public void Setup()
        {
            _rootContainer = IntegrationTestHelper.SetUp().Build();
            _messageQueueService = _rootContainer.Resolve<IMessageQueueService>();
        }

        [Test]
        public void MessageQueueService_IsSingleton()
        {
            // Arrange
            var lifeTimeScope1 = _rootContainer.BeginLifetimeScope();
            var lifeTimeScope2 = _rootContainer.BeginLifetimeScope();

            // Act
            var messageQueueService1 = lifeTimeScope1.Resolve<IMessageQueueService>();
            var messageQueueService2 = lifeTimeScope2.Resolve<IMessageQueueService>();

            // Assert
            Assert.That(messageQueueService1, Is.SameAs(messageQueueService2));

            lifeTimeScope1.Dispose();
            lifeTimeScope2.Dispose();
        }

        [Test]
        public void MessageQueueService_CanSendAndReceive()
        {
            // Arrange & Act
            var cancellationTokenSource = new CancellationTokenSource(20000);
            var taskCompletionSource = new TaskCompletionSource<Message>();

            Task.Factory.StartNew(() =>
            {
                _messageQueueService.Receive()
                    .Subscribe(message => taskCompletionSource.SetResult(message));

                _messageQueueService.Send(Message.VotingMessage);
            });

            cancellationTokenSource.Token.Register(() => taskCompletionSource.TrySetCanceled(), useSynchronizationContext: false);

            var result = taskCompletionSource.Task.Result;

            // Assert
            Assert.That(result, Is.EqualTo(Message.VotingMessage));
        }
    }
}
