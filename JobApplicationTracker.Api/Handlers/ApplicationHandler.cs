using JobApplicationTracker.Api.Interfaces;
using JobApplicationTracker.Api.Messaging.Configurations;
using JobApplicationTracker.Api.Messaging.Events;
using JobApplicationTracker.Api.Messaging.Publishers;
using JobApplicationTracker.Api.Models.Entities;
using JobApplicationTracker.Api.Models.Request;
using JobApplicationTracker.Api.Models.Response;

namespace JobApplicationTracker.Api.Handlers
{
    public class ApplicationHandler : IApplicationHandler
    {
        private readonly IApplicationRepository _repository;
        private readonly IMessagePublisher _eventPublisher;
        private readonly RabbitMqOptions _rabbitMqOptions;


        public ApplicationHandler(IApplicationRepository repository, IMessagePublisher eventPublisher, RabbitMqOptions rabbitMqOptions)
        {
            _repository = repository;
            _eventPublisher = eventPublisher;
            _rabbitMqOptions = rabbitMqOptions;
        }

        public async Task<JobApplicationResponse> CreateAsync(
            CreateJobApplicationRequest request,
            CancellationToken cancellationToken)
        {
            ValidateCreateRequest(request);

            var utcNow = DateTime.UtcNow;

            var entity = new JobApplication
            {
                Id = Guid.NewGuid().ToString(),
                CompanyName = request.CompanyName.Trim(),
                JobTitle = request.JobTitle.Trim(),
                Location = request.Location?.Trim(),
                JobUrl = request.JobUrl?.Trim(),

                SourceType = request.SourceType.Trim(),
                SourceReference = request.SourceReference?.Trim(),

                Status = request.Status.Trim(),
                AppliedDate = request.AppliedDate,

                ContactName = request.ContactName?.Trim(),
                ContactEmail = request.ContactEmail?.Trim(),

                WorkMode = request.WorkMode?.Trim(),
                EmploymentType = request.EmploymentType?.Trim(),

                SalaryMin = request.SalaryMin,
                SalaryMax = request.SalaryMax,
                Currency = request.Currency?.Trim(),
                SalaryPeriod = request.SalaryPeriod?.Trim(),

                NextActionDate = request.NextActionDate,
                Notes = request.Notes?.Trim(),

                Interviews = new List<InterviewRound>(),

                CreatedUtc = utcNow,
                UpdatedUtc = utcNow
            };

            await _repository.InsertAsync(entity, cancellationToken);

            try
            {
                var createdEvent = new JobApplicationCreatedEvent
                {
                    ApplicationId = entity.Id,
                    CompanyName = entity.CompanyName,
                    JobTitle = entity.JobTitle,
                    AppliedOnUtc = entity.AppliedDate,
                    Status = entity.Status
                };

                await _eventPublisher.PublishAsync(_rabbitMqOptions.JobApplicationCreatedQueueName, createdEvent, cancellationToken);
            }
            catch(Exception ex)
            {
                // Log the exception (not implemented here)
                Console.WriteLine($"Failed to publish event: {ex.Message}");
            }

            return MapToResponse(entity);
        }

        public async Task<JobApplicationResponse?> GetByIdAsync(
            string id,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Application id is required.", nameof(id));
            }

            var entity = await _repository.GetByIdAsync(id, cancellationToken);

            return entity is null ? null : MapToResponse(entity);
        }

        public async Task<List<JobApplicationResponse>> GetAllAsync(
            CancellationToken cancellationToken)
        {
            var entities = await _repository.GetAllAsync(cancellationToken);

            return entities
                .Select(MapToResponse)
                .ToList();
        }

        public async Task<JobApplicationResponse?> UpdateAsync(
            string id,
            UpdateJobApplicationRequest request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Application id is required.", nameof(id));
            }

            ValidateUpdateRequest(request);

            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            if (entity is null)
            {
                return null;
            }

            var previousStatus = entity.Status;
            var previousNextActionDate = entity.NextActionDate;

            entity.JobTitle = request.JobTitle.Trim();
            entity.Location = request.Location?.Trim();
            entity.JobUrl = request.JobUrl?.Trim();

            entity.ContactName = request.ContactName?.Trim();
            entity.ContactEmail = request.ContactEmail?.Trim();

            entity.WorkMode = request.WorkMode?.Trim();
            entity.EmploymentType = request.EmploymentType?.Trim();

            entity.SalaryMin = request.SalaryMin;
            entity.SalaryMax = request.SalaryMax;
            entity.Currency = request.Currency?.Trim();
            entity.SalaryPeriod = request.SalaryPeriod?.Trim();

            entity.NextActionDate = request.NextActionDate;
            entity.Notes = request.Notes?.Trim();

            entity.UpdatedUtc = DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(entity, cancellationToken);
            if (!updated)
            {
                return null;
            }

            try
            {
                var updatedEvent = new JobApplicationUpdatedEvent
                {
                    ApplicationId = entity.Id,
                    CompanyName = entity.CompanyName,
                    JobTitle = entity.JobTitle,
                    Status = entity.Status,
                    PreviousStatus = previousStatus,
                    NextActionDate = entity.NextActionDate,
                    PreviousNextActionDate = previousNextActionDate,
                    Notes = entity.Notes,
                    UpdatedAtUtc = entity.UpdatedUtc
                };

                await _eventPublisher.PublishAsync(_rabbitMqOptions.JobApplicationUpdatedQueueName, updatedEvent, cancellationToken);
            }
            catch(Exception ex)
            {
                // Log the exception (not implemented here)
                Console.WriteLine($"Failed to publish event: {ex.Message}");
            }

            return MapToResponse(entity);
        }

        public async Task<JobApplicationResponse?> UpdateStatusAsync(
            string id,
            UpdateApplicationStatusRequest request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Application id is required.", nameof(id));
            }

            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrWhiteSpace(request.Status))
            {
                throw new ArgumentException("Status is required.", nameof(request.Status));
            }

            var entity = await _repository.GetByIdAsync(id, cancellationToken);
            if (entity is null)
            {
                return null;
            }

            entity.Status = request.Status.Trim();
            entity.NextActionDate = request.NextActionDate;

            if (!string.IsNullOrWhiteSpace(request.Notes))
            {
                entity.Notes = request.Notes.Trim();
            }

            entity.UpdatedUtc = DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(entity, cancellationToken);
            if (!updated)
            {
                return null;
            }

            return MapToResponse(entity);
        }

        public async Task<List<InterviewRoundResponse>?> GetInterviewsAsync(string applicationId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(applicationId))
            {
                throw new ArgumentException("Application id is required.", nameof(applicationId));
            }

            var entity = await _repository.GetByIdAsync(applicationId, cancellationToken);
            if (entity is null)
            {
                return null;
            }

            return entity.Interviews
                .Select(MapInterviewToResponse)
                .OrderBy(x => x.RoundNumber)
                .ToList();
        }

        public async Task<InterviewRoundResponse?> AddInterviewAsync(string applicationId, AddInterviewRoundRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(applicationId))
            {
                throw new ArgumentException("Application id is required.", nameof(applicationId));
            }

            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            ValidateAddInterviewRequest(request);

            var entity = await _repository.GetByIdAsync(applicationId, cancellationToken);
            if (entity is null)
            {
                return null;
            }

            var interview = new InterviewRound
            {
                Id = Guid.NewGuid().ToString(),
                RoundNumber = request.RoundNumber,
                InterviewType = request.InterviewType.Trim(),
                InterviewDate = request.InterviewDate,
                InterviewerName = request.InterviewerName?.Trim(),
                InterviewerEmail = request.InterviewerEmail?.Trim(),
                Outcome = request.Outcome?.Trim(),
                Notes = request.Notes?.Trim()
            };

            entity.Interviews.Add(interview);
            entity.UpdatedUtc = DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(entity, cancellationToken);
            if (!updated)
            {
                return null;
            }

            return MapInterviewToResponse(interview);
        }

        public async Task<InterviewRoundResponse?> UpdateInterviewAsync(string applicationId, string interviewId, UpdateInterviewRoundRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(applicationId))
            {
                throw new ArgumentException("Application id is required.", nameof(applicationId));
            }

            if (string.IsNullOrWhiteSpace(interviewId))
            {
                throw new ArgumentException("Interview id is required.", nameof(interviewId));
            }

            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            ValidateUpdateInterviewRequest(request);

            var entity = await _repository.GetByIdAsync(applicationId, cancellationToken);
            if (entity is null)
            {
                return null;
            }

            var interview = entity.Interviews.FirstOrDefault(x => x.Id == interviewId);
            if (interview is null)
            {
                return null;
            }

            interview.RoundNumber = request.RoundNumber;
            interview.InterviewType = request.InterviewType.Trim();
            interview.InterviewDate = request.InterviewDate;
            interview.InterviewerName = request.InterviewerName?.Trim();
            interview.InterviewerEmail = request.InterviewerEmail?.Trim();
            interview.Outcome = request.Outcome?.Trim();
            interview.Notes = request.Notes?.Trim();

            entity.UpdatedUtc = DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(entity, cancellationToken);
            if (!updated)
            {
                return null;
            }

            return MapInterviewToResponse(interview);
        }

        public async Task<bool> DeleteInterviewAsync(string applicationId, string interviewId, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(applicationId))
            {
                throw new ArgumentException("Application id is required.", nameof(applicationId));
            }

            if (string.IsNullOrWhiteSpace(interviewId))
            {
                throw new ArgumentException("Interview id is required.", nameof(interviewId));
            }

            var entity = await _repository.GetByIdAsync(applicationId, cancellationToken);
            if (entity is null)
            {
                return false;
            }

            var interview = entity.Interviews.FirstOrDefault(x => x.Id == interviewId);
            if (interview is null)
            {
                return false;
            }

            entity.Interviews.Remove(interview);
            entity.UpdatedUtc = DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(entity, cancellationToken);
            return updated;
        }

        private static void ValidateCreateRequest(CreateJobApplicationRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrWhiteSpace(request.CompanyName))
            {
                throw new ArgumentException("CompanyName is required.", nameof(request.CompanyName));
            }

            if (string.IsNullOrWhiteSpace(request.JobTitle))
            {
                throw new ArgumentException("JobTitle is required.", nameof(request.JobTitle));
            }

            if (string.IsNullOrWhiteSpace(request.SourceType))
            {
                throw new ArgumentException("SourceType is required.", nameof(request.SourceType));
            }

            if (string.IsNullOrWhiteSpace(request.Status))
            {
                throw new ArgumentException("Status is required.", nameof(request.Status));
            }
        }

        private static void ValidateUpdateRequest(UpdateJobApplicationRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrWhiteSpace(request.JobTitle))
            {
                throw new ArgumentException("JobTitle is required.", nameof(request.JobTitle));
            }
        }

        private static void ValidateAddInterviewRequest(AddInterviewRoundRequest request)
        {
            if (request.RoundNumber <= 0)
            {
                throw new ArgumentException("RoundNumber must be greater than zero.", nameof(request.RoundNumber));
            }

            if (string.IsNullOrWhiteSpace(request.InterviewType))
            {
                throw new ArgumentException("InterviewType is required.", nameof(request.InterviewType));
            }
        }

        private static void ValidateUpdateInterviewRequest(UpdateInterviewRoundRequest request)
        {
            if (request.RoundNumber <= 0)
            {
                throw new ArgumentException("RoundNumber must be greater than zero.", nameof(request.RoundNumber));
            }

            if (string.IsNullOrWhiteSpace(request.InterviewType))
            {
                throw new ArgumentException("InterviewType is required.", nameof(request.InterviewType));
            }
        }

        private static JobApplicationResponse MapToResponse(JobApplication entity)
        {
            return new JobApplicationResponse
            {
                Id = entity.Id,
                CompanyName = entity.CompanyName,
                JobTitle = entity.JobTitle,
                Location = entity.Location,
                JobUrl = entity.JobUrl,

                SourceType = entity.SourceType,
                SourceReference = entity.SourceReference,

                Status = entity.Status,
                AppliedDate = entity.AppliedDate,

                ContactName = entity.ContactName,
                ContactEmail = entity.ContactEmail,

                WorkMode = entity.WorkMode,
                EmploymentType = entity.EmploymentType,

                SalaryMin = entity.SalaryMin,
                SalaryMax = entity.SalaryMax,
                Currency = entity.Currency,
                SalaryPeriod = entity.SalaryPeriod,

                NextActionDate = entity.NextActionDate,
                Notes = entity.Notes,

                Interviews = entity.Interviews
                    .Select(MapInterviewToResponse)
                    .OrderBy(x => x.RoundNumber)
                    .ToList(),

                CreatedUtc = entity.CreatedUtc,
                UpdatedUtc = entity.UpdatedUtc
            };
        }

        private static InterviewRoundResponse MapInterviewToResponse(InterviewRound interview)
        {
            return new InterviewRoundResponse
            {
                Id = interview.Id,
                RoundNumber = interview.RoundNumber,
                InterviewType = interview.InterviewType,
                InterviewDate = interview.InterviewDate,
                InterviewerName = interview.InterviewerName,
                InterviewerEmail = interview.InterviewerEmail,
                Outcome = interview.Outcome,
                Notes = interview.Notes
            };
        }
    }
}
