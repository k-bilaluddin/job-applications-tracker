namespace JobApplicationTracker.Api.Models.Request
{
    public class AddInterviewRoundRequest
    {
        public int RoundNumber { get; set; }
        public string InterviewType { get; set; } = default!;
        public DateTime? InterviewDate { get; set; }
        public string? InterviewerName { get; set; }
        public string? InterviewerEmail { get; set; }
        public string? Outcome { get; set; }
        public string? Notes { get; set; }
    }
}