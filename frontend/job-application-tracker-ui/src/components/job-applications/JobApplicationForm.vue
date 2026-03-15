<template>
  <form @submit.prevent="handleSubmit" class="job-application-form">
    <section class="form-section">
      <h3 class="section-title">Basic Information</h3>
      <div class="form-grid">
        <div class="form-group">
          <label for="companyName">Company Name *</label>
          <input
            id="companyName"
            v-model="form.companyName"
            type="text"
            required
            :disabled="submitting"
            :class="{ 'error': errors.companyName }"
          />
          <span v-if="errors.companyName" class="error-message">{{ errors.companyName }}</span>
        </div>

        <div class="form-group">
          <label for="jobTitle">Job Title *</label>
          <input
            id="jobTitle"
            v-model="form.jobTitle"
            type="text"
            required
            :disabled="submitting"
            :class="{ 'error': errors.jobTitle }"
          />
          <span v-if="errors.jobTitle" class="error-message">{{ errors.jobTitle }}</span>
        </div>

        <div class="form-group">
          <label for="status">Status *</label>
          <select id="status" v-model="form.status" required :disabled="submitting">
            <option v-for="status in statusOptions" :key="status" :value="status">
              {{ getStatusLabel(status) }}
            </option>
          </select>
        </div>

        <div class="form-group">
          <label for="appliedDate">Applied Date *</label>
          <input
            id="appliedDate"
            v-model="form.appliedDate"
            type="datetime-local"
            required
            :disabled="submitting"
            :class="{ 'error': errors.appliedDate }"
          />
          <span v-if="errors.appliedDate" class="error-message">{{ errors.appliedDate }}</span>
        </div>
      </div>
    </section>

    <section class="form-section">
      <h3 class="section-title">Job Details</h3>
      <div class="form-grid">
        <div class="form-group">
          <label for="location">Location</label>
          <input
            id="location"
            v-model="form.location"
            type="text"
            :disabled="submitting"
          />
        </div>

        <div class="form-group">
          <label for="jobUrl">Job URL</label>
          <input
            id="jobUrl"
            v-model="form.jobUrl"
            type="url"
            :disabled="submitting"
          />
        </div>

        <div class="form-group">
          <label for="sourceType">Source Type</label>
          <select id="sourceType" v-model="form.sourceType" :disabled="submitting">
            <option value="">Select source type</option>
            <option value="CompanyWebsite">Company Website</option>
            <option value="JobBoard">Job Board</option>
            <option value="Referral">Referral</option>
            <option value="Recruiter">Recruiter</option>
            <option value="Other">Other</option>
          </select>
        </div>

        <div class="form-group">
          <label for="sourceReference">Source Reference</label>
          <input
            id="sourceReference"
            v-model="form.sourceReference"
            type="text"
            :disabled="submitting"
          />
        </div>

        <div class="form-group">
          <label for="workMode">Work Mode</label>
          <select id="workMode" v-model="form.workMode" :disabled="submitting">
            <option value="">Select work mode</option>
            <option value="Remote">Remote</option>
            <option value="OnSite">On-site</option>
            <option value="Hybrid">Hybrid</option>
          </select>
        </div>

        <div class="form-group">
          <label for="employmentType">Employment Type</label>
          <select id="employmentType" v-model="form.employmentType" :disabled="submitting">
            <option value="">Select employment type</option>
            <option value="FullTime">Full-time</option>
            <option value="PartTime">Part-time</option>
            <option value="Contract">Contract</option>
            <option value="Internship">Internship</option>
          </select>
        </div>
      </div>
    </section>

    <section class="form-section">
      <h3 class="section-title">Contact Information</h3>
      <div class="form-grid">
        <div class="form-group">
          <label for="contactName">Contact Name</label>
          <input
            id="contactName"
            v-model="form.contactName"
            type="text"
            :disabled="submitting"
          />
        </div>

        <div class="form-group">
          <label for="contactEmail">Contact Email</label>
          <input
            id="contactEmail"
            v-model="form.contactEmail"
            type="email"
            :disabled="submitting"
            :class="{ 'error': errors.contactEmail }"
          />
          <span v-if="errors.contactEmail" class="error-message">{{ errors.contactEmail }}</span>
        </div>
      </div>
    </section>

    <section class="form-section">
      <h3 class="section-title">Salary Information</h3>
      <div class="form-grid">
        <div class="form-group">
          <label for="salaryMin">Minimum Salary</label>
          <input
            id="salaryMin"
            v-model.number="form.salaryMin"
            type="number"
            min="0"
            :disabled="submitting"
            :class="{ 'error': errors.salaryMin }"
          />
          <span v-if="errors.salaryMin" class="error-message">{{ errors.salaryMin }}</span>
        </div>

        <div class="form-group">
          <label for="salaryMax">Maximum Salary</label>
          <input
            id="salaryMax"
            v-model.number="form.salaryMax"
            type="number"
            min="0"
            :disabled="submitting"
            :class="{ 'error': errors.salaryMax }"
          />
          <span v-if="errors.salaryMax" class="error-message">{{ errors.salaryMax }}</span>
        </div>

        <div class="form-group">
          <label for="currency">Currency</label>
          <select id="currency" v-model="form.currency" :disabled="submitting">
            <option value="">Select currency</option>
            <option value="USD">USD</option>
            <option value="EUR">EUR</option>
            <option value="GBP">GBP</option>
            <option value="CAD">CAD</option>
            <option value="AUD">AUD</option>
          </select>
        </div>

        <div class="form-group">
          <label for="salaryPeriod">Salary Period</label>
          <select id="salaryPeriod" v-model="form.salaryPeriod" :disabled="submitting">
            <option value="">Select period</option>
            <option value="Hourly">Hourly</option>
            <option value="Daily">Daily</option>
            <option value="Weekly">Weekly</option>
            <option value="Monthly">Monthly</option>
            <option value="Yearly">Yearly</option>
          </select>
        </div>
      </div>
    </section>

    <section class="form-section">
      <h3 class="section-title">Follow-up</h3>
      <div class="form-grid">
        <div class="form-group">
          <label for="nextActionDate">Next Action Date</label>
          <input
            id="nextActionDate"
            v-model="form.nextActionDate"
            type="datetime-local"
            :disabled="submitting"
          />
        </div>
      </div>

      <div class="form-group">
        <label for="notes">Notes</label>
        <textarea
          id="notes"
          v-model="form.notes"
          rows="4"
          :disabled="submitting"
        ></textarea>
      </div>
    </section>

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
import { getStatusLabel } from '@/utils/status';
import type { JobApplication, JobApplicationStatus, CreateJobApplicationRequest } from '@/types/jobApplication.types';

interface Props {
  mode: 'create' | 'edit';
  initialValues?: Partial<JobApplication>;
}

const props = defineProps<Props>();

const emit = defineEmits<{
  submit: [data: CreateJobApplicationRequest];
  cancel: [];
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

const form = ref({
  companyName: '',
  jobTitle: '',
  status: 'Applied' as JobApplicationStatus,
  appliedDate: '',
  location: '',
  jobUrl: '',
  sourceType: '',
  sourceReference: '',
  workMode: '',
  employmentType: '',
  contactName: '',
  contactEmail: '',
  salaryMin: undefined as number | undefined,
  salaryMax: undefined as number | undefined,
  currency: '',
  salaryPeriod: '',
  nextActionDate: '',
  notes: '',
});

const submitting = ref(false);

const errors = ref({
  companyName: '',
  jobTitle: '',
  appliedDate: '',
  contactEmail: '',
  salaryMin: '',
  salaryMax: '',
});

const isValid = computed(() => {
  return form.value.companyName.trim() &&
         form.value.jobTitle.trim() &&
         form.value.appliedDate &&
         !errors.value.companyName &&
         !errors.value.jobTitle &&
         !errors.value.appliedDate &&
         !errors.value.contactEmail &&
         !errors.value.salaryMin &&
         !errors.value.salaryMax;
});

// Initialize form with initial values
watch(
  () => props.initialValues,
  (newValues) => {
    if (newValues) {
      const appliedDate = newValues.appliedDate;
      const nextActionDate = newValues.nextActionDate;
      form.value = {
        companyName: newValues.companyName || '',
        jobTitle: newValues.jobTitle || '',
        status: newValues.status || 'Applied',
        appliedDate: appliedDate ? new Date(appliedDate).toISOString().slice(0, 16) : '',
        location: newValues.location || '',
        jobUrl: newValues.jobUrl || '',
        sourceType: newValues.sourceType || '',
        sourceReference: newValues.sourceReference || '',
        workMode: newValues.workMode || '',
        employmentType: newValues.employmentType || '',
        contactName: newValues.contactName || '',
        contactEmail: newValues.contactEmail || '',
        salaryMin: newValues.salaryMin,
        salaryMax: newValues.salaryMax,
        currency: newValues.currency || '',
        salaryPeriod: newValues.salaryPeriod || '',
        nextActionDate: nextActionDate ? new Date(nextActionDate).toISOString().slice(0, 16) : '',
        notes: newValues.notes || '',
      };
    }
  },
  { immediate: true }
);

const validate = () => {
  errors.value.companyName = form.value.companyName.trim() ? '' : 'Company name is required';
  errors.value.jobTitle = form.value.jobTitle.trim() ? '' : 'Job title is required';
  errors.value.appliedDate = form.value.appliedDate ? '' : 'Applied date is required';

  if (form.value.contactEmail && !/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(form.value.contactEmail)) {
    errors.value.contactEmail = 'Please enter a valid email address';
  } else {
    errors.value.contactEmail = '';
  }

  if (form.value.salaryMin !== undefined && form.value.salaryMax !== undefined && form.value.salaryMin > form.value.salaryMax) {
    errors.value.salaryMin = 'Minimum salary cannot be greater than maximum salary';
    errors.value.salaryMax = 'Maximum salary cannot be less than minimum salary';
  } else {
    errors.value.salaryMin = '';
    errors.value.salaryMax = '';
  }

  return !errors.value.companyName &&
         !errors.value.jobTitle &&
         !errors.value.appliedDate &&
         !errors.value.contactEmail &&
         !errors.value.salaryMin &&
         !errors.value.salaryMax;
};

const handleSubmit = () => {
  if (!validate() || submitting.value) return;

  submitting.value = true;

  const formData = {
    companyName: form.value.companyName.trim(),
    jobTitle: form.value.jobTitle.trim(),
    status: form.value.status,
    appliedDate: form.value.appliedDate ? new Date(form.value.appliedDate).toISOString() : undefined,
    location: form.value.location.trim() || undefined,
    jobUrl: form.value.jobUrl.trim() || undefined,
    sourceType: form.value.sourceType || undefined,
    sourceReference: form.value.sourceReference.trim() || undefined,
    workMode: form.value.workMode || undefined,
    employmentType: form.value.employmentType || undefined,
    contactName: form.value.contactName.trim() || undefined,
    contactEmail: form.value.contactEmail.trim() || undefined,
    salaryMin: form.value.salaryMin,
    salaryMax: form.value.salaryMax,
    currency: form.value.currency || undefined,
    salaryPeriod: form.value.salaryPeriod || undefined,
    nextActionDate: form.value.nextActionDate ? new Date(form.value.nextActionDate).toISOString() : undefined,
    notes: form.value.notes.trim() || undefined,
  };

  emit('submit', formData);

  // Note: Parent component should reset submitting state after handling
};
</script>

<style scoped>
.job-application-form {
  max-width: 800px;
  margin: 0 auto;
}

.form-section {
  background: #f9f9f9;
  border-radius: 8px;
  padding: 1.5rem;
  margin-bottom: 1.5rem;
  border: 1px solid #e0e0e0;
}

.section-title {
  margin: 0 0 1rem 0;
  color: #333;
  font-size: 1.25rem;
  font-weight: 600;
}

.form-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 1rem;
}

.form-group {
  margin-bottom: 1rem;
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
  min-height: 100px;
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