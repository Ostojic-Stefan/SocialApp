import CreatePostForm from "../features/posts/CreatePostForm";
import PostList from "../features/posts/PostList";

function Home() {
  return (
    <>
      <CreatePostForm />
      <PostList />
    </>
  );
}

export default Home;
