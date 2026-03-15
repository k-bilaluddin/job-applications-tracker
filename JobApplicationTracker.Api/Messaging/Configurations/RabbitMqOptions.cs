namespace JobApplicationTracker.Api.Messaging.Configurations
{
    public class RabbitMqOptions
    {
        public const string SectionName = "RabbitMq";
        public string ConnectionString { get; set; }
        public string JobApplicationCreatedQueueName { get; set; }
        public string JobApplicationUpdatedQueueName { get; set; }
    }
}
