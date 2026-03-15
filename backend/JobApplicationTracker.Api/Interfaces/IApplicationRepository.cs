using JobApplicationTracker.Api.Models.Entities;

namespace JobApplicationTracker.Api.Interfaces
{
    public interface IApplicationRepository
    {
        Task InsertAsync(JobApplication application, CancellationToken cancellationToken);
        Task<JobApplication?> GetByIdAsync(string id, CancellationToken cancellationToken);
        Task<List<JobApplication>> GetAllAsync(CancellationToken cancellationToken);
        Task<bool> UpdateAsync(JobApplication application, CancellationToken cancellationToken);
    }
}
