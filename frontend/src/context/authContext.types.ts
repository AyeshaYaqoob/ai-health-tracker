import { createContext } from 'react';
import type { AuthResponse } from '../types/index.ts';

export interface AuthContextType {
  isAuthenticated: boolean;
  user: { firstName: string; lastName: string; email: string } | null;
  login: (data: AuthResponse) => void;
  logout: () => void;
}

export const AuthContext = createContext<AuthContextType | null>(null);
