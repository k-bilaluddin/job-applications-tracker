<template>
  <form @submit.prevent="handleSubmit" class="interview-form">
    <div class="form-group">
      <label for="roundNumber">Round Number *</label>
      <input
        id="roundNumber"
        v-model.number="form.roundNumber"
        type="number"
        min="1"
        required
        :disabled="submitting"
        :class="{ 'error': errors.roundNumber }"
      />
      <span v-if="errors.roundNumber" class="error-message">{{ errors.roundNumber }}</span>
    </div>

    <div class="form-group">
      <label for="interviewType">Interview Type *</label>
      <select id="interviewType" v-model="form.interviewType" required :disabled="submitting" :class="{ 'error': errors.interviewType }">
        <option value="">Select interview type</option>
        <option v-for="type in roundTypeOptions" :key="type" :value="type">
          {{ type }}
        </option>
      </select>
      <span v-if="errors.interviewType" class="error-message">{{ errors.interviewType }}</span>
    </div>

    <div class="form-group">
      <label for="interviewDate">Interview Date *</label>
      <input
        id="interviewDate"
        v-model="form.interviewDate"
        type="datetime-local"
        required
        :disabled="submitting"
        :class="{ 'error': errors.interviewDate }"
      />
      <span v-if="errors.interviewDate" class="error-message">{{ errors.interviewDate }}</span>
    </div>

    <div class="form-group">
      <label for="interviewerName">Interviewer Name</label>
      <input
        id="interviewerName"
        v-model="form.interviewerName"
        type="text"
        :disabled="submitting"
      />
    </div>

    <div class="form-group">
      <label for="interviewerEmail">Interviewer Email</label>
      <input
        id="interviewerEmail"
        v-model="form.interviewerEmail"
        type="email"
        :disabled="submitting"
      />
    </div>

    <div class="form-group">
      <label for="outcome">Outcome</label>
      <select id="outcome" v-model="form.outcome" :disabled="submitting">
        <option value="">Select outcome</option>
        <option v-for="res in resultOptions" :key="res" :value="res">
          {{ res }}
        </option>
      </select>
    </div>

    <div class="form-group">
      <label for="notes">Notes</label>
      <textarea
        id="notes"
        v-model="form.notes"
        rows="3"
        :disabled="submitting"
      ></textarea>
    </div>

    <div class="form-actions">
      <button type="button" @click="$emit('cancel')" :disabled="submitting" class="cancel-btn">
        Cancel
      </button>
      <button type="submit" :disabled="submitting || !isValid" class="submit-btn">
        {{ submitting ? 'Saving...' : mode === 'create' ? 'Create' : 'Update' }}
      </button>
    </div>
  </form>
</template>

<script setup lang="ts">
import { ref, computed, watch } from 'vue';
import type {
  InterviewRound,
  InterviewRoundType,
  InterviewResult,
  CreateInterviewRoundRequest,
} from '@/types/interview.types';

interface Props {
  mode: 'create' | 'edit';
  initialValues?: Partial<InterviewRound>;
  submitting?: boolean;
}

const props = withDefaults(defineProps<Props>(), {
  submitting: false,
});

const emit = defineEmits<{
  submit: [data: CreateInterviewRoundRequest];
  cancel: [];
}>();

const roundTypeOptions: InterviewRoundType[] = [
  'Phone Screen',
  'Technical Interview',
  'Coding Challenge',
  'Onsite Interview',
  'Final Interview',
  'Other',
];

const resultOptions: InterviewResult[] = [
  'Pending',
  'Passed',
  'Failed',
  'No Show',
];

const form = ref({
  roundNumber: 1,
  interviewType: '' as InterviewRoundType | '',
  interviewDate: '',
  interviewerName: '',
  interviewerEmail: '',
  outcome: '' as InterviewResult | '',
  notes: '',
});

const errors = ref({
  roundNumber: '',
  interviewType: '',
  interviewDate: '',
});

const isValid = computed(() => {
  return form.value.roundNumber > 0 && form.value.interviewType && form.value.interviewDate && !errors.value.roundNumber && !errors.value.interviewType && !errors.value.interviewDate;
});

// Initialize form with initial values
watch(
  () => props.initialValues,
  (newValues) => {
    if (newValues) {
      form.value = {
        roundNumber: newValues.roundNumber || 1,
        interviewType: newValues.interviewType || ('' as InterviewRoundType | ''),
        interviewDate: newValues.interviewDate ? new Date(newValues.interviewDate).toISOString().slice(0, 16) : '',
        interviewerName: newValues.interviewerName || '',
        interviewerEmail: newValues.interviewerEmail || '',
        outcome: newValues.outcome || ('' as InterviewResult | ''),
        notes: newValues.notes || '',
      };
    } else {
      // Reset the form when switching to create mode or clearing selection.
      form.value = {
        roundNumber: 1,
        interviewType: '' as InterviewRoundType | '',
        interviewDate: '',
        interviewerName: '',
        interviewerEmail: '',
        outcome: '' as InterviewResult | '',
        notes: '',
      };
    }
  },
  { immediate: true }
);

const validate = () => {
  errors.value.roundNumber = form.value.roundNumber > 0 ? '' : 'Round number must be greater than 0';
  errors.value.interviewType = form.value.interviewType ? '' : 'Interview type is required';
  errors.value.interviewDate = form.value.interviewDate ? '' : 'Interview date and time are required';
  return !errors.value.roundNumber && !errors.value.interviewType && !errors.value.interviewDate;
};

const handleSubmit = () => {
  if (!validate() || props.submitting) return;

  const formData = {
    roundNumber: form.value.roundNumber,
    interviewType: form.value.interviewType as InterviewRoundType,
    interviewDate: form.value.interviewDate,
    interviewerName: form.value.interviewerName.trim() || undefined,
    interviewerEmail: form.value.interviewerEmail.trim() || undefined,
    outcome: form.value.outcome || undefined,
    notes: form.value.notes.trim() || undefined,
  };

  emit('submit', formData);
};
</script>

<style scoped>
.interview-form {
  max-width: 500px;
  margin: 0 auto;
}

.form-group {
  margin-bottom: 1.5rem;
}

.form-group label {
  display: block;
  margin-bottom: 0.5rem;
  font-weight: 500;
  color: #333;
}

.form-group input,
.form-group select,
.form-group textarea {
  width: 100%;
  padding: 0.75rem;
  border: 1px solid #ddd;
  border-radius: 4px;
  font-size: 1rem;
  transition: border-color 0.2s;
}

.form-group input:focus,
.form-group select:focus,
.form-group textarea:focus {
  outline: none;
  border-color: #3498db;
}

.form-group input.error,
.form-group select.error,
.form-group textarea.error {
  border-color: #e74c3c;
}

.error-message {
  color: #e74c3c;
  font-size: 0.875rem;
  margin-top: 0.25rem;
  display: block;
}

.form-group textarea {
  resize: vertical;
  min-height: 80px;
}

.form-actions {
  display: flex;
  gap: 1rem;
  justify-content: flex-end;
  margin-top: 2rem;
}

.form-actions button {
  padding: 0.75rem 1.5rem;
  border: none;
  border-radius: 4px;
  font-size: 1rem;
  cursor: pointer;
  transition: background-color 0.2s;
}

.cancel-btn {
  background-color: #f5f5f5;
  color: #333;
}

.cancel-btn:hover:not(:disabled) {
  background-color: #e0e0e0;
}

.submit-btn {
  background-color: #3498db;
  color: white;
}

.submit-btn:hover:not(:disabled) {
  background-color: #2980b9;
}

.submit-btn:disabled,
.cancel-btn:disabled {
  opacity: 0.6;
  cursor: not-allowed;
}
</style>