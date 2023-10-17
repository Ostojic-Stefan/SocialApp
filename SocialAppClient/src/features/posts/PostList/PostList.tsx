import { useEffect } from "react";
import { getPosts } from "../postSlice";
import { useAppDispatch, useAppSelector } from "../../../store";
import PostItem from "../PostItem/PostItem";
import Modal from "../../../components/Modal";
import CreatePostForm from "../CreatePostForm/CreatePostForm";
import styles from './PostList.module.css';
import Button from "../../../ui/components/Button/Button";

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
      {posts.map((post) => (
        <PostItem key={post.id} post={post} />
      ))}
    </>
  );
}

export default PostList;
