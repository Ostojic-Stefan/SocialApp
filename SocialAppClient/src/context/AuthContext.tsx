import { createContext, useContext, useEffect, useState } from 'react';
import { CurrentUserInfo, identityService } from '../api/identityService';
import { useLocalStorage } from '../hooks/useLocalStorage';
import { useNavigate } from 'react-router-dom';

interface AuthState {
  user: CurrentUserInfo | null;
  login: (email: string, password: string) => Promise<void>;
  logout: () => void;
}

const AuthContext = createContext<AuthState | null>(null);

interface Props {
  children: React.ReactNode;
}

export const AuthProvider = ({ children }: Props) => {
  const [user, setUser] = useState<CurrentUserInfo | null>(null);
  const { value, setValue } = useLocalStorage<string | null>(null, 'auth');
  const navigate = useNavigate();

  useEffect(() => {
    async function getUserData() {
      if (value) {
        const response = await identityService.getCurrentUserInfo();
        if (response.hasError) {
          console.log('Failed to authenticate');
          navigate('/login');
          return;
        }
        setUser(response.value);
        // navigate('/');
      } else {
        console.log('No token present in local storage');
        // navigate('/login');
      }
    }
    getUserData();
  }, []);

  async function login(email: string, password: string) {
    const response = await identityService.login({ email, password });
    if (response.hasError) {
      console.log('Failed to Login');
      return;
    }
    setValue(response.value.data);
    // const userDataResponse = await identityService.getCurrentUserInfo();
    // if (userDataResponse.hasError) {
    //   console.log('Failed to get user data');
    //   return;
    // }
    // setUser(userDataResponse.value);
    // console.log('Successfully logged in');
    navigate('/');
    window.location.reload();
  }

  function logout() {
    setValue(null);
    setUser(null);
    navigate('/login', { replace: true });
  }

  return <AuthContext.Provider value={{ login, logout, user }}>{children}</AuthContext.Provider>;
};

export function useAuth() {
  const authContext = useContext(AuthContext);
  if (!authContext) {
    console.error('useAuth needs to be used inside a AuthContext');
  }
  return authContext!;
}
