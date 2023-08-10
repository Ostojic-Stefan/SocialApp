import { CommentResponse } from "../../../api/commentService";

interface Props {
  comment: CommentResponse;
}

function CommentItem({ comment }: Props) {
  return <li>{comment.contents}</li>;
}

export default CommentItem;
