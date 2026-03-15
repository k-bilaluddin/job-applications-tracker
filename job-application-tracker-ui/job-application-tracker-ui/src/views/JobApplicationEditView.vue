<template>
  <div class="job-application-edit">
    <AppLoader v-if="loading" message="Loading application..." />
    <AppError v-else-if="error" :message="error" @retry="fetchData" />
    <template v-else-if="selectedApplication">
      <div class="header">
        <h1>Edit Job Application</h1>
        <router-link :to="`/applications/${$route.params.id}`" class="back-link">← Back to Details</router-link>
      </div>

      <JobApplicationForm
        mode="edit"
        :initialValues="selectedApplication"
        @submit="handleSubmit"
        @cancel="handleCancel"
      />

      <AppError v-if="error" :message="error" />
    </template>
    <AppError v-else message="Application not found" />
  </div>
</template>

<script setup lang="ts">
import { onMounted, computed } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { useJobApplicationsStore } from '@/stores/jobApplications.store';
import JobApplicationForm from '@/components/job-applications/JobApplicationForm.vue';
import AppLoader from '@/components/common/AppLoader.vue';
import AppError from '@/components/common/AppError.vue';
import type { UpdateJobApplicationRequest } from '@/types/jobApplication.types';

const route = useRoute();
const router = useRouter();
const store = useJobApplicationsStore();

const applicationId = computed(() => route.params.id as string);

// Computed from store
const { selectedApplication, loading, error } = store;

const fetchData = () => {
  if (applicationId.value) {
    store.fetchApplicationById(applicationId.value);
  }
};

onMounted(fetchData);

const handleSubmit = async (data: UpdateJobApplicationRequest) => {
  try {
    await store.updateApplication(applicationId.value, data);
    router.push(`/applications/${applicationId.value}`);
  } catch {
    // Error is handled in store
  }
};

const handleCancel = () => {
  router.push(`/applications/${applicationId.value}`);
};
</script>

<style scoped>
.job-application-edit {
  max-width: 800px;
  margin: 0 auto;
  padding: 2rem;
}

.content {
  display: flex;
  flex-direction: column;
  gap: 2rem;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
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