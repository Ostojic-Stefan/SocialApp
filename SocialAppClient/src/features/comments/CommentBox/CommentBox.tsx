import { useEffect, useState } from "react";
import CommentItem from "./CommentItem";
import { useAppDispatch, useAppSelector } from "../../../store";
import { addCommentToAPost, getCommentsByPostId } from "../commentsSlice";

interface Props {
  postId: string;
}

function CommentBox({ postId }: Props) {
  const [comment, setComment] = useState("");
  const dispatch = useAppDispatch();
  const comments = useAppSelector(
    (store) => store.comment.data.find((d) => d.postId == postId)?.comments
  );

  useEffect(() => {
    dispatch(getCommentsByPostId(postId));
  }, []);

  function submitComment(): void {
    dispatch(addCommentToAPost({ contents: comment, postId }));
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
