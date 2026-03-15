<template>
  <div class="job-application-create">
    <div class="header">
      <h1>Create Job Application</h1>
      <router-link to="/applications" class="back-link">← Back to Applications</router-link>
    </div>

    <JobApplicationForm
      mode="create"
      @submit="handleSubmit"
      @cancel="handleCancel"
    />

    <AppError v-if="error" :message="error" />
  </div>
</template>

<script setup lang="ts">
import { useRouter } from 'vue-router';
import { useJobApplicationsStore } from '@/stores/jobApplications.store';
import JobApplicationForm from '@/components/job-applications/JobApplicationForm.vue';
import AppError from '@/components/common/AppError.vue';
import type { CreateJobApplicationRequest } from '@/types/jobApplication.types';

const store = useJobApplicationsStore();
const router = useRouter();

// Computed from store
const { error } = store;

const handleSubmit = async (data: CreateJobApplicationRequest) => {
  try {
    const newApplication = await store.createApplication(data);
    if (newApplication) {
      router.push(`/applications/${newApplication.id}`);
    }
  } catch {
    // Error is handled in store
  }
};

const handleCancel = () => {
  router.push('/applications');
};
</script>

<style scoped>
.job-application-create {
  max-width: 800px;
  margin: 0 auto;
  padding: 2rem;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 2rem;
}

.header h1 {
  margin: 0;
  color: #333;
}

.back-link {
  color: #3498db;
  text-decoration: none;
}

.back-link:hover {
  text-decoration: underline;
}
</style>