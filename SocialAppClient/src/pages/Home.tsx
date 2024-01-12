import HomeNav from '../components/HomeNav';
import FeaturedGroups from '../components/FeaturedGroups';
import { Outlet } from 'react-router-dom';
import TopicList from '../components/TopicList';

function Home() {
  return (
    <>
      <div className='flex flex-col gap-9'>
        <HomeNav />
        <div className='grid grid-cols-[1fr_2fr_1fr] gap-16'>
          <TopicList />
          <Outlet />
          <FeaturedGroups />
        </div>
      </div>
    </>
  );
}

export default Home;
