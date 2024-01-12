import { Outlet } from 'react-router-dom';
import Header from '../components/Header';

function AppLayout() {
  return (
    <div className='flex flex-col gap-12'>
      <Header />
      <div className='container mx-auto px-4 max-w-9xl' style={{ height: '90vh' }}>
        <Outlet />
      </div>
    </div>
  );
}

export default AppLayout;
