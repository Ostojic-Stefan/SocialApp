import Modal from "../../components/Modal";
import CreatePostForm from "../../features/posts/CreatePostForm/CreatePostForm";
import styles from "./HeaderStyles.module.css";

function Header() {
  return (
    <header className={styles.navbar}>
      <div className={styles.flexContainer}>
        <Modal>
          <Modal.Open>
            <button>Create A Post</button>
          </Modal.Open>
          <Modal.Content>
            <CreatePostForm />
          </Modal.Content>
        </Modal>
        <input type="text" placeholder="Search People" />
      </div>
    </header>
  );
}

export default Header;
