import { Link } from 'react-router-dom';
import { Button, Image } from '@nextui-org/react';
import { PostResponse } from '../api/dtos/post';

interface PostPreviewProps {
  postData: PostResponse;
}

export default function PostPreview({ postData }: PostPreviewProps) {
  return (
    <div className='flex flex-col shadow bg-default-50 px-4 pb-4 w-3/5 overflow-hidden hover:cursor-pointer hover:bg-default-100'>
      <div className='flex w-full items-center gap-4 h-36'>
        <Image
          src={postData.images[0].thumbnailImagePath}
          alt='Placeholder'
          width='100'
          height='100'
          className='aspect-square object-cover w-24 h-24 rounded-md'
        />
        <div className='space-y-2'>
          <h3 className='tracking-tight text-lg font-semibold'>Card Title</h3>
          <p className='text-sm text-gray-500 dark:text-gray-400'>{postData.contents}</p>
        </div>
      </div>
      <div className='flex justify-between'>
        <div className='flex gap-5'>
          <span>{postData.numLikes} likes</span>
          <span>{postData.numComments} comments</span>
        </div>
        <Button color='primary' variant='light' as={Link} to={`/post/${postData.id}`}>
          Visit Post &rarr;
        </Button>
      </div>
    </div>
  );
}
