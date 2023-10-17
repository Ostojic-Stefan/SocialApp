import { ChangeEvent, FormEvent, useState } from "react";
import styles from "./CreatePostForm.module.css";
import { useAppDispatch } from "../../../store";
import { uploadPost } from "../postSlice";
import FileDropArea from "../../../components/FileDropArea";
import Button from "../../../ui/components/Button/Button";

function CreatePostForm() {
  const dispatch = useAppDispatch();
  const [contents, setContents] = useState<string>("");
  const [file, setFile] = useState<File>();
  const [imagePreview, setImagePreview] = useState("");

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    console.log({ contents, file });

    if (!file) return;

    const formData = new FormData();
    formData.append("img", file);
    dispatch(uploadPost({ formData, contents }));

    setContents("");
  }

  function displayImage(file: File) {
    const reader = new FileReader();
    reader.onload = function (event) {
      setImagePreview(event.target!.result as string);
    };
    reader.readAsDataURL(file);
  }

  function onFileChange(files: FileList): void {
    setFile(files[0]);
    displayImage(files[0]);
  }

  function handleFileChange(event: ChangeEvent<HTMLInputElement>): void {
    if (event.target.files) {
      setFile(event.target.files[0]);
      displayImage(event.target.files[0]);
    }
  }

  return (
    <form onSubmit={handleSubmit} className={styles.createPost}>
      <FileDropArea onFileChange={onFileChange}>
        {!file ? (
          <div className={styles.fileDropContents}>
            <p style={{ fontWeight: 700, fontSize: 20 }}>Drag and Drop Image Or</p>
            <label className={styles.btn}>
              Upload
              <input type="file" onChange={handleFileChange} />
            </label>
          </div>
        ) : (
          <img width={150} src={imagePreview} />
        )}
      </FileDropArea>
      <input
        type="text"
        placeholder="Enter text Here"
        value={contents}
        onChange={(e) => setContents(e.target.value)}
        className={styles.createPostInput}
      />
      <div className={styles.buttonContainer}>
        <Button className={styles.btn}>
          Cancel
        </Button>
        <Button className={styles.btn} type="submit">
          Submit
        </Button>
      </div>
    </form>
  );
}

export default CreatePostForm;
