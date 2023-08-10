import PostList from "../posts/PostList/PostList";
import CreatePostForm from "../posts/CreatePostForm/CreatePostForm";

function Home() {
  return (
    <>
      <CreatePostForm />
      <PostList />
    </>
  );
}

export default Home;
