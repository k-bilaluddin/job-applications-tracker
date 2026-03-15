using JobApplicationTracker.Api.Messaging.Configurations;
using RabbitMQ.Client;

namespace JobApplicationTracker.Api.Messaging.Connection
{
    public sealed class RabbitMqConnectionProvider : IRabbitMqConnectionProvider
    {
        private readonly RabbitMqOptions _options;
        private IConnection? _connection;

        public RabbitMqConnectionProvider(RabbitMqOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task<IConnection> GetConnectionAsync(CancellationToken cancellationToken = default)
        {
            if (_connection is not null)
            {
                return _connection;
            }

            var factory = new ConnectionFactory
            {
                Uri = new Uri(_options.ConnectionString)
            };

            _connection = await factory.CreateConnectionAsync(cancellationToken);
            return _connection;
        }

        public async Task<IChannel> CreateChannelAsync(CancellationToken cancellationToken = default)
        {
            var connection = await GetConnectionAsync(cancellationToken);
            return await connection.CreateChannelAsync(cancellationToken: cancellationToken);
        }

        public async ValueTask DisposeAsync()
        {
            if (_connection is not null)
            {
                await _connection.DisposeAsync();
            }
        }
    }
}
