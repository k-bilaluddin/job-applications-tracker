using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JobApplicationTracker.Api.Models.Persistence.Documents
{
    public class JobApplicationDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; } = default!;

        [BsonElement("companyName")]
        public string CompanyName { get; set; } = default!;

        [BsonElement("jobTitle")]
        public string JobTitle { get; set; } = default!;

        [BsonElement("location")]
        public string? Location { get; set; }

        [BsonElement("jobUrl")]
        public string? JobUrl { get; set; }

        [BsonElement("sourceType")]
        public string SourceType { get; set; } = default!;

        [BsonElement("sourceReference")]
        public string? SourceReference { get; set; }

        [BsonElement("status")]
        public string Status { get; set; } = default!;

        [BsonElement("appliedDate")]
        public DateTime AppliedDate { get; set; }

        [BsonElement("contactName")]
        public string? ContactName { get; set; }

        [BsonElement("contactEmail")]
        public string? ContactEmail { get; set; }

        [BsonElement("workMode")]
        public string? WorkMode { get; set; }

        [BsonElement("employmentType")]
        public string? EmploymentType { get; set; }

        [BsonElement("salaryMin")]
        public decimal? SalaryMin { get; set; }

        [BsonElement("salaryMax")]
        public decimal? SalaryMax { get; set; }

        [BsonElement("currency")]
        public string? Currency { get; set; }

        [BsonElement("salaryPeriod")]
        public string? SalaryPeriod { get; set; }

        [BsonElement("nextActionDate")]
        public DateTime? NextActionDate { get; set; }

        [BsonElement("notes")]
        public string? Notes { get; set; }

        [BsonElement("interviews")]
        public List<InterviewRoundDocument> Interviews { get; set; } = new();

        [BsonElement("createdUtc")]
        public DateTime CreatedUtc { get; set; }

        [BsonElement("updatedUtc")]
        public DateTime UpdatedUtc { get; set; }
    }
}
