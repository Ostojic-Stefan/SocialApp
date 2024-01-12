import {
  Button,
  Modal,
  ModalBody,
  ModalContent,
  ModalFooter,
  ModalHeader,
  Textarea,
  useDisclosure,
} from '@nextui-org/react';
import { Link } from 'react-router-dom';
import { FormEvent, useState } from 'react';
import { useAppDispatch } from '../store';
import { uploadPost } from '../store/post-slice';

export default function HomeNav() {
  const { isOpen, onOpen, onOpenChange } = useDisclosure();
  const [file, setFile] = useState<File | undefined>(undefined);
  const [preview, setPreview] = useState<string | undefined>(undefined);
  const [contents, setContents] = useState<string>('');

  const dispatch = useAppDispatch();

  async function handleSubmit(event: FormEvent<HTMLFormElement>): Promise<void> {
    event.preventDefault();
    if (!file) {
      console.log('must select file');
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
    <div>
      <div className='flex justify-between shadow-md border-b-2 border-secondary-300  px-6 py-4'>
        <div className='flex gap-4'>
          <Link to={'friend-posts'}>
            <Button className='text-md font-semibold' variant='flat' color='secondary'>
              Friends Only
            </Button>
          </Link>
          <Link to={'all-posts'}>
            <Button className='text-md font-semibold' variant='flat' color='secondary'>
              All Posts
            </Button>
          </Link>
        </div>
        <div>
          <Button onPress={onOpen} className='text-md font-semibold' variant='flat' color='secondary'>
            Create a Post
          </Button>
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
        </div>
      </div>
    </div>
  );
}
