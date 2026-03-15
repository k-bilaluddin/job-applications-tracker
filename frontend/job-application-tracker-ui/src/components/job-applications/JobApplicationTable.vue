<template>
  <div class="job-application-table">
    <table v-if="applications.length > 0">
      <thead>
        <tr>
          <th>Company</th>
          <th>Job Title</th>
          <th>Status</th>
          <th>Applied Date</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="application in applications" :key="application.id">
          <td>{{ application.companyName }}</td>
          <td>{{ application.jobTitle }}</td>
          <td>
            <StatusBadge :status="application.status" />
          </td>
          <td>{{ formatDate(application.appliedDate) }}</td>
          <td>
            <button @click="$emit('view', application.id)" class="action-btn view-btn">
              View
            </button>
            <button @click="$emit('edit', application.id)" class="action-btn edit-btn">
              Edit
            </button>
            <button @click="$emit('delete', application.id)" class="action-btn delete-btn">
              Delete
            </button>
          </td>
        </tr>
      </tbody>
    </table>
    <p v-else class="no-data">No job applications found.</p>
  </div>
</template>

<script setup lang="ts">
import StatusBadge from '@/components/common/StatusBadge.vue';
import { formatDate } from '@/utils/date';
import type { JobApplication } from '@/types/jobApplication.types';

interface Props {
  applications: JobApplication[];
}

defineProps<Props>();

defineEmits<{
  view: [id: string];
  edit: [id: string];
  delete: [id: string];
}>();
</script>

<style scoped>
.job-application-table {
  width: 100%;
  overflow-x: auto;
}

table {
  width: 100%;
  border-collapse: collapse;
  background: white;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
  border-radius: 8px;
  overflow: hidden;
}

thead {
  background-color: #f5f5f5;
}

th,
td {
  padding: 1rem;
  text-align: left;
  border-bottom: 1px solid #eee;
}

th {
  font-weight: 600;
  color: #1a1a1a;
}

td {
  color: #333;
}

tbody tr:hover {
  background-color: #f9f9f9;
}

.action-btn {
  padding: 0.5rem 1rem;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 0.9rem;
  margin-right: 0.5rem;
  transition: background-color 0.2s;
}

.view-btn {
  background-color: #3498db;
  color: white;
}

.view-btn:hover {
  background-color: #2980b9;
}

.edit-btn {
  background-color: #f39c12;
  color: white;
}

.edit-btn:hover {
  background-color: #e67e22;
}

.delete-btn {
  background-color: #e74c3c;
  color: white;
}

.delete-btn:hover {
  background-color: #c0392b;
}

.no-data {
  text-align: center;
  padding: 2rem;
  color: #666;
  font-style: italic;
}

@media (max-width: 768px) {
  th,
  td {
    padding: 0.5rem;
    font-size: 0.9rem;
  }

  .action-btn {
    padding: 0.4rem 0.8rem;
    font-size: 0.8rem;
  }
}
</style>