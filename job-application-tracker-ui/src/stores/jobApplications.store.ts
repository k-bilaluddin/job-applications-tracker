import { ref, computed, reactive } from 'vue';
import { defineStore } from 'pinia';
import jobApplicationsApi from '@/api/jobApplications.api';
import type {
  JobApplication,
  CreateJobApplicationRequest,
  UpdateJobApplicationRequest,
  UpdateJobApplicationStatusRequest,
  JobApplicationStatus,
} from '@/types/jobApplication.types';

interface FetchApplicationsParams {
  search?: string;
  status?: JobApplicationStatus;
  sortBy?: 'appliedDate';
  sortOrder?: 'asc' | 'desc';
}

const getErrorMessage = (err: unknown, fallback: string): string => {
  if (err instanceof Error) {
    return err.message;
  }

  if (typeof err === 'object' && err !== null && 'message' in err) {
    const maybeMessage = (err as { message?: unknown }).message;
    if (typeof maybeMessage === 'string' && maybeMessage.trim().length > 0) {
      return maybeMessage;
    }
  }

  return fallback;
};

export const useJobApplicationsStore = defineStore('jobApplications', () => {
  const allApplications = ref<JobApplication[]>([]);
  const selectedApplication = ref<JobApplication | null>(null);
  const currentPage = ref(1);
  const pageSize = ref(10);
  const loading = ref(false);
  const submitting = ref(false);
  const error = ref<string | null>(null);

  const filters = reactive({
    search: '',
    status: 'all' as JobApplicationStatus | 'all',
    sortOrder: 'desc' as 'asc' | 'desc',
  });

  const filteredApplications = computed(() => {
    let filtered = allApplications.value;

    if (filters.search) {
      const search = filters.search.toLowerCase();
      filtered = filtered.filter(app =>
        app.companyName.toLowerCase().includes(search) ||
        app.jobTitle.toLowerCase().includes(search)
      );
    }

    if (filters.status !== 'all') {
      filtered = filtered.filter(app => app.status === filters.status);
    }

    filtered.sort((a, b) => {
      const dateA = new Date(a.appliedDate).getTime();
      const dateB = new Date(b.appliedDate).getTime();
      return filters.sortOrder === 'asc' ? dateA - dateB : dateB - dateA;
    });

    return filtered;
  });

  const totalItems = computed(() => filteredApplications.value.length);
  const totalPages = computed(() => Math.ceil(totalItems.value / pageSize.value));

  const applications = computed(() => {
    const start = (currentPage.value - 1) * pageSize.value;
    const end = start + pageSize.value;
    return filteredApplications.value.slice(start, end);
  });

  const fetchApplications = async (
    params?: FetchApplicationsParams
  ): Promise<JobApplication[]> => {
    console.log('Store: fetchApplications called with params:', params);
    loading.value = true;
    error.value = null;

    try {
      const response = await jobApplicationsApi.getAll(params);
      console.log('Store: API response received, length:', response.length);
      allApplications.value = response;
      currentPage.value = 1;
      console.log('Store: allApplications set, currentPage reset');
      return response;
    } catch (err: unknown) {
      console.error('Store: fetchApplications error:', err);
      error.value = getErrorMessage(err, 'Failed to fetch applications');
      throw err;
    } finally {
      console.log('Store: fetchApplications finally, setting loading to false');
      loading.value = false;
    }
  };

  const fetchApplicationById = async (id: string): Promise<JobApplication> => {
    loading.value = true;
    error.value = null;

    try {
      const application = await jobApplicationsApi.getById(id);
      selectedApplication.value = application;
      return application;
    } catch (err: unknown) {
      error.value = getErrorMessage(err, 'Failed to fetch application');
      throw err;
    } finally {
      loading.value = false;
    }
  };

  const createApplication = async (
    data: CreateJobApplicationRequest
  ): Promise<JobApplication | undefined> => {
    if (submitting.value) return;

    submitting.value = true;
    error.value = null;

    try {
      const newApplication = await jobApplicationsApi.create(data);
      allApplications.value.push(newApplication);
      return newApplication;
    } catch (err: unknown) {
      error.value = getErrorMessage(err, 'Failed to create application');
      throw err;
    } finally {
      submitting.value = false;
    }
  };

  const updateApplication = async (
    id: string,
    data: UpdateJobApplicationRequest
  ): Promise<JobApplication | undefined> => {
    if (submitting.value) return;

    submitting.value = true;
    error.value = null;

    try {
      const updatedApplication = await jobApplicationsApi.update(id, data);

      const index = allApplications.value.findIndex((app) => app.id === id);
      if (index > -1) {
        allApplications.value[index] = updatedApplication;
      }

      if (selectedApplication.value?.id === id) {
        selectedApplication.value = updatedApplication;
      }

      return updatedApplication;
    } catch (err: unknown) {
      error.value = getErrorMessage(err, 'Failed to update application');
      throw err;
    } finally {
      submitting.value = false;
    }
  };

  const updateApplicationStatus = async (
    id: string,
    data: UpdateJobApplicationStatusRequest
  ): Promise<JobApplication | undefined> => {
    if (submitting.value) return;

    submitting.value = true;
    error.value = null;

    try {
      const updatedApplication = await jobApplicationsApi.updateStatus(id, data);

      const index = allApplications.value.findIndex((app) => app.id === id);
      if (index > -1) {
        allApplications.value[index] = updatedApplication;
      }

      if (selectedApplication.value?.id === id) {
        selectedApplication.value = updatedApplication;
      }

      return updatedApplication;
    } catch (err: unknown) {
      error.value = getErrorMessage(err, 'Failed to update application status');
      throw err;
    } finally {
      submitting.value = false;
    }
  };

  const setPage = (page: number) => {
    if (page >= 1 && page <= totalPages.value) {
      currentPage.value = page;
    }
  };

  const setPageSize = (size: number) => {
    pageSize.value = size;
    currentPage.value = 1;
  };

  const clearSelectedApplication = (): void => {
    selectedApplication.value = null;
  };

  const clearError = (): void => {
    error.value = null;
  };

  return {
    allApplications,
    applications,
    selectedApplication,
    filters,
    currentPage,
    pageSize,
    totalItems,
    totalPages,
    loading,
    submitting,
    error,
    filteredApplications,
    fetchApplications,
    fetchApplicationById,
    createApplication,
    updateApplication,
    updateApplicationStatus,
    setPage,
    setPageSize,
    clearSelectedApplication,
    clearError,
  };
});