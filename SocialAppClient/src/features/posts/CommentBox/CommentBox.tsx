import { useEffect, useState } from "react";
import { apiHandler } from "../../../api/apiHandler";
import { CommentResponse } from "../../user/types";

interface Props {
  postId: string;
}

function CommentBox({ postId }: Props) {
  const [comments, setComments] = useState<CommentResponse[]>([]);
  useEffect(() => {
    apiHandler.comment.getCommentsOnAPost(postId).then((res) => {
      setComments((curr) => [...curr, res]);
      console.log(res);
    });
  }, []);

  return (
    <div>
      {/* add a comment box */}
      <div>
        <input type="text" placeholder="write a comment..." />
      </div>
      {comments.map((c) => (
        <div key={c.id}>{c.contents}</div>
      ))}
    </div>
  );
}

export default CommentBox;
