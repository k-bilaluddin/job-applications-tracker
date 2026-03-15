<template>
  <Teleport to="body">
    <div v-if="show" class="confirm-dialog-overlay" @click="handleCancel">
      <div class="confirm-dialog" @click.stop>
        <h3>{{ title }}</h3>
        <p>{{ message }}</p>
        <div class="actions">
          <button type="button" @click="handleCancel" class="cancel-btn">
            {{ cancelText }}
          </button>
          <button type="button" @click="handleConfirm" :class="['confirm-btn', confirmClass]">
            {{ confirmText }}
          </button>
        </div>
      </div>
    </div>
  </Teleport>
</template>

<script setup lang="ts">
interface Props {
  show: boolean;
  title: string;
  message: string;
  confirmText?: string;
  cancelText?: string;
  confirmClass?: string;
}

withDefaults(defineProps<Props>(), {
  confirmText: 'Confirm',
  cancelText: 'Cancel',
  confirmClass: 'danger',
});

const emit = defineEmits<{
  confirm: [];
  cancel: [];
}>();

const handleConfirm = () => {
  emit('confirm');
};

const handleCancel = () => {
  emit('cancel');
};
</script>

<style scoped>
.confirm-dialog-overlay {
  position: fixed;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
}

.confirm-dialog {
  background: white;
  border-radius: 8px;
  padding: 2rem;
  max-width: 400px;
  width: 90%;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

.confirm-dialog h3 {
  margin: 0 0 1rem 0;
  font-size: 1.25rem;
  color: #333;
}

.confirm-dialog p {
  margin: 0 0 1.5rem 0;
  color: #666;
  line-height: 1.5;
}

.actions {
  display: flex;
  gap: 1rem;
  justify-content: flex-end;
}

.actions button {
  padding: 0.5rem 1rem;
  border: none;
  border-radius: 4px;
  cursor: pointer;
  font-size: 0.9rem;
  transition: background-color 0.2s;
}

.cancel-btn {
  background-color: #f5f5f5;
  color: #333;
}

.cancel-btn:hover {
  background-color: #e0e0e0;
}

.confirm-btn.danger {
  background-color: #d32f2f;
  color: white;
}

.confirm-btn.danger:hover {
  background-color: #b71c1c;
}

.confirm-btn.primary {
  background-color: #1976d2;
  color: white;
}

.confirm-btn.primary:hover {
  background-color: #1565c0;
}
</style>