namespace JobApplicationTracker.Api.Models.Request
{
    public class UpdateApplicationStatusRequest
    {
        public string Status { get; set; } = default!;
        public DateTime StatusDate { get; set; }
        public string? Notes { get; set; }
        public DateTime? NextActionDate { get; set; }
    }
}
