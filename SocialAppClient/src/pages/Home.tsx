import PostList from "../features/posts/PostList/PostList";
import CreatePostForm from "../features/posts/CreatePostForm/CreatePostForm";

function Home() {
  return (
    <>
      <CreatePostForm />
      <PostList />
    </>
  );
}

export default Home;
