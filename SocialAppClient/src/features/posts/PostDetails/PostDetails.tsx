import { useEffect, useState } from "react";
import CommentItem from "../../comments/CommentItem/CommentItem";
import { RootState, useAppDispatch, useAppSelector } from "../../../store";
import { addCommentToAPost, getCommentsByPostId } from "../../comments/commentsSlice";
import styles from "./PostDetails.module.css"
import { NavLink } from "react-router-dom";
import { createSelector } from "@reduxjs/toolkit";
import Button from "../../../ui/components/Button/Button";
import { Post } from "../types";

interface Props {
  post: Post;
}

export const selectNumLikesAndComments = (postId: string) =>
  createSelector(
    [(state: RootState) => state.post.posts.find((p) => p.id === postId)],
    (foundPost) => ({
      numLikes: foundPost?.numLikes,
      numComments: foundPost?.numComments,
    })
  );

function PostDetails({ post }: Props) {
  const [comment, setComment] = useState("");
  const dispatch = useAppDispatch();

  const comments = useAppSelector(
    (store) => store.comment.data.find((d) => d.postId === post.id)?.comments
  );

  const { numLikes, numComments } = useAppSelector(
    selectNumLikesAndComments(post.id)
  );

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
            <h4>{post.userInfo.username}</h4>
          </NavLink>
          <span className={styles.createdAgo}>
            {/* {timeAgo.format(new Date(post.createdAt).getTime())} */}
          </span>
        </div>
      </div>

      <h1 className={styles.postContents}>{post.contents}</h1>

      <img
        src={post.imageUrl}
        className={styles.postImage}
      />
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
      <div className={styles.inputContainer}>
        <textarea
          className={styles.textArea}
          placeholder="write a comment..."
          value={comment}
          onChange={(e) => setComment(e.target.value)}
        />
        <Button style={{ width: "fit-content" }} onClick={submitComment}>submit comment</Button>  
      </div>
      
    </div>
  );
}

export default PostDetails;
