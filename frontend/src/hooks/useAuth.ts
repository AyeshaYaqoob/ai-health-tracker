import { useContext } from 'react';
import { AuthContext } from '../context/authContext.types.ts';

/** Hook to access auth state and actions. Must be used within AuthProvider. */
export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) throw new Error('useAuth must be used within AuthProvider');
  return context;
};
