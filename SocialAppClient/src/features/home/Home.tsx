import { useEffect } from "react";
import Modal from "../../components/Modal";
import { useAppDispatch, useAppSelector } from "../../store";
import Button from "../../ui/components/Button/Button";
import CreatePostForm from "../posts/CreatePostForm/CreatePostForm";
import PostList from "../posts/PostList/PostList";
import styles from './Home.module.css';
import { getPosts } from "../posts/postSlice";

function Home() {
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
      <Modal>
        <Modal.Open>
          <div className={styles.createPostContainer}>
            <h2>Create your own post here:</h2>
            <Button>Create A Post</Button>
          </div>
        </Modal.Open>
        <Modal.Content>
          <CreatePostForm />
        </Modal.Content>
      </Modal>
      <PostList posts={posts}/>
    </>
  );
}

export default Home;
