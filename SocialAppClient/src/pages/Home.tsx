import { useEffect } from "react";
import PostList from "../features/posts/PostList/PostList";
import { getUserInformation } from "../features/user/userSlice";
import { useAppDispatch } from "../store";
import CreatePostForm from "../features/posts/CreatePostForm/CreatePostForm";

function Home() {
  const dispatch = useAppDispatch();

  useEffect(() => {
    dispatch(getUserInformation());
  }, []);

  return (
    <>
      <CreatePostForm />
      <PostList />
    </>
  );
}

export default Home;
