import styles from "./PostItem.module.css";
import TimeAgo from "javascript-time-ago";
import en from "javascript-time-ago/locale/en";
import { useState } from "react";
import CommentBox from "../../comments/CommentBox/CommentBox";
import { NavLink } from "react-router-dom";
import { Post } from "../../../api/postService";

TimeAgo.addLocale(en);

interface Props {
  post: Post;
}

function PostItem({ post }: Props) {
  const [openCommentBox, setOpenCommentBox] = useState<boolean>(false);
  const timeAgo = new TimeAgo("en-US");

  function formatLike(likeNum: number): string {
    return `${likeNum} ${likeNum === 1 ? "like" : "likes"}`;
  }

  function handleOpenCommentBox(): void {
    setOpenCommentBox((curr) => !curr);
  }

  return (
    <div className={styles.post}>
      <div className={styles.userInfo}>
        <img
          src={`${post.userInfo.avatarUrl}`}
          className={styles.userInfoImg}
        />
        <div className={styles.container}>
          <NavLink to={`profile/${post.userInfo.username}`}>
            {post.userInfo.username}
          </NavLink>
          <span className={styles.createdAgo}>
            {timeAgo.format(new Date(post.createdAt).getTime())}
          </span>
        </div>
      </div>
      <span>{post.contents}</span>
      <div className={styles.image}>
        <img
          src={`http://localhost:5167/${post.imageUrl}`}
          className={styles.postImage}
        />
      </div>
      <div className={styles.likes}>
        <span>{formatLike(post.numLikes)}</span>
      </div>
      <div className={styles.actions}>
        <div className={styles.btnAction}>Like</div>
        <div className={styles.btnAction} onClick={handleOpenCommentBox}>
          Comment
        </div>
        {openCommentBox && <CommentBox postId={post.id} />}
      </div>
    </div>
  );
}

export default PostItem;
