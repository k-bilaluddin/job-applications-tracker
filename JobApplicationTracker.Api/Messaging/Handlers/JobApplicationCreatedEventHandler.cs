using JobApplicationTracker.Api.Messaging.Events;
using JobApplicationTracker.Api.Messaging.Interfaces;

namespace JobApplicationTracker.Api.Messaging.Handlers
{
    public sealed class JobApplicationCreatedEventHandler : IMessageHandler<JobApplicationCreatedEvent>
    {
        public async Task HandleAsync(JobApplicationCreatedEvent message, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"Job application created: {message.CompanyName} - {message.JobTitle}");

            await Task.CompletedTask;
        }
    }
}
