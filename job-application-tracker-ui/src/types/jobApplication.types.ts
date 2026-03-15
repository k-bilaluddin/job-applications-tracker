export type JobApplicationStatus =
  | 'Applied'
  | 'In Review'
  | 'Interview Scheduled'
  | 'Interviewing'
  | 'Offer Received'
  | 'Rejected'
  | 'Accepted'
  | 'Withdrawn';

import type { InterviewRound } from './interview.types';

export interface JobApplication {
  id: string;
  companyName: string;
  jobTitle: string;
  location?: string;
  jobUrl?: string;
  sourceType?: string;
  sourceReference?: string;
  status: JobApplicationStatus;
  appliedDate: string;
  contactName?: string;
  contactEmail?: string;
  workMode?: string;
  employmentType?: string;
  salaryMin?: number;
  salaryMax?: number;
  currency?: string;
  salaryPeriod?: string;
  nextActionDate?: string;
  notes?: string;
  interviews?: InterviewRound[]; // Assuming array of interviews
  createdUtc?: string;
  updatedUtc?: string;
}

export interface CreateJobApplicationRequest {
  companyName: string;
  jobTitle: string;
  location?: string;
  jobUrl?: string;
  sourceType?: string;
  sourceReference?: string;
  status?: JobApplicationStatus;
  appliedDate?: string;
  contactName?: string;
  contactEmail?: string;
  workMode?: string;
  employmentType?: string;
  salaryMin?: number;
  salaryMax?: number;
  currency?: string;
  salaryPeriod?: string;
  nextActionDate?: string;
  notes?: string;
}

export interface UpdateJobApplicationRequest {
  companyName?: string;
  jobTitle?: string;
  appliedDate?: string;
  notes?: string;
}

export interface UpdateJobApplicationStatusRequest {
  status: JobApplicationStatus;
}