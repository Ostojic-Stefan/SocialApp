import { NavLink, Outlet } from "react-router-dom";
import styles from "./AppLayout.module.css";
import { useAppSelector } from "../../store";
import Header from "../components/Header";

function AppLayout() {
  const userInfo = useAppSelector(
    (store) => store.identity.userInfo?.userInformation
  );

  return (
    <div className={styles.layout}>
      <Header />
      <aside className={styles.profile}>
        <NavLink to="/" className={styles.btn}>
          Home
        </NavLink>
        <NavLink to={`/profile/${userInfo?.username}`} className={styles.btn}>
          Profile
        </NavLink>
        <div className={styles.btn}>Messages</div>
        <div className={styles.btn}>Notifications</div>
      </aside>

      <main className={styles.content}>
        <Outlet />
      </main>
      <aside className={styles.friendList}>Firend List</aside>
    </div>
  );
}

export default AppLayout;
