import styles from "./PostItem.module.css";
import TimeAgo from "javascript-time-ago";
import en from "javascript-time-ago/locale/en";
import CommentBox from "../../comments/CommentBox/CommentBox";
import { NavLink } from "react-router-dom";
import { PostResponse } from "../../../api/postService";
import { useAppDispatch, useAppSelector } from "../../../store";
import { LikeReaction } from "../../../api/likeService";
import LikeButton from "../LikeButton/LikeButton";
import Modal from "../../../components/Modal";
import LikeList from "../../likes/LikeList/LikeList";
import { deleteLike, getLikesForPost, likePost } from "../../likes/likeSlice";
import { useEffect } from "react";

TimeAgo.addLocale(en);

interface Props {
  post: PostResponse;
}

function PostItem({ post }: Props) {
  const dispatch = useAppDispatch();
  const numLikes = useAppSelector((store) => {
    const likes = store.like.likes.find((l) => l.postId === post.id)?.likeInfo;
    return likes?.length;
  });

  const timeAgo = new TimeAgo("en-US");

  useEffect(() => {
    dispatch(getLikesForPost({ postId: post.id }));
  }, []);

  function formatLike(likeNum: number): string {
    return `${likeNum} ${likeNum === 1 ? "like" : "likes"}`;
  }

  async function handleLikeClick(reaction: LikeReaction): Promise<void> {
    await dispatch(likePost({ postId: post.id, reaction }));
    await dispatch(getLikesForPost({ postId: post.id }));
  }

  async function handleUnlike(): Promise<void> {
    await dispatch(deleteLike({ likeId: post.likeInfo?.likeId! }));
    await dispatch(getLikesForPost({ postId: post.id }));
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
          src={post.imageUrl}
          className={styles.postImage}
        />
      </div>
      <Modal>
        <Modal.Open>
          <div className={styles.likes}>
            <span>{numLikes && formatLike(numLikes)}</span>
          </div>
        </Modal.Open>
        <Modal.Content>
          <LikeList postId={post.id} />
        </Modal.Content>
      </Modal>
      <div className={styles.actions}>
        {!post.likeInfo ? (
          <LikeButton post={post} onClick={handleLikeClick} />
        ) : (
          <button onClick={handleUnlike}>UnLike</button>
        )}

        <Modal>
          <Modal.Open>
            <div className={styles.btnAction}>
              Comments ({post.numComments})
            </div>
          </Modal.Open>
          <Modal.Content>
            <CommentBox post={post} />
          </Modal.Content>
        </Modal>
      </div>
    </div>
  );
}

export default PostItem;
