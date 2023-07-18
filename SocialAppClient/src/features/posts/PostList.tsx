import { useEffect } from "react";
import PostItem from "./PostItem";
import { getPosts } from "./postSlice";
import { useAppDispatch, useAppSelector } from "../../store";

function PostList() {
  const dispatch = useAppDispatch();
  const { posts, isLoading } = useAppSelector((store) => store.post);

  useEffect(() => {
    dispatch(getPosts());
  }, [dispatch]);

  if (isLoading) return <div>Loading...</div>;

  console.log(posts);

  return (
    <>
      {posts.map((post) => (
        <PostItem key={post.id} post={post} />
      ))}
    </>
  );
}

export default PostList;
