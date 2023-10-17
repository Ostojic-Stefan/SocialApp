import styles from "./HeaderStyles.module.css";

function Header() {
  return (
    <header className={styles.navbar}>
      <h1 className={styles.logo}>ChadBook</h1>
      <div className={styles.flexContainer}>
        <input type="text" placeholder="Search People" />
      </div>
    </header>
  );
}

export default Header;
