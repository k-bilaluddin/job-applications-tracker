namespace JobApplicationTracker.Api.Messaging.Events
{
    public sealed class JobApplicationCreatedEvent
    {
        public string ApplicationId { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public DateTime AppliedOnUtc { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
