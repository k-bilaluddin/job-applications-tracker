import type { JobApplicationStatus } from '@/types/jobApplication.types';

export const getStatusLabel = (status: JobApplicationStatus): string => {
  const labels: Record<JobApplicationStatus, string> = {
    'Applied': 'Applied',
    'In Review': 'In Review',
    'Interview Scheduled': 'Interview Scheduled',
    'Interviewing': 'Interviewing',
    'Offer Received': 'Offer Received',
    'Rejected': 'Rejected',
    'Accepted': 'Accepted',
    'Withdrawn': 'Withdrawn',
  };
  return labels[status];
};