import { Outlet } from "react-router-dom";
import styles from "./AppLayout.module.css";

function AppLayout() {
  return (
    <div className={styles.layout}>
      <header className={styles.navbar}>
        <div className={styles.flexContainer}>
          <input type="text" placeholder="Search People" />
        </div>
      </header>

      <aside className={styles.profile}>
        <div className={styles.btn}>Profile</div>
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
