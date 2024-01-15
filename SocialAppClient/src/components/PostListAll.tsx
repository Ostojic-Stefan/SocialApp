import PostList from './PostList';
import { Spinner } from '@nextui-org/react';
import { useAppDispatch, useAppSelector } from '../store';
import { useEffect } from 'react';
import { getAllPosts } from '../store/post-slice';

export default function PostListAll() {
  const { allPosts, postsLoading, allPostsError } = useAppSelector((store) => store.post);
  const dispatch = useAppDispatch();

  useEffect(() => {
    dispatch(getAllPosts());
  }, []);

  if (postsLoading) {
    return <Spinner color='secondary' />;
  }

  if (allPostsError) {
    return <p className='text-danger'>{allPostsError}</p>;
  }

  return <PostList posts={allPosts} />;
}
