using JobApplicationTracker.Api.Messaging.Consumers;

namespace JobApplicationTracker.Api.Messaging
{
    public sealed class MessagingHostedService : BackgroundService
    {
        private readonly IEnumerable<IMessageConsumer> _consumers;

        public MessagingHostedService(IEnumerable<IMessageConsumer> consumers)
        {
            _consumers = consumers;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            foreach (var consumer in _consumers)
            {
                await consumer.StartAsync(stoppingToken);
            }

            try
            {
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
            catch (OperationCanceledException)
            {
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (var consumer in _consumers)
            {
                await consumer.DisposeAsync();
            }

            await base.StopAsync(cancellationToken);
        }
    }
}
