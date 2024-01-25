import { ReactNode } from 'react';
import { AuthStatus, useAuth } from '../context/AuthContext';
import { Navigate } from 'react-router-dom';

interface AuthProps {
  children: ReactNode;
}

export default function Auth({ children }: AuthProps) {
  const { authStatus } = useAuth();
  if (authStatus === AuthStatus.Loading) {
    return <h1>Loading initial data...</h1>;
  } else if (authStatus === AuthStatus.Unauthenticated) {
    return <Navigate to={'/login'} />;
  }
  return <>{children}</>;
}
