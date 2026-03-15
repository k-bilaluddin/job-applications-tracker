using RabbitMQ.Client;

namespace JobApplicationTracker.Api.Messaging.Connection
{
    public interface IRabbitMqConnectionProvider : IAsyncDisposable
    {
        Task<IConnection> GetConnectionAsync(CancellationToken cancellationToken = default);
        Task<IChannel> CreateChannelAsync(CancellationToken cancellationToken = default);
    }
}
