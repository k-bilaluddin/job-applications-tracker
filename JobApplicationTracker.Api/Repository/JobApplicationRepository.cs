using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JobApplicationTracker.Api.Errors;
using JobApplicationTracker.Api.Interfaces;
using JobApplicationTracker.Api.Models.Entities;
using JobApplicationTracker.Api.Models.Persistence;
using JobApplicationTracker.Api.Models.Persistence.Documents;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace JobApplicationTracker.Api.Repository
{
    public class JobApplicationRepository : IApplicationRepository
    {
        private readonly IMongoCollection<JobApplicationDocument> _collection;

        public JobApplicationRepository(IOptions<MongoDbOptions> options)
        {
            try
            {
                var mongoOptions = options.Value;

                var client = new MongoClient(mongoOptions.ConnectionString);
                var database = client.GetDatabase(mongoOptions.DatabaseName);

                _collection = database.GetCollection<JobApplicationDocument>(
                    mongoOptions.JobApplicationsCollectionName);
            }
            catch (Exception ex)
            {
                throw new ApiException(ErrorCache.RepositoryError, ex);
            }
        }

        public async Task InsertAsync(JobApplication application, CancellationToken cancellationToken)
        {
            try
            {
                var document = MapToDocument(application);

                await _collection.InsertOneAsync(document, cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                throw new ApiException(ErrorCache.RepositoryError, ex);
            }
        }

        public async Task<JobApplication?> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            try
            {
                var document = await _collection
                    .Find(x => x.Id == id)
                    .FirstOrDefaultAsync(cancellationToken);

                if(document is null)
                    throw new Exception($"Job application with ID '{id}' not found.");
                else
                    return MapToDomain(document);
            }
            catch (Exception ex)
            {
                throw new ApiException(ErrorCache.RepositoryError, ex);
            }
        }

        public async Task<List<JobApplication>> GetAllAsync(CancellationToken cancellationToken)
        {
            try
            {
                var documents = await _collection
                    .Find(_ => true)
                    .SortByDescending(x => x.CreatedUtc)
                    .ToListAsync(cancellationToken);

                return documents.Select(MapToDomain).ToList();
            }
            catch (Exception ex)
            {
                throw new ApiException(ErrorCache.RepositoryError, ex);
            }
        }

        public async Task<bool> UpdateAsync(JobApplication application, CancellationToken cancellationToken)
        {
            try
            {
                var document = MapToDocument(application);

                var result = await _collection.ReplaceOneAsync(
                    x => x.Id == application.Id,
                    document,
                    cancellationToken: cancellationToken);

                return result.ModifiedCount > 0 || result.MatchedCount > 0;
            }
            catch (Exception ex)
            {
                throw new ApiException(ErrorCache.RepositoryError, ex);
            }
        }

        private static JobApplicationDocument MapToDocument(JobApplication entity)
        {
            return new JobApplicationDocument
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

                Interviews = entity.Interviews?.Select(i => new InterviewRoundDocument
                {
                    Id = i.Id,
                    RoundNumber = i.RoundNumber,
                    InterviewType = i.InterviewType,
                    InterviewDate = i.InterviewDate,
                    InterviewerName = i.InterviewerName,
                    InterviewerEmail = i.InterviewerEmail,
                    Outcome = i.Outcome,
                    Notes = i.Notes
                }).ToList() ?? new List<InterviewRoundDocument>(),

                CreatedUtc = entity.CreatedUtc,
                UpdatedUtc = entity.UpdatedUtc
            };
        }

        private static JobApplication MapToDomain(JobApplicationDocument document)
        {
            return new JobApplication
            {
                Id = document.Id,
                CompanyName = document.CompanyName,
                JobTitle = document.JobTitle,
                Location = document.Location,
                JobUrl = document.JobUrl,

                SourceType = document.SourceType,
                SourceReference = document.SourceReference,

                Status = document.Status,
                AppliedDate = document.AppliedDate,

                ContactName = document.ContactName,
                ContactEmail = document.ContactEmail,

                WorkMode = document.WorkMode,
                EmploymentType = document.EmploymentType,

                SalaryMin = document.SalaryMin,
                SalaryMax = document.SalaryMax,
                Currency = document.Currency,
                SalaryPeriod = document.SalaryPeriod,

                NextActionDate = document.NextActionDate,
                Notes = document.Notes,

                Interviews = document.Interviews?.Select(d => new InterviewRound
                {
                    Id = d.Id,
                    RoundNumber = d.RoundNumber,
                    InterviewType = d.InterviewType,
                    InterviewDate = d.InterviewDate,
                    InterviewerName = d.InterviewerName,
                    InterviewerEmail = d.InterviewerEmail,
                    Outcome = d.Outcome,
                    Notes = d.Notes
                }).ToList() ?? new List<InterviewRound>(),

                CreatedUtc = document.CreatedUtc,
                UpdatedUtc = document.UpdatedUtc
            };
        }
    }
}
