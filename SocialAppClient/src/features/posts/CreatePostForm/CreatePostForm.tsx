import { useState } from "react";
import styles from "./CreatePostForm.module.css";
import { useAppDispatch } from "../../../store";
import { uploadPost } from "../postSlice";
import UploadImageForm from "../../../ui/ImageUploadForm/UploadImageForm";

function CreatePostForm() {
  const dispatch = useAppDispatch();
  const [contents, setContents] = useState<string>("");

  async function handleSubmit(formData: FormData) {
    dispatch(uploadPost({ formData, contents }));
    setContents("");
  }

  return (
    <UploadImageForm onSubmit={handleSubmit}>
      <input
        type="text"
        placeholder="Enter text Here"
        value={contents}
        onChange={(e) => setContents(e.target.value)}
        className={styles.createPostInput}
      />
    </UploadImageForm>
  );
}

export default CreatePostForm;
