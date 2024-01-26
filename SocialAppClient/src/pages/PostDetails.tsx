import { FormEvent, useEffect, useState } from 'react';
import { postService } from '../api/postService';
import { useParams } from 'react-router-dom';
import { Textarea, Image, Spinner, Divider } from '@nextui-org/react';
import TimeAgo from 'timeago-react';
import { commentService } from '../api/commentService';
import CommentList from '../components/CommentList';
import { PostDetailsResponse } from '../api/dtos/post';
import UserInfo from '../components/UserInfo';

export default function PostDetails() {
  const { postId } = useParams();
  const [postDetails, setPostDetails] = useState<PostDetailsResponse | null>(null);
  const [commentContents, setCommentContents] = useState<string>('');

  useEffect(() => {
    async function getPostDetails() {
      if (!postId) {
        console.log('postId must be present');
        return;
      }
      const response = await postService.getPostDetails(postId);
      if (response.hasError) {
        console.log(response.error);
        return;
      }
      setPostDetails(response.value);
    }
    getPostDetails();
  }, []);

  if (!postDetails) {
    return <Spinner />;
  }

  async function handleSubmitComment(event: FormEvent<HTMLFormElement>): Promise<void> {
    event.preventDefault();
    if (!postDetails) return;
    const response = await commentService.addCommentToAPost({ contents: commentContents, postId: postDetails.id });
    if (response.hasError) {
      return;
    }
    // TODO: not sure if it's the best approach
    const postDetailsResponse = await postService.getPostDetails(postId!);
    if (postDetailsResponse.hasError) {
      console.log(postDetailsResponse.error);
      return;
    }
    setPostDetails(postDetailsResponse.value);
    setCommentContents('');
  }

  return (
    <main className='flex' style={{ height: '80vh' }}>
      <div className='bg-default-800 p-9 flex items-center justify-center'>
        <Image
          alt='Post Image'
          className='w-full object-cover rounded-lg'
          src={postDetails.images[0].fullscreenImagePath}
          style={{
            objectFit: 'cover',
          }}
          width={800}
        />
      </div>

      <div className='flex-grow bg-default-50 p-5 flex flex-col justify-between gap-6'>
        <div className='flex flex-col h-full'>
          <section>
            <div>
              <UserInfo userInfo={postDetails.userInfo}>
                <TimeAgo datetime={postDetails.createdAt} locale='en_us' />
              </UserInfo>
            </div>
            <p>{postDetails.contents}</p>
          </section>

          <section className='my-4 flex gap-5'>
            <span className='px-2 py-1 bg-secondary-200 rounded'>{postDetails.numLikes} Likes</span>
            <span className='px-2 py-1 bg-primary-200 rounded'>{postDetails.numComments} comments</span>
          </section>

          <Divider />

          <h1 className='text-2xl mb-5'>Comments</h1>
          <div className='flex-grow mt-5 overflow-y-scroll'>
            <CommentList comments={postDetails.comments} />
          </div>
          <section>
            <form onSubmit={handleSubmitComment} className='mt-8 space-y-6'>
              <div className='rounded-md shadow-sm'>
                <Textarea
                  onChange={(e) => setCommentContents(e.target.value)}
                  value={commentContents}
                  placeholder='Add a comment...'
                  id='comment'
                  name='comment'
                  className='appearance-none rounded-none relative block w-full placeholder-gray-500
                    text-gray-900 rounded-t-md focus:outline-none focus:ring-indigo-500
                    focus:border-indigo-500 focus:z-10 sm:text-sm'
                />
              </div>
              <button
                className='group relative w-full flex justify-center py-2 px-4
                  border border-transparent text-sm font-medium rounded-md text-white bg-secondary
                  hover:bg-secondary-600 focus:outline-none focus:ring-2
                  focus:ring-offset-2 '
                type='submit'
              >
                Submit
              </button>
            </form>
          </section>
        </div>
      </div>
    </main>
  );
}
