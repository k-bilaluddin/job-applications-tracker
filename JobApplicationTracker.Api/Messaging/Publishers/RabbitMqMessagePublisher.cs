using RabbitMQ.Client;
using JobApplicationTracker.Api.Messaging.Configurations;
using JobApplicationTracker.Api.Messaging.Serialization;
using JobApplicationTracker.Api.Messaging.Connection;

namespace JobApplicationTracker.Api.Messaging.Publishers
{
    public sealed class RabbitMqMessagePublisher : IMessagePublisher
    {
        private readonly IRabbitMqConnectionProvider _connectionProvider;
        private readonly IEventSerializer _serializer;

        public RabbitMqMessagePublisher(
            IRabbitMqConnectionProvider connectionProvider,
            IEventSerializer serializer)
        {
            _connectionProvider = connectionProvider ?? throw new ArgumentNullException(nameof(connectionProvider));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        public async Task PublishAsync<T>(string queueName, T message, CancellationToken cancellationToken = default)
        {
            if (message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            await using var channel = await _connectionProvider.CreateChannelAsync(cancellationToken);

            await channel.QueueDeclareAsync(
                queue: queueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                cancellationToken: cancellationToken);

            var body = _serializer.Serialize(message);

            var properties = new BasicProperties
            {
                Persistent = true,
                ContentType = "application/json"
            };

            await channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: queueName,
                mandatory: false,
                basicProperties: properties,
                body: body,
                cancellationToken: cancellationToken);
        }
    }
}
