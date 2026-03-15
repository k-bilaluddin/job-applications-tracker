import apiClient from './client';
import type {
  JobApplication,
  CreateJobApplicationRequest,
  UpdateJobApplicationRequest,
  UpdateJobApplicationStatusRequest,
  JobApplicationStatus,
} from '@/types/jobApplication.types';

interface GetJobApplicationsParams {
  search?: string;
  status?: JobApplicationStatus;
  sortBy?: 'appliedDate';
  sortOrder?: 'asc' | 'desc';
}

const jobApplicationsApi = {
  async getAll(params?: GetJobApplicationsParams): Promise<JobApplication[]> {
    const response = await apiClient.get('/api/job-applications', { params });
    return response.data;
  },

  async getById(id: string): Promise<JobApplication> {
    const response = await apiClient.get(`/api/job-applications/${id}`);
    return response.data;
  },

  async create(data: CreateJobApplicationRequest): Promise<JobApplication> {
    const response = await apiClient.post('/api/job-applications', data);
    return response.data;
  },

  async update(id: string, data: UpdateJobApplicationRequest): Promise<JobApplication> {
    const response = await apiClient.put(`/api/job-applications/${id}`, data);
    return response.data;
  },

  async updateStatus(id: string, data: UpdateJobApplicationStatusRequest): Promise<JobApplication> {
    const response = await apiClient.patch(`/api/job-applications/${id}/status`, data);
    return response.data;
  },
};

export default jobApplicationsApi;