export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  expiresAt: string;
  firstName: string;
  lastName: string;
  email: string;
}

export interface SymptomLog {
  id: string;
  symptomName: string;
  severity: number;
  notes?: string;
  logDate: string;
  createdAt: string;
}

export interface MoodLog {
  id: string;
  moodScore: number;
  notes?: string;
  logDate: string;
  createdAt: string;
}

export interface SleepLog {
  id: string;
  hoursSlept: number;
  qualityScore: number;
  bedTime: string;
  wakeTime: string;
  logDate: string;
  createdAt: string;
}

export interface MealLog {
  id: string;
  mealType: string;
  description: string;
  logDate: string;
  createdAt: string;
}

export interface HealthInsights {
  insights: string;
  from: string;
  to: string;
  totalLogsAnalyzed: number;
  generatedAt: string;
}

export interface WeeklyReport {
  id: string;
  weekStartDate: string;
  weekEndDate: string;
  aiInsights: string;
  topSymptoms: string;
  avgMoodScore: number;
  avgSleepHours: number;
  generatedAt: string;
}

export interface HealthAlert {
  severity: 'danger' | 'warning' | 'success' | 'info';
  title: string;
  message: string;
  category: string;
}

export interface AlertsResponse {
  generatedAt: string;
  period: { from: string; to: string };
  totalAlerts: number;
  alerts: HealthAlert[];
}