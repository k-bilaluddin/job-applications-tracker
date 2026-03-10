namespace JobApplicationTracker.Api.Models.Persistence
{
    public class MongoDbOptions
    {
        public string ConnectionString { get; set; } = default!;
        public string DatabaseName { get; set; } = default!;
        public string JobApplicationsCollectionName { get; set; } = "jobApplications";
    }
}
