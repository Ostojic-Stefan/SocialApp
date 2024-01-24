import PostList from './PostList';
import { Spinner } from '@nextui-org/react';
import { useAppDispatch, useAppSelector } from '../store';
import { useEffect } from 'react';
import { getAllPosts } from '../store/post-slice';

export default function PostListAll() {
  const { allPosts, postsLoading, allPostsError } = useAppSelector((store) => store.post);
  const dispatch = useAppDispatch();

  console.log('RELOAD');

  useEffect(() => {
    dispatch(getAllPosts());
  }, []);

  if (postsLoading) {
    return <Spinner color='secondary' />;
  }

  if (allPostsError) {
    return <p className='text-danger'>{allPostsError}</p>;
  }

  return (
    <div
    // style={{
    //   height: '80vh',
    // }}
    // className='w-full overflow-y-scroll'
    >
      <PostList posts={allPosts} />
    </div>
  );
}
