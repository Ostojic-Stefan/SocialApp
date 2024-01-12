import ReactDOM from 'react-dom/client';
import App from './App.tsx';
import { BrowserRouter } from 'react-router-dom';
import Providers from './Providers.tsx';

import './index.css';
ReactDOM.createRoot(document.getElementById('root')!).render(
  <>
    <BrowserRouter>
      <Providers>
        <App />
        {/* <ToastContainer
          position='bottom-right'
          autoClose={5000}
          hideProgressBar={false}
          newestOnTop={false}
          closeOnClick
          rtl={false}
          pauseOnFocusLoss
          draggable
          pauseOnHover
          theme='light'
        /> */}
      </Providers>
    </BrowserRouter>
  </>
);
