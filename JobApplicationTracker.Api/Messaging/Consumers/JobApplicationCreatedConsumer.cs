using JobApplicationTracker.Api.Messaging.Configurations;
using JobApplicationTracker.Api.Messaging.Connection;
using JobApplicationTracker.Api.Messaging.Events;
using JobApplicationTracker.Api.Messaging.Interfaces;
using JobApplicationTracker.Api.Messaging.Serialization;
using Microsoft.Extensions.Options;

namespace JobApplicationTracker.Api.Messaging.Consumers
{
    public sealed class JobApplicationCreatedConsumer : RabbitMqConsumerBase<JobApplicationCreatedEvent>
    {
        public JobApplicationCreatedConsumer(
        IRabbitMqConnectionProvider connectionProvider,
        IEventSerializer serializer,
        IMessageHandler<JobApplicationCreatedEvent> handler,
        RabbitMqOptions options)
        : base(
            connectionProvider,
            serializer,
            handler,
            options.JobApplicationCreatedQueueName)
        {
        }
    }
}
