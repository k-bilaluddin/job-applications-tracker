namespace JobApplicationTracker.Api.Models.Entities
{
    public class InterviewRound
    {
        public string Id { get; set; } = default!;
        public int RoundNumber { get; set; }
        public string InterviewType { get; set; } = default!;
        public DateTime? InterviewDate { get; set; }
        public string? InterviewerName { get; set; }
        public string? InterviewerEmail { get; set; }
        public string? Outcome { get; set; }
        public string? Notes { get; set; }
    }
}