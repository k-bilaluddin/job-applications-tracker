namespace JobApplicationTracker.Contracts.Messaging.Events
{
    public sealed class JobApplicationStatusChangedEvent
    {
        public string ApplicationId { get; set; } = string.Empty;
        public string PreviousStatus { get; set; } = string.Empty;
        public string NewStatus { get; set; } = string.Empty;
        public DateTime ChangedAtUtc { get; set; }
    }
}