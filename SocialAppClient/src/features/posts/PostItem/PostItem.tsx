import styles from "./PostItem.module.css";
import TimeAgo from "javascript-time-ago";
import en from "javascript-time-ago/locale/en";
import CommentBox from "../../comments/CommentBox/CommentBox";
import { NavLink } from "react-router-dom";
import { PostResponse } from "../../../api/postService";
import { useAppDispatch } from "../../../store";
import { LikeReaction } from "../../../api/likeService";
import LikeButton from "../LikeButton/LikeButton";
import Modal from "../../../components/Modal";
import LikeList from "../../likes/LikeList/LikeList";
import { deleteLike, likePost } from "../../likes/likeSlice";
import { getPostById } from "../postSlice";

TimeAgo.addLocale(en);

interface Props {
  post: PostResponse;
}

function PostItem({ post }: Props) {
  const dispatch = useAppDispatch();

  const timeAgo = new TimeAgo("en-US");

  function formatLike(likeNum: number): string {
    return `${likeNum} ${likeNum === 1 ? "like" : "likes"}`;
  }

  async function handleLikeClick(reaction: LikeReaction): Promise<void> {
    await dispatch(likePost({ postId: post.id, reaction }));
    await dispatch(getPostById(post.id));
  }

  async function handleUnlike(): Promise<void> {
    await dispatch(deleteLike({ likeId: post.likeInfo?.likeId! }));
    await dispatch(getPostById(post.id));
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
      <Modal>
        <Modal.Open>
          <div className={styles.likes}>
            <span>{formatLike(post.numLikes)}</span>
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
