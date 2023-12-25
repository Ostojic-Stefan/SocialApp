import { NavLink, Outlet } from 'react-router-dom';
import styles from './AppLayout.module.css';
import { useAppSelector } from '../../store';
import Header from '../components/Header';
import FriendList from '../../features/userProfile/friends/FriendList';

function AppLayout() {
  const { userInfo } = useAppSelector((store) => store.user);

  return (
    <div className={styles.layout}>
      <Header />
      <aside className={styles.profile}>
        {userInfo ? (
          <>
            <NavLink to='/' className={styles.btn}>
              Home
            </NavLink>
            <NavLink to={`/profile/${userInfo?.username}`} className={styles.btn}>
              Profile
            </NavLink>
            <div className={styles.btn}>Messages</div>
            <div className={styles.btn}>Notifications</div>
          </>
        ) : (
          <>
            <NavLink to='/login'>Login</NavLink>
            <NavLink to='/register'>Register</NavLink>
          </>
        )}
      </aside>
      <main className={styles.content}>
        <Outlet />
      </main>
      <aside className={styles.friendList}>
        <FriendList />
      </aside>
    </div>
  );
}

export default AppLayout;
