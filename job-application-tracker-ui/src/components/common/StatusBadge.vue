<template>
  <span :class="['status-badge', statusClass]">
    {{ label }}
  </span>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import { getStatusLabel } from '@/utils/status';
import type { JobApplicationStatus } from '@/types/jobApplication.types';

interface Props {
  status: JobApplicationStatus;
}

const props = defineProps<Props>();

const label = computed(() => getStatusLabel(props.status));

const statusClass = computed(() => {
  const classes: Record<JobApplicationStatus, string> = {
    'Applied': 'applied',
    'In Review': 'in-review',
    'Interview Scheduled': 'interview-scheduled',
    'Interviewing': 'interviewing',
    'Offer Received': 'offer-received',
    'Rejected': 'rejected',
    'Accepted': 'accepted',
    'Withdrawn': 'withdrawn',
  };
  return classes[props.status];
});
</script>

<style scoped>
.status-badge {
  display: inline-block;
  padding: 0.25rem 0.5rem;
  border-radius: 12px;
  font-size: 0.75rem;
  font-weight: 500;
  text-transform: uppercase;
  letter-spacing: 0.05em;
}

.status-badge.applied {
  background-color: #e3f2fd;
  color: #1976d2;
}

.status-badge.in-review {
  background-color: #fff3e0;
  color: #f57c00;
}

.status-badge.interview-scheduled {
  background-color: #f3e5f5;
  color: #7b1fa2;
}

.status-badge.interviewing {
  background-color: #e8f5e8;
  color: #388e3c;
}

.status-badge.offer-received {
  background-color: #fff8e1;
  color: #fbc02d;
}

.status-badge.rejected {
  background-color: #ffebee;
  color: #d32f2f;
}

.status-badge.accepted {
  background-color: #e8f5e8;
  color: #2e7d32;
}

.status-badge.withdrawn {
  background-color: #f5f5f5;
  color: #616161;
}
</style>