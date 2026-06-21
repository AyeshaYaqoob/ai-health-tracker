import { useState, type ReactNode } from 'react';
import type { AuthResponse } from '../types/index.ts';
import { AuthContext } from './authContext.types.ts';

/** Read persisted auth state once at startup — no effect needed. */
function getInitialAuth() {
  const token = localStorage.getItem('accessToken');
  const savedUser = localStorage.getItem('user');
  if (token && savedUser) {
    try {
      return {
        isAuthenticated: true,
        user: JSON.parse(savedUser) as { firstName: string; lastName: string; email: string },
      };
    } catch {
      // corrupted storage — treat as logged out
    }
  }
  return { isAuthenticated: false, user: null };
}

export default function AuthProvider({ children }: { children: ReactNode }) {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(
    () => getInitialAuth().isAuthenticated
  );
  const [user, setUser] = useState<{ firstName: string; lastName: string; email: string } | null>(
    () => getInitialAuth().user
  );

  const login = (data: AuthResponse) => {
    localStorage.setItem('accessToken', data.accessToken);
    localStorage.setItem('refreshToken', data.refreshToken);
    localStorage.setItem('user', JSON.stringify({
      firstName: data.firstName,
      lastName: data.lastName,
      email: data.email,
    }));
    setIsAuthenticated(true);
    setUser({ firstName: data.firstName, lastName: data.lastName, email: data.email });
  };

  const logout = () => {
    localStorage.clear();
    setIsAuthenticated(false);
    setUser(null);
  };

  return (
    <AuthContext.Provider value={{ isAuthenticated, user, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
}