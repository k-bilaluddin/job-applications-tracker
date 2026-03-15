namespace JobApplicationTracker.Api.Models.Request
{
    public class SearchApplicationsRequest
    {
        public string? CompanyName { get; set; }
        public string? Status { get; set; }
        public string? Location { get; set; }
        public string? WorkMode { get; set; }
        public DateTime? AppliedAfter { get; set; }
        public DateTime? AppliedBefore { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;

        public string? SortBy { get; set; }
        public string? SortDirection { get; set; } // asc / desc
    }
}
