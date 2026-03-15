import apiClient from './client';
import type {
  InterviewRound,
  CreateInterviewRoundRequest,
  UpdateInterviewRoundRequest,
} from '@/types/interview.types';

const interviewsApi = {
  async getByApplicationId(jobApplicationId: string): Promise<InterviewRound[]> {
    const response = await apiClient.get(`/api/job-applications/${jobApplicationId}/interviews`);
    return response.data;
  },

  async create(jobApplicationId: string, data: CreateInterviewRoundRequest): Promise<InterviewRound> {
    const response = await apiClient.post(`/api/job-applications/${jobApplicationId}/interviews`, data);
    return response.data;
  },

  async update(jobApplicationId: string, interviewId: string, data: UpdateInterviewRoundRequest): Promise<InterviewRound> {
    const response = await apiClient.put(`/api/job-applications/${jobApplicationId}/interviews/${interviewId}`, data);
    return response.data;
  },

  async remove(jobApplicationId: string, interviewId: string): Promise<void> {
    await apiClient.delete(`/api/job-applications/${jobApplicationId}/interviews/${interviewId}`);
  },
};

export default interviewsApi;