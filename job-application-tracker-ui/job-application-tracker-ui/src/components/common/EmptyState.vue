<template>
  <div class="empty-state" role="status" aria-live="polite">
    <div class="empty-icon" aria-hidden="true">{{ icon }}</div>
    <h3>{{ title }}</h3>
    <p>{{ description }}</p>
    <slot></slot>
  </div>
</template>

<script setup lang="ts">
interface Props {
  title: string;
  description: string;
  icon?: string;
}

withDefaults(defineProps<Props>(), {
  icon: '📭',
});
</script>

<style scoped>
.empty-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  text-align: center;
  padding: 3rem 2rem;
  color: #666;
}

.empty-icon {
  font-size: 3rem;
  margin-bottom: 1rem;
  opacity: 0.7;
}

.empty-state h3 {
  margin: 0 0 0.5rem 0;
  font-size: 1.5rem;
  color: #333;
}

.empty-state p {
  margin: 0 0 1rem 0;
  max-width: 400px;
  line-height: 1.5;
}
</style>