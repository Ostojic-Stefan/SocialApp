import { ChangeEvent, FormEvent, useState } from "react";
import styles from "./CreatePostForm.module.css";
import { useAppDispatch, useAppSelector } from "../../../store";
import { uploadPost } from "../postSlice";

function CreatePostForm() {
  const dispatch = useAppDispatch();
  const avatarUrl = useAppSelector((store) => store.user.userInfo?.avatarUrl);

  const [file, setFile] = useState<File>();
  const [contents, setContents] = useState<string>("");

  function handleFileChange(ev: ChangeEvent<HTMLInputElement>): void {
    if (ev.target.files) {
      setFile(ev.target.files[0]);
    }
  }
  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    if (!file) return;

    const formData = new FormData();
    formData.append("img", file);
    dispatch(uploadPost({ formData, contents }));

    setContents("");
  }

  return (
    <form onSubmit={handleSubmit} className={styles.createPost}>
      <img src={avatarUrl} className={styles.createPostImg} />
      <input
        type="text"
        placeholder="Create Post"
        value={contents}
        onChange={(e) => setContents(e.target.value)}
        className={styles.createPostInput}
      ></input>
      <input type="file" onChange={handleFileChange} />
      <button className={styles.btn}>Submit</button>
    </form>
  );
}

export default CreatePostForm;
