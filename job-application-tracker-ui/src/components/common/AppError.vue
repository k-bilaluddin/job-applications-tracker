<template>
  <div class="app-error" role="alert" aria-live="assertive">
    <div class="error-icon" aria-hidden="true">⚠️</div>
    <h3>{{ title }}</h3>
    <p>{{ message }}</p>
    <button v-if="retry" @click="$emit('retry')" class="retry-btn" aria-label="Retry the failed action">
      Try Again
    </button>
  </div>
</template>

<script setup lang="ts">
interface Props {
  title?: string;
  message: string;
  retry?: boolean;
}

defineProps<Props>();

defineEmits<{
  retry: [];
}>();
</script>

<style scoped>
.app-error {
  display: flex;
  flex-direction: column;
  align-items: center;
  text-align: center;
  padding: 2rem;
  background-color: #fee;
  border: 1px solid #fcc;
  border-radius: 8px;
  color: #c33;
}

.error-icon {
  font-size: 2rem;
  margin-bottom: 1rem;
}

.app-error h3 {
  margin: 0 0 0.5rem 0;
  font-size: 1.2rem;
}

.app-error p {
  margin: 0 0 1rem 0;
  color: #a33;
}

.retry-btn {
  padding: 0.5rem 1rem;
  background-color: #c33;
  color: white;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

.retry-btn:hover {
  background-color: #a22;
}
</style>