import { ref } from 'vue';
import { defineStore } from 'pinia';
import interviewsApi from '@/api/interviews.api';
import type {
  InterviewRound,
  CreateInterviewRoundRequest,
  UpdateInterviewRoundRequest,
} from '@/types/interview.types';
import type { ApiErrorResponse } from '@/types/api.types';

export const useInterviewsStore = defineStore('interviews', () => {
  // State
  const interviews = ref<InterviewRound[]>([]);
  const loading = ref(false);
  const submitting = ref(false);
  const error = ref<string | null>(null);

  // Actions
  const fetchInterviews = async (jobApplicationId: string) => {
    loading.value = true;
    error.value = null;
    try {
      const data = await interviewsApi.getByApplicationId(jobApplicationId);
      interviews.value = data;
    } catch (err) {
      const errorObj = err as ApiErrorResponse;
      error.value = errorObj.message;
    } finally {
      loading.value = false;
    }
  };

  const createInterview = async (jobApplicationId: string, data: CreateInterviewRoundRequest) => {
    if (submitting.value) return;
    submitting.value = true;
    error.value = null;
    try {
      const newInterview = await interviewsApi.create(jobApplicationId, data);
      interviews.value.push(newInterview);
      return newInterview;
    } catch (err) {
      const errorObj = err as ApiErrorResponse;
      error.value = errorObj.message;
      throw err;
    } finally {
      submitting.value = false;
    }
  };

  const updateInterview = async (jobApplicationId: string, interviewId: string, data: UpdateInterviewRoundRequest) => {
    if (submitting.value) return;
    submitting.value = true;
    error.value = null;
    try {
      const updatedInterview = await interviewsApi.update(jobApplicationId, interviewId, data);
      const index = interviews.value.findIndex(interview => interview.id === interviewId);
      if (index > -1) {
        interviews.value[index] = updatedInterview;
      }
      return updatedInterview;
    } catch (err) {
      const errorObj = err as ApiErrorResponse;
      error.value = errorObj.message;
      throw err;
    } finally {
      submitting.value = false;
    }
  };

  const deleteInterview = async (jobApplicationId: string, interviewId: string) => {
    if (submitting.value) return;
    submitting.value = true;
    error.value = null;
    try {
      await interviewsApi.remove(jobApplicationId, interviewId);
      interviews.value = interviews.value.filter(interview => interview.id !== interviewId);
    } catch (err) {
      const errorObj = err as ApiErrorResponse;
      error.value = errorObj.message;
      throw err;
    } finally {
      submitting.value = false;
    }
  };

  const clearError = () => {
    error.value = null;
  };

  return {
    // State
    interviews,
    loading,
    submitting,
    error,
    // Actions
    fetchInterviews,
    createInterview,
    updateInterview,
    deleteInterview,
    clearError,
  };
});