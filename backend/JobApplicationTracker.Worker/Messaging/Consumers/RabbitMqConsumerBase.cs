using JobApplicationTracker.Worker.Messaging.Connection;
using JobApplicationTracker.Worker.Messaging.Interfaces;
using JobApplicationTracker.Worker.Messaging.Serialization;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace JobApplicationTracker.Worker.Messaging.Consumers
{
    public abstract class RabbitMqConsumerBase<TMessage> : IMessageConsumer
    {
        private readonly IRabbitMqConnectionProvider _connectionProvider;
        private readonly IEventSerializer _serializer;
        private readonly IMessageHandler<TMessage> _handler;

        private IChannel? _channel;
        private string? _consumerTag;
        private readonly string _queueName;

        protected RabbitMqConsumerBase(
            IRabbitMqConnectionProvider connectionProvider,
            IEventSerializer serializer,
            IMessageHandler<TMessage> handler,
            string queueName)
        {
            _connectionProvider = connectionProvider;
            _serializer = serializer;
            _handler = handler;
            _queueName = queueName ?? throw new ArgumentNullException(nameof(queueName));
        }

        protected string QueueName => _queueName;

        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            _channel = await _connectionProvider.CreateChannelAsync(cancellationToken);

            await _channel.QueueDeclareAsync(
                queue: QueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: cancellationToken);

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (_, ea) =>
            {
                try
                {
                    var message = _serializer.Deserialize<TMessage>(ea.Body);

                    if (message is null)
                    {
                        await _channel.BasicNackAsync(ea.DeliveryTag, false, false, cancellationToken);
                        return;
                    }

                    await _handler.HandleAsync(message, cancellationToken);
                    await _channel.BasicAckAsync(ea.DeliveryTag, false, cancellationToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Consumer error for {typeof(TMessage).Name}: {ex.Message}");
                    await _channel.BasicNackAsync(ea.DeliveryTag, false, false, cancellationToken);
                }
            };

            _consumerTag = await _channel.BasicConsumeAsync(
                queue: QueueName,
                autoAck: false,
                consumer: consumer,
                cancellationToken: cancellationToken);
        }

        public async ValueTask DisposeAsync()
        {
            if (_channel is not null)
            {
                if (!string.IsNullOrWhiteSpace(_consumerTag))
                {
                    try
                    {
                        await _channel.BasicCancelAsync(_consumerTag);
                    }
                    catch
                    {
                    }
                }

                await _channel.DisposeAsync();
            }
        }
    }
}