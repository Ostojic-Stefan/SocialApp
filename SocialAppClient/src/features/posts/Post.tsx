import { useParams } from "react-router-dom";
import { useEffect } from "react";
import PostDetails from "./PostDetails/PostDetails";
import { getPosts } from "./postSlice";
import { useAppDispatch, useAppSelector } from "../../store";

function Post() {
    const { postId } = useParams();
    const dispatch = useAppDispatch();

    const post = useAppSelector((store) => {
      return store.post.posts.find(p => p.id === postId);
    });
  
    useEffect(() => {
        if (!post) {
            console.log("dispatching...");
            dispatch(getPosts());
        }
    }, [dispatch]);

    if (!post)
        return <h1>Loading...</h1>

    return (
        <>
            <PostDetails post={post} />
        </>
    );
}

export default Post
