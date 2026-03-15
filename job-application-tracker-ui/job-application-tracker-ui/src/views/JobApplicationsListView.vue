<template>
  <div class="job-applications-list">
    <AppLoader v-if="loading" message="Loading applications..." />
    <AppError v-else-if="error" :message="error" @retry="fetchData" />
    <div v-else class="content">
      <div class="header">
        <h1>Job Applications</h1>
        <router-link to="/applications/create" class="create-btn">Add Application</router-link>
      </div>

      <JobApplicationFilters
        v-model:search="filters.search"
        v-model:status="filters.status"
        v-model:sortOrder="filters.sortOrder"
      />

      <JobApplicationTable
        v-if="applications.length > 0"
        :applications="applications"
        @view="handleView"
        @edit="handleEdit"
        @delete="handleDelete"
      />

      <div v-if="totalPages > 1" class="pagination">
        <button
          :disabled="currentPage <= 1"
          @click="currentPage--"
          class="pagination-btn"
        >
          Previous
        </button>

        <span class="pagination-info">
          Page {{ currentPage }} of {{ totalPages }} ({{ totalItems }} total)
        </span>

        <button
          :disabled="currentPage >= totalPages"
          @click="currentPage++"
          class="pagination-btn"
        >
          Next
        </button>

        <select v-model="pageSize" @change="currentPage = 1" class="page-size-select">
          <option :value="5">5 per page</option>
          <option :value="10">10 per page</option>
          <option :value="20">20 per page</option>
        </select>
      </div>
      <EmptyState
        v-else
        title="No Applications Found"
        description="Try adjusting your filters or add a new job application."
      >
        <router-link to="/applications/create" class="create-link">Add Your First Application</router-link>
      </EmptyState>
    </div>
  </div>
</template>

<script setup lang="ts">
import { watch, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { storeToRefs } from 'pinia';
import { useJobApplicationsStore } from '@/stores/jobApplications.store';
import JobApplicationFilters from '@/components/job-applications/JobApplicationFilters.vue';
import JobApplicationTable from '@/components/job-applications/JobApplicationTable.vue';
import AppLoader from '@/components/common/AppLoader.vue';
import AppError from '@/components/common/AppError.vue';
import EmptyState from '@/components/common/EmptyState.vue';

const store = useJobApplicationsStore();
const router = useRouter();

// Computed from store - use storeToRefs for reactivity
const { applications, filters, currentPage, pageSize, totalItems, totalPages, loading, error } = storeToRefs(store);

const fetchData = () => {
  console.log('Fetching all applications');
  store.fetchApplications(); // No params, fetch all
};

watch(filters, fetchData, { deep: true });

onMounted(() => {
  console.log('JobApplicationsListView mounted');
  fetchData();
});

const handleView = (id: string) => {
  router.push(`/applications/${id}`);
};

const handleEdit = (id: string) => {
  router.push(`/applications/${id}/edit`);
};

const handleDelete = (id: string) => {
  // TODO: Implement delete functionality
  // For now, show placeholder
  alert(`Delete application ${id} - connect to store.deleteApplication when implemented`);
};
</script>

<style scoped>
.job-applications-list {
  padding: 1rem;
  max-width: 1200px;
  margin: 0 auto;
}

.content {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  flex-wrap: wrap;
  gap: 1rem;
}

.header h1 {
  margin: 0;
  color: #333;
  font-size: 1.5rem;
}

.create-btn {
  padding: 0.75rem 1.5rem;
  background-color: #3498db;
  color: white;
  text-decoration: none;
  border-radius: 4px;
  transition: background-color 0.2s;
  white-space: nowrap;
}

.create-btn:hover {
  background-color: #2980b9;
}

.create-link {
  display: inline-block;
  margin-top: 1rem;
  padding: 0.75rem 1.5rem;
  background-color: #3498db;
  color: white;
  text-decoration: none;
  border-radius: 4px;
  transition: background-color 0.2s;
  white-space: nowrap;
}

.create-link:hover {
  background-color: #2980b9;
}

/* Tablet styles */
@media (min-width: 768px) {
  .job-applications-list {
    padding: 1.5rem;
  }

  .content {
    gap: 2rem;
  }

  .header h1 {
    font-size: 2rem;
  }

  .create-btn,
  .create-link {
    padding: 0.75rem 2rem;
  }
}

/* Desktop styles */
@media (min-width: 1024px) {
  .job-applications-list {
    padding: 2rem;
  }

  .header {
    flex-wrap: nowrap;
  }

  .header h1 {
    font-size: 2.5rem;
  }
}

/* Mobile adjustments */
@media (max-width: 767px) {
  .create-btn,
  .create-link {
    width: 100%;
    text-align: center;
  }

  .pagination {
    flex-direction: column;
    gap: 0.75rem;
    align-items: center;
  }

  .pagination-info {
    order: -1;
  }

  .page-size-select {
    width: 100%;
    max-width: 200px;
  }
}

.pagination {
  display: flex;
  justify-content: center;
  align-items: center;
  gap: 1rem;
  margin-top: 1.5rem;
  padding: 1rem;
  background: white;
  border-radius: 8px;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
}

.pagination-btn {
  padding: 0.5rem 1rem;
  border: 1px solid #ddd;
  background: white;
  border-radius: 4px;
  cursor: pointer;
  transition: background-color 0.2s;
}

.pagination-btn:hover:not(:disabled) {
  background-color: #f5f5f5;
}

.pagination-btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

.pagination-info {
  font-size: 0.9rem;
  color: #666;
}

.page-size-select {
  padding: 0.5rem;
  border: 1px solid #ddd;
  border-radius: 4px;
  background: white;
}
</style>