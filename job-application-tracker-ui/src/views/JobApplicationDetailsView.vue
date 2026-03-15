<script setup lang="ts">
import { ref, computed, onMounted } from 'vue';
import { storeToRefs } from 'pinia';
import { useRoute, useRouter } from 'vue-router';
import { useJobApplicationsStore } from '@/stores/jobApplications.store';
import { useInterviewsStore } from '@/stores/interviews.store';
import StatusBadge from '@/components/common/StatusBadge.vue';
import JobApplicationStatusUpdater from '@/components/job-applications/JobApplicationStatusUpdater.vue';
import InterviewList from '@/components/interviews/InterviewList.vue';
import AppLoader from '@/components/common/AppLoader.vue';
import AppError from '@/components/common/AppError.vue';
import InterviewForm from '@/components/interviews/InterviewForm.vue';
import ConfirmDialog from '@/components/common/ConfirmDialog.vue';
import { formatDate } from '@/utils/date';
import type { JobApplicationStatus } from '@/types/jobApplication.types';
import type {
  InterviewRound,
  CreateInterviewRoundRequest,
  UpdateInterviewRoundRequest,
} from '@/types/interview.types';

const route = useRoute();
const router = useRouter();
const jobStore = useJobApplicationsStore();
const interviewStore = useInterviewsStore();

const applicationId = computed(() => route.params.id as string);

// Local reactive state
const showInterviewForm = ref(false);
const editingInterviewId = ref<string | null>(null);
const showDeleteConfirm = ref(false);
const interviewToDelete = ref<string | null>(null);

// Pinia state refs
const {
  selectedApplication,
  loading,
  error,
  submitting: statusUpdating,
} = storeToRefs(jobStore);

const {
  interviews,
  loading: interviewsLoading,
  submitting: interviewSubmitting,
  error: interviewError,
} = storeToRefs(interviewStore);

const fetchData = async (): Promise<void> => {
  if (!applicationId.value) return;

  try {
    await jobStore.fetchApplicationById(applicationId.value);
  } catch (err) {
    console.error('Failed to fetch application', err);
  }

  try {
    await interviewStore.fetchInterviews(applicationId.value);
  } catch (err) {
    console.error('Failed to fetch interviews', err);
  }
};

onMounted(async () => {
  console.log('Route param id:', applicationId.value);

  await fetchData();

  console.log('selectedApplication:', selectedApplication.value);
  console.log('jobStore error:', error.value);
  console.log('interviews:', interviews.value);
  console.log('interview error:', interviewError.value);
});

const handleStatusUpdate = async (newStatus: JobApplicationStatus): Promise<void> => {
  try {
    await jobStore.updateApplicationStatus(applicationId.value, { status: newStatus });
  } catch {
    // handled in store
  }
};

const isEditingInterview = computed(() => Boolean(editingInterviewId.value));

const selectedInterview = computed(() => {
  if (!editingInterviewId.value) return undefined;
  return getInterviewById(editingInterviewId.value);
});

const startAddInterview = (): void => {
  console.log('[Details] startAddInterview called');
  editingInterviewId.value = null;
  showInterviewForm.value = true;
};

const startEditInterview = (id: string): void => {
  console.log('[Details] startEditInterview called', { id });
  editingInterviewId.value = id;
  showInterviewForm.value = true;
};

const getInterviewById = (id: string): InterviewRound | undefined => {
  return interviews.value.find((interview) => interview.id === id);
};

const location = computed(() => {
  // The backend may not always provide a location field, so default gracefully.
  return selectedApplication.value?.location ?? 'N/A';
});

const salaryDisplay = computed(() => {
  const app = selectedApplication.value;
  if (!app?.salaryMin && !app?.salaryMax) return 'N/A';
  const min = app.salaryMin ? new Intl.NumberFormat().format(app.salaryMin) : '';
  const max = app.salaryMax ? new Intl.NumberFormat().format(app.salaryMax) : '';
  const range = [min, max].filter(Boolean).join(' - ');
  const currency = app.currency || '';
  const period = app.salaryPeriod || '';
  return `${currency} ${range} / ${period}`.trim();
});

const nextActionDateDisplay = computed(() => {
  return selectedApplication.value?.nextActionDate ? formatDate(selectedApplication.value.nextActionDate) : 'N/A';
});

const appliedDateDisplay = computed(() => {
  return selectedApplication.value ? formatDate(selectedApplication.value.appliedDate) : '';
});

const handleInterviewSubmit = async (
  data: CreateInterviewRoundRequest | UpdateInterviewRoundRequest
): Promise<void> => {
  console.log('[Details] handleInterviewSubmit called', {
    editingInterviewId: editingInterviewId.value,
    data,
  });
  try {
    if (editingInterviewId.value) {
      await interviewStore.updateInterview(
        applicationId.value,
        editingInterviewId.value,
        data as UpdateInterviewRoundRequest
      );
    } else {
      await interviewStore.createInterview(
        applicationId.value,
        data as CreateInterviewRoundRequest
      );
    }

    showInterviewForm.value = false;
    editingInterviewId.value = null;
  } catch {
    // handled in store
  }
};

const cancelInterviewForm = (): void => {
  console.log('[Details] cancelInterviewForm called');
  showInterviewForm.value = false;
  editingInterviewId.value = null;
};

const confirmDeleteInterview = (id: string): void => {
  console.log('[Details] confirmDeleteInterview called', { id });
  interviewToDelete.value = id;
  showDeleteConfirm.value = true;
};

const deleteInterview = async (): Promise<void> => {
  console.log('[Details] deleteInterview called', { interviewToDelete: interviewToDelete.value });
  if (!interviewToDelete.value) {
    showDeleteConfirm.value = false;
    return;
  }

  try {
    await interviewStore.deleteInterview(applicationId.value, interviewToDelete.value);
  } catch {
    // handled in store
  } finally {
    showDeleteConfirm.value = false;
    interviewToDelete.value = null;
  }
};

const cancelDelete = (): void => {
  console.log('[Details] cancelDelete called');
  showDeleteConfirm.value = false;
  interviewToDelete.value = null;
};
</script>

<template>
  <div class="page">
    <div class="page__container">
      <header class="page__header">
        <div class="page__title">
          <h1>Application details</h1>
          <p class="page__subtitle">View and manage application information and interviews</p>
        </div>
        <div class="page__actions">
          <button type="button" class="btn btn--secondary" @click="router.back()">
            Back
          </button>
          <button type="button" class="btn btn--primary" @click="startAddInterview">
            + Schedule Interview
          </button>
        </div>
      </header>

      <AppLoader v-if="loading || interviewsLoading" class="page__spinner" />

      <AppError
        v-else-if="error || interviewError"
        :message="error || interviewError || 'An unexpected error occurred'"
      />

      <div v-else-if="!selectedApplication" class="card card--empty">
        <p>Application not found.</p>
      </div>

      <div v-else class="page__content">
        <section class="card">
          <div class="card__header">
            <div class="card__header-content">
              <h2 class="card__title">{{ selectedApplication.companyName }}</h2>
              <p class="card__subtitle">{{ selectedApplication.jobTitle }}</p>
            </div>
            <div class="card__actions">
              <StatusBadge :status="selectedApplication.status" />
              <JobApplicationStatusUpdater
                :status="selectedApplication.status"
                :loading="statusUpdating"
                @update:status="handleStatusUpdate"
              />
            </div>
          </div>

          <div class="card__body">
            <div class="meta-grid">
              <div class="meta-item">
                <div class="meta-label">Location</div>
                <div class="meta-value">{{ location }}</div>
              </div>
              <div class="meta-item">
                <div class="meta-label">Work Mode</div>
                <div class="meta-value">{{ selectedApplication.workMode || 'N/A' }}</div>
              </div>
              <div class="meta-item">
                <div class="meta-label">Employment Type</div>
                <div class="meta-value">{{ selectedApplication.employmentType || 'N/A' }}</div>
              </div>
              <div class="meta-item">
                <div class="meta-label">Applied Date</div>
                <div class="meta-value">{{ appliedDateDisplay }}</div>
              </div>
              <div class="meta-item">
                <div class="meta-label">Next Action Date</div>
                <div class="meta-value">{{ nextActionDateDisplay }}</div>
              </div>
              <div class="meta-item" v-if="selectedApplication.contactName">
                <div class="meta-label">Contact Name</div>
                <div class="meta-value">{{ selectedApplication.contactName }}</div>
              </div>
              <div class="meta-item" v-if="selectedApplication.contactEmail">
                <div class="meta-label">Contact Email</div>
                <div class="meta-value">
                  <a :href="`mailto:${selectedApplication.contactEmail}`">{{ selectedApplication.contactEmail }}</a>
                </div>
              </div>
              <div class="meta-item" v-if="selectedApplication.sourceType">
                <div class="meta-label">Source Type</div>
                <div class="meta-value">{{ selectedApplication.sourceType }}</div>
              </div>
              <div class="meta-item" v-if="selectedApplication.sourceReference">
                <div class="meta-label">Source Reference</div>
                <div class="meta-value">{{ selectedApplication.sourceReference }}</div>
              </div>
              <div class="meta-item">
                <div class="meta-label">Salary</div>
                <div class="meta-value">{{ salaryDisplay }}</div>
              </div>
              <div class="meta-item" v-if="selectedApplication.jobUrl">
                <div class="meta-label">Job URL</div>
                <div class="meta-value">
                  <a :href="selectedApplication.jobUrl" target="_blank" rel="noopener">View Job Posting</a>
                </div>
              </div>
            </div>

            <div class="section">
              <h3 class="section__title">Notes</h3>
              <p class="section__text" v-if="selectedApplication.notes">
                {{ selectedApplication.notes }}
              </p>
              <p class="section__text section__text--muted" v-else>
                No notes provided.
              </p>
            </div>
          </div>
        </section>

        <section class="card">
          <div class="card__header card__header--with-action">
            <h2 class="card__title">Interview rounds</h2>
            <button class="btn btn--secondary" type="button" @click="startAddInterview">
              Add interview
            </button>
          </div>
          <div class="card__body card__body--interviews">
            <InterviewList
              :interviews="interviews"
              @edit="startEditInterview"
              @delete="confirmDeleteInterview"
            />

            <section v-if="showInterviewForm" class="interview-form-panel">
              <header class="interview-form-panel__header">
                <h3 class="interview-form-panel__title">
                  {{ isEditingInterview ? 'Edit Interview' : 'Schedule Interview' }}
                </h3>
                <button type="button" class="btn btn--ghost" @click="cancelInterviewForm">
                  Close
                </button>
              </header>

              <InterviewForm
                :mode="isEditingInterview ? 'edit' : 'create'"
                :initialValues="selectedInterview"
                :submitting="interviewSubmitting"
                @submit="handleInterviewSubmit"
                @cancel="cancelInterviewForm"
              />
            </section>
          </div>
        </section>
      </div>

      <ConfirmDialog
        :show="showDeleteConfirm"
        title="Delete interview"
        message="Are you sure you want to delete this interview round?"
        confirmText="Delete"
        cancelText="Cancel"
        @confirm="deleteInterview"
        @cancel="cancelDelete"
      />
    </div>
  </div>
</template>

<style scoped>
.page {
  min-height: 100vh;
  background: #f6f8fb;
  padding: 2.5rem 1rem;
  color: #2b2b2b;
}

.page__container {
  max-width: 1024px;
  margin: 0 auto;
  display: grid;
  gap: 1.5rem;
}

.page__content {
  display: grid;
  gap: 1.25rem;
}

.page__header {
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  gap: 1rem;
  flex-wrap: wrap;
}

.page__title h1 {
  margin: 0;
  font-size: 1.75rem;
  letter-spacing: -0.02em;
}

.page__subtitle {
  margin: 0.25rem 0 0;
  color: #5f6a77;
  font-size: 0.95rem;
}

.page__actions {
  display: flex;
  gap: 0.75rem;
  align-items: center;
}

.page__spinner {
  padding: 2.5rem 0;
}

.card {
  background: white;
  border-radius: 12px;
  box-shadow: 0 10px 20px rgba(0, 0, 0, 0.04);
  border: 1px solid rgba(140, 152, 164, 0.15);
  overflow: hidden;
}

.card--empty {
  padding: 2rem;
  text-align: center;
  color: #5f6a77;
}

.card__header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  gap: 1rem;
  padding: 1.25rem 1.5rem;
  border-bottom: 1px solid rgba(140, 152, 164, 0.15);
}

.card__header-content {
  display: flex;
  flex-direction: column;
  gap: 0.35rem;
}

.card__title {
  font-size: 1.25rem;
  margin: 0;
  color: #1c2530;
}

.card__subtitle {
  margin: 0;
  color: #5f6a77;
  font-size: 0.95rem;
}

.card__actions {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
  align-items: flex-end;
}

.status-row {
  display: grid;
  gap: 0.75rem;
  grid-template-columns: auto auto;
  align-items: center;
}

.card__body {
  padding: 1.25rem 1.5rem;
  display: flex;
  flex-direction: column;
  gap: 1.25rem;
}

.card__body--interviews {
  gap: 1rem;
}

.meta-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 1.5rem;
}

.meta-item {
  display: grid;
  gap: 0.25rem;
}

.meta-label {
  font-size: 0.85rem;
  font-weight: 600;
  color: #5f6a77;
}

.meta-value {
  font-size: 1rem;
  color: #2b2b2b;
}

.meta-value a {
  color: #2f80ed;
  text-decoration: none;
}

.meta-value a:hover {
  text-decoration: underline;
}

.section {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
  padding-top: 0.5rem;
  border-top: 1px solid rgba(140, 152, 164, 0.15);
}

.section__title {
  margin: 0;
  font-size: 1.05rem;
  color: #1c2530;
}

.section__text {
  margin: 0;
  color: #2b2b2b;
  line-height: 1.6;
}

.section__text--muted {
  color: #5f6a77;
}

.card__header--with-action {
  align-items: center;
}

.interview-form-panel {
  border: 1px solid rgba(140, 152, 164, 0.2);
  border-radius: 10px;
  background: #fbfcfe;
  padding: 1rem;
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.interview-form-panel__header {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 0.75rem;
  padding-bottom: 0.75rem;
  border-bottom: 1px solid rgba(140, 152, 164, 0.2);
}

.interview-form-panel__title {
  margin: 0;
  font-size: 1rem;
  color: #1c2530;
}

.interview-form-panel :deep(.interview-form) {
  max-width: none;
  margin: 0;
}

.interview-form-panel :deep(.form-group) {
  margin-bottom: 1rem;
}

.interview-form-panel :deep(.form-actions) {
  margin-top: 1.25rem;
}

/* Button styles for this view */
.btn {
  border: none;
  border-radius: 999px;
  padding: 0.55rem 1rem;
  cursor: pointer;
  font-weight: 600;
  font-size: 0.95rem;
  transition: transform 0.15s ease, background-color 0.15s ease;
}

.btn:focus-visible {
  outline: 2px solid rgba(52, 152, 219, 0.8);
  outline-offset: 2px;
}

.btn--primary {
  background-color: #2f80ed;
  color: white;
}

.btn--primary:hover {
  background-color: #256ac9;
}

.btn--secondary {
  background-color: #f1f4f8;
  color: #2b2b2b;
}

.btn--secondary:hover {
  background-color: #e2e8f0;
}

.btn--ghost {
  background-color: transparent;
  color: #5f6a77;
  border: 1px solid rgba(140, 152, 164, 0.35);
}

.btn--ghost:hover {
  background-color: #f3f6fb;
  color: #2b2b2b;
}

@media (max-width: 720px) {
  .page__container {
    padding: 0 0.75rem;
  }

  .card__header {
    flex-direction: column;
    align-items: flex-start;
  }

  .card__actions {
    width: 100%;
    flex-direction: row;
    justify-content: flex-start;
    flex-wrap: wrap;
  }

  .status-row {
    grid-template-columns: 1fr;
    width: 100%;
  }

  .meta-grid {
    grid-template-columns: 1fr;
  }
}
</style>