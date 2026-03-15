<template>
  <div class="job-application-filters">
    <div class="filter-group">
      <label for="search">Search</label>
      <input
        id="search"
        v-model="localFilters.search"
        type="text"
        placeholder="Search by company or job title..."
        @input="emitFilters"
      />
    </div>

    <div class="filter-group">
      <label for="status">Status</label>
      <select id="status" v-model="localFilters.status" @change="emitFilters">
        <option value="all">All Statuses</option>
        <option v-for="status in statusOptions" :key="status" :value="status">
          {{ getStatusLabel(status) }}
        </option>
      </select>
    </div>

    <div class="filter-group">
      <label for="sortOrder">Sort by Applied Date</label>
      <select id="sortOrder" v-model="localFilters.sortOrder" @change="emitFilters">
        <option value="desc">Newest First</option>
        <option value="asc">Oldest First</option>
      </select>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, watch } from 'vue';
import { getStatusLabel } from '@/utils/status';
import type { JobApplicationStatus } from '@/types/jobApplication.types';

interface Props {
  search?: string;
  status?: JobApplicationStatus | 'all';
  sortOrder?: 'asc' | 'desc';
}

const props = withDefaults(defineProps<Props>(), {
  search: '',
  status: 'all',
  sortOrder: 'desc',
});

const emit = defineEmits<{
  'update:search': [value: string];
  'update:status': [value: JobApplicationStatus | 'all'];
  'update:sortOrder': [value: 'asc' | 'desc'];
}>();

const statusOptions: JobApplicationStatus[] = [
  'Applied',
  'In Review',
  'Interview Scheduled',
  'Interviewing',
  'Offer Received',
  'Rejected',
  'Accepted',
  'Withdrawn',
];

const localFilters = ref({
  search: props.search,
  status: props.status,
  sortOrder: props.sortOrder,
});

// Sync props to local
watch(props, (newProps) => {
  localFilters.value = {
    search: newProps.search || '',
    status: newProps.status || 'all',
    sortOrder: newProps.sortOrder || 'desc',
  };
});

const emitFilters = () => {
  emit('update:search', localFilters.value.search);
  emit('update:status', localFilters.value.status);
  emit('update:sortOrder', localFilters.value.sortOrder);
};
</script>

<style scoped>
.job-application-filters {
  display: flex;
  gap: 1rem;
  align-items: end;
  margin-bottom: 2rem;
  flex-wrap: wrap;
}

.filter-group {
  display: flex;
  flex-direction: column;
  min-width: 200px;
}

.filter-group label {
  margin-bottom: 0.5rem;
  font-weight: 500;
  color: #333;
}

.filter-group input,
.filter-group select {
  padding: 0.75rem;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 1rem;
  transition: border-color 0.2s;
}

.filter-group input:focus,
.filter-group select:focus {
  outline: none;
  border-color: #3498db;
}

@media (max-width: 768px) {
  .job-application-filters {
    flex-direction: column;
    align-items: stretch;
  }

  .filter-group {
    min-width: auto;
  }
}
</style>