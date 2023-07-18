import styles from "./CreatePostForm.module.css";

function CreatePostForm() {
  return (
    <div className={styles.createPost}>
      <img src="./images/meme.jpg" className={styles.createPostImg} />
      <input
        type="text"
        placeholder="Create Post"
        className={styles.createPostInput}
      ></input>
      <input type="file" />
      <button className={styles.btn}>Submit</button>
    </div>
  );
}

export default CreatePostForm;
