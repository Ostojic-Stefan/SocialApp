import { Button, useDisclosure } from '@nextui-org/react';
import { Link } from 'react-router-dom';
import CreatePostModal from './CreatePostModal';

export default function HomeNav() {
  const { onOpen, isOpen, onOpenChange } = useDisclosure();

  return (
    <div>
      <div className='flex justify-between px-6 py-4'>
        <div className='flex gap-4'>
          <Button as={Link} to={'friend-posts'} className='text-md font-semibold' variant='flat' color='secondary'>
            Friends Only
          </Button>
          <Button as={Link} to={'all-posts'} className='text-md font-semibold' variant='flat' color='secondary'>
            All Posts
          </Button>
        </div>
        <div>
          <CreatePostModal isOpen={isOpen} onOpenChange={onOpenChange} />
          <Button onPress={onOpen} className='text-md font-semibold' variant='flat' color='secondary'>
            Create a Post
          </Button>
        </div>
      </div>
    </div>
  );
}
