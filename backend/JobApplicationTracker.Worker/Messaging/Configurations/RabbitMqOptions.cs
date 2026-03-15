namespace JobApplicationTracker.Worker.Messaging.Configurations
{
    public class RabbitMqOptions
    {
        public const string SectionName = "RabbitMq";
        public string ConnectionString { get; set; } = string.Empty;
        public string JobApplicationCreatedQueueName { get; set; } = string.Empty;
        public string JobApplicationUpdatedQueueName { get; set; } = string.Empty;
    }
}