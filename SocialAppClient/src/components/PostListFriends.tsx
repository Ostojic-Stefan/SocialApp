import { useEffect } from 'react';
import { useAppDispatch, useAppSelector } from '../store';
import { getPostsFromFriends } from '../store/post-slice';
import { Spinner } from '@nextui-org/react';
import PostList from './PostList';

export default function PostListFriends() {
  const dispatch = useAppDispatch();
  const posts = useAppSelector((store) => store.post.postsFromFriends);

  useEffect(() => {
    dispatch(getPostsFromFriends());
  }, []);

  if (!posts) {
    return <Spinner />;
  }

  return <PostList posts={posts} />;
}
