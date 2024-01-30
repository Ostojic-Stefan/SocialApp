import { createContext, useContext, useEffect, useState } from 'react';
import { identityService } from '../api/identityService';
import { useNavigate } from 'react-router-dom';
import { CurrentUserResponse, RegisterRequest } from '../api/dtos/identity';
import { failureToast } from '../utils/toastDefinitions';

export enum AuthStatus {
  Unauthenticated,
  Authenticated,
  Loading,
}

interface AuthState {
  user: CurrentUserResponse;
  authStatus: AuthStatus;
  login: (email: string, password: string) => Promise<void>;
  logout: () => void;
  register: (registerRequest: RegisterRequest) => Promise<void>;
}

const AuthContext = createContext<AuthState | null>(null);

interface Props {
  children: React.ReactNode;
}

export const AuthProvider = ({ children }: Props) => {
  const [user, setUser] = useState<CurrentUserResponse | null>(null);
  const [authStatus, setAuthStatus] = useState<AuthStatus>(AuthStatus.Loading);
  const navigate = useNavigate();

  useEffect(() => {
    getUserData();
  }, []);

  async function getUserData() {
    const response = await identityService.getCurrentUserInfo();

    if (response.hasError) {
      setAuthStatus(AuthStatus.Unauthenticated);
      failureToast(response.error.title);
      console.log(response.error);
      return;
    }

    setAuthStatus(AuthStatus.Authenticated);
    setUser(response.value);
  }

  async function login(email: string, password: string) {
    setAuthStatus(AuthStatus.Loading);
    const response = await identityService.login({ email, password });
    if (response.hasError) {
      console.log(response.error);
      setAuthStatus(AuthStatus.Unauthenticated);
      return;
    }
    setAuthStatus(AuthStatus.Authenticated);
    await getUserData();
    navigate('/');
  }

  async function register(registerRequest: RegisterRequest) {
    const response = await identityService.register(registerRequest);
    if (response.hasError) {
      console.log('Failed to register');
      return;
    }
    navigate('/login');
  }

  async function logout() {
    await identityService.logout();
    setUser(null);
    navigate('/login', { replace: true });
    setAuthStatus(AuthStatus.Unauthenticated);
  }

  return (
    <AuthContext.Provider value={{ login, logout, user: user!, register, authStatus }}>{children}</AuthContext.Provider>
  );
};

export function useAuth() {
  const authContext = useContext(AuthContext);
  if (!authContext) {
    console.error('useAuth needs to be used inside a AuthContext');
  }
  return authContext!;
}
