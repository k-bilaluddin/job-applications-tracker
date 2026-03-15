<template>
  <div class="interview-list">
    <h3>Interview Rounds</h3>
    <div v-if="interviews.length > 0" class="interviews">
      <InterviewItem
        v-for="interview in interviews"
        :key="interview.id"
        :interview="interview"
        @edit="$emit('edit', $event)"
        @delete="$emit('delete', $event)"
      />
    </div>
    <EmptyState v-else title="No Interviews" description="No interview rounds have been scheduled yet." />
  </div>
</template>

<script setup lang="ts">
import InterviewItem from './InterviewItem.vue';
import EmptyState from '@/components/common/EmptyState.vue';
import type { InterviewRound } from '@/types/interview.types';

interface Props {
  interviews: InterviewRound[];
}

defineProps<Props>();

defineEmits<{
  edit: [id: string];
  delete: [id: string];
}>();
</script>

<style scoped>
.interview-list h3 {
  margin-bottom: 1rem;
  color: #333;
}

.interviews {
  display: flex;
  flex-direction: column;
}
</style>