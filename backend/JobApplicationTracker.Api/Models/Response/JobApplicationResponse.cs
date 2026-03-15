namespace JobApplicationTracker.Api.Models.Response
{
    public class JobApplicationResponse
    {
        public string Id { get; set; } = default!;

        public string CompanyName { get; set; } = default!;
        public string JobTitle { get; set; } = default!;
        public string? Location { get; set; }
        public string? JobUrl { get; set; }

        public string SourceType { get; set; } = default!;
        public string? SourceReference { get; set; }

        public string Status { get; set; } = default!;
        public DateTime AppliedDate { get; set; }

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

        public List<InterviewRoundResponse> Interviews { get; set; } = new();

        public DateTime CreatedUtc { get; set; }
        public DateTime UpdatedUtc { get; set; }
    }
}
