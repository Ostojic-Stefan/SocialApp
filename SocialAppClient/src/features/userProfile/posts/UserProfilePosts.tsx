import { useEffect } from "react";
import { useAppDispatch, useAppSelector } from "../../../store";
import { useParams } from "react-router-dom";
import { getPostsForUser } from "../userProfileSlice";
import PostItem from "../../posts/PostItem/PostItem";

function UserProfilePosts() {
  const dispatch = useAppDispatch();
  const { username } = useParams();
  // const userPosts = useAppSelector((store) => store.user.posts);

  useEffect(() => {
    if (username) {
      dispatch(getPostsForUser({ username: username }));
    }
  }, []);

  return (
    <ul>
      {/* {userPosts.map((post) => (
        <PostItem key={post.id} post={post} />
        // <li key={post.id}>{post.contents}</li>
      ))} */}
    </ul>
  );
}

export default UserProfilePosts;
