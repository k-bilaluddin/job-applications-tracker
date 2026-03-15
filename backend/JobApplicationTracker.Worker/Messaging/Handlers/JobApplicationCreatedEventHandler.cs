using JobApplicationTracker.Contracts.Messaging.Events;
using JobApplicationTracker.Worker.Messaging.Interfaces;

namespace JobApplicationTracker.Worker.Messaging.Handlers
{
    public sealed class JobApplicationCreatedEventHandler : IMessageHandler<JobApplicationCreatedEvent>
    {
        public async Task HandleAsync(
            JobApplicationCreatedEvent message,
            CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"Job application created: {message.CompanyName} - {message.JobTitle}");
            await Task.CompletedTask;
        }
    }
}