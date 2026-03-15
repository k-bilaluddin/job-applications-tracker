using JobApplicationTracker.Api.Messaging.Interfaces;
using JobApplicationTracker.Contracts.Messaging.Events;

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
