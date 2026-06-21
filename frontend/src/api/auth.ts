import client from './client';
import type { AuthResponse } from '../types/index';

export const register = async (data: {
  firstName: string; lastName: string; email: string; password: string;
}): Promise<AuthResponse> => {
  const response = await client.post('/api/v1/auth/register', data);
  return response.data;
};

export const login = async (data: {
  email: string; password: string;
}): Promise<AuthResponse> => {
  const response = await client.post('/api/v1/auth/login', data);
  return response.data;
};