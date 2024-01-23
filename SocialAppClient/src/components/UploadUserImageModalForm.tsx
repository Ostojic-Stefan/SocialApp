import { Button, Modal, ModalContent, ModalHeader } from '@nextui-org/react';
import { FormEvent, useState } from 'react';
import { userService } from '../api/userService';

interface UploadUserImageModalFormProps {
  isOpen: boolean;
  onOpenChange: () => void;
}

function UploadUserImageModalForm({ isOpen, onOpenChange }: UploadUserImageModalFormProps) {
  const [file, setFile] = useState<File | undefined>();
  const [preview, setPreview] = useState<string | undefined>(undefined);

  async function handleSubmitImage(event: FormEvent<HTMLFormElement>): Promise<void> {
    event.preventDefault();
    if (!file) {
      console.log('must select a file');
      return;
    }

    const formData = new FormData();
    formData.append('img', file);
    const response = await userService.uploadImage(formData);
    if (response.hasError) {
      console.log(response.error);
      return;
    }
    const res = await userService.addUserImage({ avatarUrl: response.value.imageName });
    if (res.hasError) {
      console.log(res.error);
      return;
    }
    console.log(res.value);
  }

  function handleOnChange(event: React.FormEvent<HTMLInputElement>): void {
    const target = event.target as HTMLInputElement & {
      files: FileList;
    };
    setFile(target.files[0]);
    const file = new FileReader();
    file.onload = function () {
      setPreview(file.result as string);
    };
    file.readAsDataURL(target.files[0]);
  }

  return (
    <div>
      <Modal isOpen={isOpen} onOpenChange={onOpenChange} isDismissable={false}>
        <ModalContent>
          <ModalHeader className='flex flex-col gap-1'>Upload Image</ModalHeader>
          <div className='p-10'>
            <form onSubmit={handleSubmitImage}>
              <input type='file' onChange={handleOnChange} />
              <img src={preview} alt='' />
              <Button type='submit'>Upload</Button>
            </form>
          </div>
        </ModalContent>
      </Modal>
    </div>
  );
}

export default UploadUserImageModalForm;
