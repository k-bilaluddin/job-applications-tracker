namespace JobApplicationTracker.Api.Messaging.Events
{
    public sealed class JobApplicationUpdatedEvent
    {
        public string ApplicationId { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string PreviousStatus { get; set; } = string.Empty;
        public DateTime? NextActionDate { get; set; }
        public DateTime? PreviousNextActionDate { get; set; }
        public string? Notes { get; set; }
        public DateTime UpdatedAtUtc { get; set; }
    }
}