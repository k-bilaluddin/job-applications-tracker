namespace JobApplicationTracker.Api.Models.Request
{
    public class UpdateJobApplicationRequest
    {
        public string JobTitle { get; set; } = default!;
        public string? Location { get; set; }
        public string? JobUrl { get; set; }

        public string? ContactName { get; set; }
        public string? ContactEmail { get; set; }

        public string? WorkMode { get; set; }
        public string? EmploymentType { get; set; }

        public decimal? SalaryMin { get; set; }
        public decimal? SalaryMax { get; set; }
        public string? Currency { get; set; }
        public string? SalaryPeriod { get; set; }

        public DateTime? NextActionDate { get; set; }
        public string? Notes { get; set; }
    }
}
