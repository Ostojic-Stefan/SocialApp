import styles from "./PostItem.module.css";
import TimeAgo from "javascript-time-ago";
import en from "javascript-time-ago/locale/en";
import { Post } from "./types";

TimeAgo.addLocale(en);

interface Props {
  post: Post;
}

function PostItem({ post }: Props) {
  const timeAgo = new TimeAgo("en-US");

  return (
    <div className={styles.post}>
      <div className={styles.userInfo}>
        <img
          src={`${post.userInfo.avatarUrl}`}
          className={styles.userInfoImg}
        />
        <div className={styles.container}>
          <span>{post.userInfo.username}</span>
          <span className={styles.createdAgo}>
            {timeAgo.format(new Date(post.createdAt).getTime())}
          </span>
        </div>
      </div>
      <span>{post.contents}</span>
      <div className={styles.image}>
        <img src={`${post.imageUrl}`} className={styles.postImage} />
      </div>
      <div className={styles.likes}>
        <span>1K</span>
      </div>
      <div className={styles.actions}>
        <div className={styles.btnAction}>Like</div>
        <div className={styles.btnAction}>Comment</div>
      </div>
    </div>
  );
}

export default PostItem;
