import styles from "./PostItem.module.css";
import TimeAgo from "javascript-time-ago";
import en from "javascript-time-ago/locale/en";
import { useState } from "react";
import CommentBox from "../../comments/CommentBox/CommentBox";
import { NavLink } from "react-router-dom";
import { PostResponse } from "../../../api/postService";
import { useAppDispatch } from "../../../store";
import { deleteLike, likePost } from "../postSlice";
import { LikeReaction } from "../../../api/likeService";
import LikeButton from "../LikeButton/LikeButton";

TimeAgo.addLocale(en);

interface Props {
  post: PostResponse;
}

function PostItem({ post }: Props) {
  const dispatch = useAppDispatch();
  const [openCommentBox, setOpenCommentBox] = useState<boolean>(false);
  const timeAgo = new TimeAgo("en-US");

  function formatLike(likeNum: number): string {
    return `${likeNum} ${likeNum === 1 ? "like" : "likes"}`;
  }

  function handleOpenCommentBox(): void {
    setOpenCommentBox((curr) => !curr);
  }

  function handleLikeClick(reaction: LikeReaction): void {
    dispatch(likePost({ postId: post.id, reaction }));
  }

  function handleUnlike(): void {
    dispatch(deleteLike({ likeId: post.likeInfo?.likeId! }));
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
        {!post.likeInfo ? (
          <LikeButton post={post} onClick={handleLikeClick} />
        ) : (
          <button onClick={handleUnlike}>UnLike</button>
        )}

        <div className={styles.btnAction} onClick={handleOpenCommentBox}>
          Comments ({post.numComments})
        </div>

        {openCommentBox && <CommentBox post={post} />}
      </div>
    </div>
  );
}

export default PostItem;
