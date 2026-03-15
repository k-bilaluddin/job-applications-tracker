<template>
  <div class="interview-item">
    <div class="interview-header">
      <h4>{{ interview.interviewType }}</h4>
      <div class="interview-actions">
        <button type="button" @click="$emit('edit', interview.id)" class="edit-btn">Edit</button>
        <button type="button" @click="$emit('delete', interview.id)" class="delete-btn">Delete</button>
      </div>
    </div>
    <div class="interview-details">
      <p><strong>Round:</strong> {{ interview.roundNumber }}</p>
      <p><strong>Scheduled:</strong> {{ formatDateTime(interview.interviewDate) }}</p>
      <p><strong>Outcome:</strong> {{ interview.outcome || 'Pending' }}</p>
      <p v-if="interview.interviewerName"><strong>Interviewer:</strong> {{ interview.interviewerName }}</p>
      <p v-if="interview.interviewerEmail"><strong>Email:</strong> {{ interview.interviewerEmail }}</p>
      <p v-if="interview.notes"><strong>Notes:</strong> {{ interview.notes }}</p>
    </div>
  </div>
</template>

<script setup lang="ts">
import { formatDateTime } from '@/utils/date';
import type { InterviewRound } from '@/types/interview.types';

interface Props {
  interview: InterviewRound;
}

defineProps<Props>();

defineEmits<{
  edit: [id: string];
  delete: [id: string];
}>();
</script>

<style scoped>
.interview-item {
  border: 1px solid #ddd;
  border-radius: 8px;
  padding: 1rem;
  margin-bottom: 1rem;
  background: white;
}

.interview-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 0.5rem;
}

.interview-header h4 {
  margin: 0;
  color: #333;
}

.interview-actions {
  display: flex;
  gap: 0.5rem;
}

.edit-btn,
.delete-btn {
  padding: 0.4rem 0.8rem;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 0.9rem;
  transition: background-color 0.2s;
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

.interview-details p {
  margin: 0.25rem 0;
  color: #555;
}

.interview-details strong {
  color: #333;
}
</style>