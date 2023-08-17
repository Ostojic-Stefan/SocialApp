import { useEffect, useState } from "react";
import CommentItem from "../CommentItem/CommentItem";
import { RootState, useAppDispatch, useAppSelector } from "../../../store";
import { addCommentToAPost, getCommentsByPostId } from "../commentsSlice";
import { PostResponse } from "../../../api/postService";
import styles from "./styles.module.css";
import { NavLink } from "react-router-dom";
import { createSelector } from "@reduxjs/toolkit";

interface Props {
  post: PostResponse;
}

export const selectNumLikesAndComments = (postId: string) =>
  createSelector(
    (state: RootState) => state.post.posts.find((p) => p.id === postId),
    (foundPost) => ({
      numLikes: foundPost?.numLikes,
      numComments: foundPost?.numComments,
    })
  );

function CommentBox({ post }: Props) {
  const [comment, setComment] = useState("");
  const dispatch = useAppDispatch();

  const comments = useAppSelector(
    (store) => store.comment.data.find((d) => d.postId == post.id)?.comments
  );

  const { numLikes, numComments } = useAppSelector(
    selectNumLikesAndComments(post.id)
  );

  // const { numLikes, numComments } = useAppSelector((store) => {
  //   const foundPost = store.post.posts.find((p) => p.id === post.id);
  //   return {
  //     numLikes: foundPost?.numLikes!,
  //     numComments: foundPost?.numComments!,
  //   };
  // });

  useEffect(() => {
    dispatch(getCommentsByPostId(post.id));
  }, []);

  function submitComment(): void {
    dispatch(addCommentToAPost({ contents: comment, postId: post.id }));
    setComment("");
  }

  return (
    <div className={styles.container}>
      <div className={styles.userInfo}>
        <img
          src={`${post.userInfo.avatarUrl}`}
          className={styles.userInfoImg}
        />
        <div>
          <NavLink to={`profile/${post.userInfo.username}`}>
            {post.userInfo.username}
          </NavLink>
          <span className={styles.createdAgo}>
            {/* {timeAgo.format(new Date(post.createdAt).getTime())} */}
          </span>
        </div>
      </div>

      <div className={styles.postContents}>{post.contents}</div>

      <div className={styles.image}>
        <img
          src={post.imageUrl}
          className={styles.postImage}
        />
      </div>
      <div className={styles.stats}>
        <span>Likes: {numLikes}</span>
        <span>Comments: {numComments}</span>
      </div>
      <div className={styles.commentsContainer}>
        {comments &&
          comments.map((comment) => (
            <CommentItem key={comment.id} comment={comment} />
          ))}
      </div>
      <input
        type="text"
        placeholder="write a comment..."
        value={comment}
        onChange={(e) => setComment(e.target.value)}
      />
      <button onClick={submitComment}>submit comment</button>
    </div>
  );
}

export default CommentBox;
