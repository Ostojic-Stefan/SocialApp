import { useEffect } from "react";
import CreatePostForm from "../features/posts/CreatePostForm";
import PostList from "../features/posts/PostList";
import { getUserInformation, login } from "../features/user/userSlice";
import { useAppDispatch, useAppSelector } from "../store";

function Home() {
  // useAppSelector(store => store.user);
  const dispatch = useAppDispatch();

  useEffect(() => {
    // dispatch(login({ email: "string", password: "string" }));
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
