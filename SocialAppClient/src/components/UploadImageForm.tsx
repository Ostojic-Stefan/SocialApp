import { ChangeEvent, FormEvent, ReactNode, useState } from 'react';
import FileDropArea from './FileDropArea';
import { Button } from '@nextui-org/react';

interface Props {
  children?: ReactNode;
  onSubmit: (formData: FormData) => Promise<void>;
}

function UploadImageForm({ children, onSubmit }: Props) {
  const [file, setFile] = useState<File>();
  const [imagePreview, setImagePreview] = useState('');

  function displayImage(file: File) {
    const reader = new FileReader();
    reader.onload = function (event) {
      setImagePreview(event.target!.result as string);
    };
    reader.readAsDataURL(file);
  }

  function onDrop(files: FileList): void {
    setFile(files[0]);
    displayImage(files[0]);
  }

  function handleFileChange(event: ChangeEvent<HTMLInputElement>): void {
    if (event.target.files) {
      setFile(event.target.files[0]);
      displayImage(event.target.files[0]);
    }
  }

  function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();

    if (!file) return;

    const formData = new FormData();
    formData.append('img', file);

    console.log(formData);

    onSubmit(formData);
  }

  return (
    <form onSubmit={handleSubmit}>
      <FileDropArea onDrop={onDrop}>
        {!file ? (
          <div>
            <p style={{ fontWeight: 700, fontSize: 20 }}>Drag and Drop Image Or</p>
            <label>
              Upload
              <input type='file' onChange={handleFileChange} />
            </label>
          </div>
        ) : (
          <img width={150} src={imagePreview} />
        )}
      </FileDropArea>
      {children}
      <div>
        <Button type='submit'>Submit</Button>
      </div>
    </form>
  );
}

export default UploadImageForm;
