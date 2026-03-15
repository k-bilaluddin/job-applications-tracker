using JobApplicationTracker.Contracts.Messaging.Events;
using JobApplicationTracker.Worker.Messaging.Configurations;
using JobApplicationTracker.Worker.Messaging.Connection;
using JobApplicationTracker.Worker.Messaging.Interfaces;
using JobApplicationTracker.Worker.Messaging.Serialization;

namespace JobApplicationTracker.Worker.Messaging.Consumers
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