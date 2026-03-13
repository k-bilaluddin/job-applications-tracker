namespace JobApplicationTracker.Api.Messaging.Publishers
{
    public interface IMessagePublisher
    {
        Task PublishAsync<T>(string queueName, T message, CancellationToken cancellationToken = default);
    }
}
