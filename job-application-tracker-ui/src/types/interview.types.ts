export type InterviewRoundType =
  | 'Phone Screen'
  | 'Technical Interview'
  | 'Coding Challenge'
  | 'Onsite Interview'
  | 'Final Interview'
  | 'Other';

export type InterviewResult = 'Pending' | 'Passed' | 'Failed' | 'No Show';

export interface InterviewRound {
  id: string;
  jobApplicationId: string;
  roundNumber: number;
  interviewType: InterviewRoundType;
  interviewDate: string;
  interviewerName?: string;
  interviewerEmail?: string;
  outcome?: InterviewResult;
  notes?: string;
}

export interface CreateInterviewRoundRequest {
  roundNumber: number;
  interviewType: InterviewRoundType;
  interviewDate: string;
  interviewerName?: string;
  interviewerEmail?: string;
  outcome?: InterviewResult;
  notes?: string;
}

export interface UpdateInterviewRoundRequest {
  roundNumber?: number;
  interviewType?: InterviewRoundType;
  interviewDate?: string;
  interviewerName?: string;
  interviewerEmail?: string;
  outcome?: InterviewResult;
  notes?: string;
}