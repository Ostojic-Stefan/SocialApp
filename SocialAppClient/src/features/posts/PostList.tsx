import { useEffect, useState } from "react";
import PostItem from "./PostItem";
import axios from "axios";
import { GetAllPostsResponse, Post } from "./types";

function PostList() {
  const [posts, setPosts] = useState<Post[]>([]);

  useEffect(() => {
    axios
      .get<GetAllPostsResponse>("https://localhost:7113/api/Posts")
      .then((response) => {
        setPosts(response.data.items);
      })
      .catch((err) => console.log(err));
  }, []);

  if (!posts.length) return <div>Loading...</div>;

  return (
    <>
      {posts.map((post) => (
        <PostItem key={post.id} post={post} />
      ))}
    </>
  );
}

export default PostList;
