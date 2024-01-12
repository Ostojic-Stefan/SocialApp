import { useParams } from 'react-router-dom';
import { useEffect } from 'react';
import { useAppDispatch, useAppSelector } from '../store';

function Post() {
  const { postId } = useParams();
  const dispatch = useAppDispatch();

  // const post = useAppSelector((store) => {
  //   return store.post.posts.find((p) => p.id === postId);
  // });

  // useEffect(() => {
  //   if (!post) {
  //     console.log('dispatching...');
  //     dispatch(getPosts());
  //   }
  // }, [dispatch]);

  // if (!post) return <h1>Loading...</h1>;

  return (
    <>
      <h1>Post</h1>
    </>
  );
}

export default Post;
