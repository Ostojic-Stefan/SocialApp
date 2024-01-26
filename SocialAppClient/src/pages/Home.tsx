import HomeNav from '../components/HomeNav';
import { Outlet } from 'react-router-dom';
import TopicList from '../components/TopicList';
import FriendList from '../components/FriendList';
import { Divider } from '@nextui-org/react';

function Home() {
  return (
    <>
      <div className='flex flex-col gap-9'>
        <HomeNav />
        <Divider />
        <div className='grid grid-cols-[1fr_2fr_1fr] gap-16'>
          <TopicList />
          <Outlet />
          <FriendList />
        </div>
      </div>
    </>
  );
}

export default Home;
