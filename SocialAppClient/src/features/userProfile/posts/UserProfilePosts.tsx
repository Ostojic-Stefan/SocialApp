import { useEffect } from "react";
import { useAppDispatch, useAppSelector } from "../../../store";
import { useParams } from "react-router-dom";
import PostList from "../../posts/PostList/PostList";
import { getPostsForUser, postsForUserSelector } from "../../posts/postSlice";

function UserProfilePosts() {
  const dispatch = useAppDispatch();
  const { username } = useParams();

  const userPosts = useAppSelector((store) => postsForUserSelector(store, username!));

  useEffect(() => {
    if (username) {
      dispatch(getPostsForUser({ username }));
    }
  }, []);

  if (!userPosts) {
    return <h1>Loading...</h1>
  }

  return (
    <>
      <PostList posts={userPosts} />
    </>
  );
}

export default UserProfilePosts;
