import { Modal, ModalContent, ModalHeader, ModalBody, Textarea, Button, ModalFooter } from '@nextui-org/react';
import { useState, FormEvent } from 'react';
import { useAppDispatch } from '../store';
import { uploadPost } from '../store/post-slice';

interface CreatePostModalProps {
  isOpen: boolean;
  onOpenChange: () => void;
}

export default function CreatePostForm({ isOpen, onOpenChange }: CreatePostModalProps) {
  const [file, setFile] = useState<File | undefined>(undefined);
  const [preview, setPreview] = useState<string | undefined>(undefined);
  const [contents, setContents] = useState<string>('');

  const dispatch = useAppDispatch();

  async function handleSubmit(event: FormEvent<HTMLFormElement>): Promise<void> {
    event.preventDefault();
    if (!file) {
      console.log('must select a file');
      return;
    }

    const formData = new FormData();
    formData.append('img', file);
    dispatch(uploadPost({ formData, contents }));
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
    <Modal isOpen={isOpen} onOpenChange={onOpenChange} isDismissable={false}>
      <ModalContent>
        {(onClose) => (
          <>
            <ModalHeader className='flex flex-col gap-1'>Create Post</ModalHeader>
            <ModalBody>
              <form onSubmit={handleSubmit}>
                <div>
                  <label htmlFor='image'>Image</label>
                  <input accept='image/jpg, image/png' type='file' name='image' onChange={handleOnChange} />
                  <img src={preview} alt='' />
                </div>
                <div>
                  <Textarea
                    placeholder='enter contents'
                    onChange={(e) => setContents(e.target.value)}
                    value={contents}
                  />
                </div>
                <Button type='submit' color='primary'>
                  Submit
                </Button>
              </form>
            </ModalBody>
            <ModalFooter>
              <Button color='danger' variant='light' onPress={onClose}>
                Close
              </Button>
            </ModalFooter>
          </>
        )}
      </ModalContent>
    </Modal>
  );
}
