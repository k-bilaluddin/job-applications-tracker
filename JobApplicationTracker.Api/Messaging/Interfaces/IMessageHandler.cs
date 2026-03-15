namespace JobApplicationTracker.Api.Messaging.Interfaces
{
    public interface IMessageHandler<T>
    {
        Task HandleAsync(T message, CancellationToken cancellationToken = default);
    }
}
