import { useEffect } from "react";
import { getPosts } from "../postSlice";
import { useAppDispatch, useAppSelector } from "../../../store";
import PostItem from "../PostItem/PostItem";

function PostList() {
  const dispatch = useAppDispatch();
  const { posts, isLoading } = useAppSelector((store) => {
    return store.post;
  });

  useEffect(() => {
    dispatch(getPosts());
  }, [dispatch]);

  if (isLoading) return <div>Loading...</div>;

  return (
    <>
      {posts.map((post) => (
        <PostItem key={post.id} post={post} />
      ))}
    </>
  );
}

export default PostList;
