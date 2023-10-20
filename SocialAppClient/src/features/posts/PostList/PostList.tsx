import { useNavigate } from "react-router-dom";
import { Post } from "../types";
import PostItem from "../PostItem/PostItem";

interface Props {
  posts: Post[];
}

function PostList({ posts }: Props) {
  const navigate = useNavigate();

  return (
    <>
      {posts.map((post) => (
        <PostItem 
          key={post.id} post={post} 
          onClick={() => navigate(`post/${post.id}`)}/>
      ))}
    </>
  );
}

export default PostList;
