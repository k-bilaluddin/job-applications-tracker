using MongoDB.Bson.Serialization.Attributes;

namespace JobApplicationTracker.Api.Models.Persistence.Documents
{
    public class InterviewRoundDocument
    {
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        [BsonElement("id")]
        public string Id { get; set; } = default!;

        [BsonElement("roundNumber")]
        public int RoundNumber { get; set; }

        [BsonElement("interviewType")]
        public string InterviewType { get; set; } = default!;

        [BsonElement("interviewDate")]
        public DateTime? InterviewDate { get; set; }

        [BsonElement("interviewerName")]
        public string? InterviewerName { get; set; }

        [BsonElement("interviewerEmail")]
        public string? InterviewerEmail { get; set; }

        [BsonElement("outcome")]
        public string? Outcome { get; set; }

        [BsonElement("notes")]
        public string? Notes { get; set; }
    }
}