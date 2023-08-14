import { CommentResponse } from "../../../api/commentService";
import styles from "./CommentItem.module.css";

interface Props {
  comment: CommentResponse;
}

function CommentItem({ comment }: Props) {
  return (
    <div className={styles.commentContainer}>
      <div className={styles.userInfo}>
        <img src={`${comment.avatarUrl}`} className={styles.userInfoImg} />
      </div>
      <div className={styles.main}>
        <p className={styles.commentUsername}>{comment.username}</p>
        <p>{comment.contents}</p>
      </div>
    </div>
  );
}

export default CommentItem;
