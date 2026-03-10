using JobApplicationTracker.Api.Models.Request;
using JobApplicationTracker.Api.Models.Response;

namespace JobApplicationTracker.Api.Interfaces
{
    public interface IApplicationHandler
    {
        Task<JobApplicationResponse> CreateAsync(CreateJobApplicationRequest request, CancellationToken cancellationToken);
        Task<JobApplicationResponse?> GetByIdAsync(string id, CancellationToken cancellationToken);
        Task<List<JobApplicationResponse>> GetAllAsync(CancellationToken cancellationToken);
        Task<JobApplicationResponse?> UpdateAsync(string id, UpdateJobApplicationRequest request, CancellationToken cancellationToken);
        Task<JobApplicationResponse?> UpdateStatusAsync(string id, UpdateApplicationStatusRequest request, CancellationToken cancellationToken);

        Task<List<InterviewRoundResponse>?> GetInterviewsAsync(string applicationId, CancellationToken cancellationToken);
        Task<InterviewRoundResponse?> AddInterviewAsync(string applicationId, AddInterviewRoundRequest request, CancellationToken cancellationToken);
        Task<InterviewRoundResponse?> UpdateInterviewAsync(string applicationId, string interviewId, UpdateInterviewRoundRequest request, CancellationToken cancellationToken);
        Task<bool> DeleteInterviewAsync(string applicationId, string interviewId, CancellationToken cancellationToken);
    }
}
