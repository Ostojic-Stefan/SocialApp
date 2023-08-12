import { useEffect, useState } from "react";
import CommentItem from "./CommentItem";
import { useAppDispatch, useAppSelector } from "../../../store";
import { addCommentToAPost, getCommentsByPostId } from "../commentsSlice";
import { PostResponse } from "../../../api/postService";

interface Props {
  post: PostResponse;
}

function CommentBox({ post }: Props) {
  const [comment, setComment] = useState("");
  const dispatch = useAppDispatch();
  const comments = useAppSelector(
    (store) => store.comment.data.find((d) => d.postId == post.id)?.comments
  );

  useEffect(() => {
    dispatch(getCommentsByPostId(post.id));
  }, []);

  function submitComment(): void {
    dispatch(addCommentToAPost({ contents: comment, postId: post.id }));
  }

  return (
    <div>
      <div>
        <input
          type="text"
          placeholder="write a comment..."
          value={comment}
          onChange={(e) => setComment(e.target.value)}
        />
        <button onClick={submitComment}>submit comment</button>
      </div>
      <ul>
        {comments &&
          comments.map((comment) => (
            <CommentItem key={comment.id} comment={comment} />
          ))}
      </ul>
    </div>
  );
}

export default CommentBox;
