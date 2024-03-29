import { NextUIProvider } from '@nextui-org/react';
import React from 'react';
import { Provider } from 'react-redux';
import store from './store/index.ts';
import { AuthProvider } from './context/AuthContext.tsx';
import { ToastContainer } from 'react-toastify';

function Providers({ children }: { children: React.ReactNode }) {
  return (
    <NextUIProvider>
      <AuthProvider>
        <ToastContainer />
        <Provider store={store}>{children}</Provider>
      </AuthProvider>
    </NextUIProvider>
  );
}

export default Providers;
