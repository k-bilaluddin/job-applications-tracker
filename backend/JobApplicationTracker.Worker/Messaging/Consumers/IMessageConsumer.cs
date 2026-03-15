namespace JobApplicationTracker.Worker.Messaging.Consumers
{
    public interface IMessageConsumer : IAsyncDisposable
    {
        Task StartAsync(CancellationToken cancellationToken = default);
    }
}