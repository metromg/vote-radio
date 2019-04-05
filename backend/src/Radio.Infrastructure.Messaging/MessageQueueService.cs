using System;
using System.Reactive.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Radio.Core.Services;

namespace Radio.Infrastructure.Messaging
{
    public class MessageQueueService : IMessageQueueService
    {
        private static readonly string EXCHANGE_NAME = "pubsub";

        private readonly IModel _channelModel;
        private readonly ISerializationService _serializationService;
        private readonly ILogger _logger;

        private EventingBasicConsumer _consumer;

        public MessageQueueService(IModel channelModel, ISerializationService serializationService, ILogger logger)
        {
            _channelModel = channelModel;
            _serializationService = serializationService;
            _logger = logger;

            Initialize();
        }

        public void Send(Message message)
        {
            var serializedMessage = _serializationService.Serialize(message);
            var body = Encoding.UTF8.GetBytes(serializedMessage);

            // https://www.rabbitmq.com/releases/rabbitmq-dotnet-client/v3.3.0/rabbitmq-dotnet-client-3.3.0-user-guide.pdf 
            // Section "IModel should not be shared between threads"
            lock (_channelModel)
            {
                _channelModel.BasicPublish(
                    exchange: EXCHANGE_NAME,
                    routingKey: string.Empty,
                    basicProperties: null,
                    body: body);
            }
        }

        public IObservable<Message> Receive()
        {
            return Observable
                .FromEventPattern<BasicDeliverEventArgs>(
                    x => _consumer.Received += x,
                    x => _consumer.Received -= x
                )
                .Select(x =>
                {
                    var body = x.EventArgs.Body;
                    var serializedMessage = Encoding.UTF8.GetString(body);

                    return _serializationService.Deserialize<Message>(serializedMessage);
                });
        }

        private void Initialize()
        {
            _logger.LogInformation("Initializing RabbitMQ message queue channel...");

            // https://www.rabbitmq.com/releases/rabbitmq-dotnet-client/v3.3.0/rabbitmq-dotnet-client-3.3.0-user-guide.pdf 
            // Section "IModel should not be shared between threads"
            lock (_channelModel)
            {
                _channelModel.ExchangeDeclare(EXCHANGE_NAME, "fanout");

                var queueName = _channelModel.QueueDeclare().QueueName;
                _channelModel.QueueBind(
                    queue: queueName,
                    exchange: EXCHANGE_NAME,
                    routingKey: string.Empty);

                _consumer = new EventingBasicConsumer(_channelModel);

                _channelModel.BasicConsume(
                    queue: queueName,
                    autoAck: true,
                    consumer: _consumer);
            }
        }
    }
}
