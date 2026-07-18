import client from './client';
import type { SymptomLog, MoodLog, SleepLog, MealLog, HealthInsights, WeeklyReport, AlertsResponse } from '../types/index.ts';

const API = '/api/v1';

export const getSymptomLogs = async (from?: string, to?: string): Promise<SymptomLog[]> => {
  const params = new URLSearchParams();
  if (from) params.append('from', from);
  if (to) params.append('to', to);
  const response = await client.get(`${API}/symptom-logs?${params}`);
  return response.data;
};

export const createSymptomLog = async (data: {
  symptomName: string; severity: number; notes?: string; logDate: string;
}): Promise<SymptomLog> => {
  const response = await client.post(`${API}/symptom-logs`, data);
  return response.data;
};

export const getMoodLogs = async (from?: string, to?: string): Promise<MoodLog[]> => {
  const params = new URLSearchParams();
  if (from) params.append('from', from);
  if (to) params.append('to', to);
  const response = await client.get(`${API}/mood-logs?${params}`);
  return response.data;
};

export const createMoodLog = async (data: {
  moodScore: number; notes?: string; logDate: string;
}): Promise<MoodLog> => {
  const response = await client.post(`${API}/mood-logs`, data);
  return response.data;
};

export const getSleepLogs = async (from?: string, to?: string): Promise<SleepLog[]> => {
  const params = new URLSearchParams();
  if (from) params.append('from', from);
  if (to) params.append('to', to);
  const response = await client.get(`${API}/sleep-logs?${params}`);
  return response.data;
};

export const createSleepLog = async (data: {
  hoursSlept: number; qualityScore: number; bedTime: string; wakeTime: string; logDate: string;
}): Promise<SleepLog> => {
  const response = await client.post(`${API}/sleep-logs`, data);
  return response.data;
};

export const getMealLogs = async (from?: string, to?: string): Promise<MealLog[]> => {
  const params = new URLSearchParams();
  if (from) params.append('from', from);
  if (to) params.append('to', to);
  const response = await client.get(`${API}/meal-logs?${params}`);
  return response.data;
};

export const createMealLog = async (data: {
  mealType: string; description: string; logDate: string;
}): Promise<MealLog> => {
  const response = await client.post(`${API}/meal-logs`, data);
  return response.data;
};

export const getInsights = async (from?: string, to?: string): Promise<HealthInsights> => {
  const params = new URLSearchParams();
  if (from) params.append('from', from);
  if (to) params.append('to', to);
  const response = await client.get(`${API}/insights/correlations?${params}`);
  return response.data;
};

export const getWeeklyReports = async (): Promise<WeeklyReport[]> => {
  const response = await client.get(`${API}/weekly-reports`);
  return response.data;
};

export const getAlerts = async (): Promise<AlertsResponse> => {
  const response = await client.get(`${API}/alerts`);
  return response.data;
};

export const exportCsv = (from: string, to: string): void => {
  const token = localStorage.getItem('accessToken');
  const baseUrl = client.defaults.baseURL ?? 'http://localhost:8080';
  const url = `${baseUrl}${API}/export/csv?from=${from}&to=${to}`;
  const link = document.createElement('a');
  link.href = url;
  // Attach token via a fetch blob download
  fetch(url, { headers: { Authorization: `Bearer ${token}` } })
    .then(res => res.blob())
    .then(blob => {
      const blobUrl = window.URL.createObjectURL(blob);
      link.href = blobUrl;
      link.download = `health-export-${from}-to-${to}.csv`;
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
      window.URL.revokeObjectURL(blobUrl);
    });
};

export const exportPdf = (from: string, to: string): void => {
  const token = localStorage.getItem('accessToken');
  const baseUrl = client.defaults.baseURL ?? 'http://localhost:8080';
  const url = `${baseUrl}${API}/export/pdf?from=${from}&to=${to}`;
  fetch(url, { headers: { Authorization: `Bearer ${token}` } })
    .then(res => res.blob())
    .then(blob => {
      const blobUrl = window.URL.createObjectURL(blob);
      const link = document.createElement('a');
      link.href = blobUrl;
      link.download = `health-report-${from}-to-${to}.pdf`;
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
      window.URL.revokeObjectURL(blobUrl);
    });
};