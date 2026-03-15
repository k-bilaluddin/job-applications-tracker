<template>
  <div class="status-updater">
    <label for="status-select">Update Status</label>
    <select
      id="status-select"
      :value="status"
      :disabled="loading"
      @change="handleStatusChange"
    >
      <option v-for="statusOption in statusOptions" :key="statusOption" :value="statusOption">
        {{ getStatusLabel(statusOption) }}
      </option>
    </select>
  </div>
</template>

<script setup lang="ts">
import { getStatusLabel } from '@/utils/status';
import type { JobApplicationStatus } from '@/types/jobApplication.types';

interface Props {
  status: JobApplicationStatus;
  loading?: boolean;
}

defineProps<Props>();

const emit = defineEmits<{
  'update:status': [status: JobApplicationStatus];
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

const handleStatusChange = (event: Event) => {
  const target = event.target as HTMLSelectElement;
  const newStatus = target.value as JobApplicationStatus;
  emit('update:status', newStatus);
};
</script>

<style scoped>
.status-updater {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.status-updater label {
  font-weight: 500;
  color: #333;
}

.status-updater select {
  padding: 0.5rem;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 1rem;
  background: white;
  transition: border-color 0.2s;
}

.status-updater select:focus {
  outline: none;
  border-color: #3498db;
}

.status-updater select:disabled {
  background-color: #f5f5f5;
  cursor: not-allowed;
}
</style>