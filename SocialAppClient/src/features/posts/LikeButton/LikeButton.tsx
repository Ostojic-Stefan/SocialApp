import { useRef, useState } from "react";
import { OnMouseOverArgs } from "../../../components/Popup";
import styles from "./LikeButton.module.css";
import Popup from "../../../components/Popup";
import LikeReactions from "../../../ui/components/LikeReactions";
import { LikeReaction } from "../../../api/likeService";
import { useAppDispatch } from "../../../store";
import { deleteLike, likePost } from "../postSlice";
import { PostResponse } from "../../../api/postService";

interface Props {
  post: PostResponse;
}

function LikeButton({ post }: Props) {
  const ref = useRef<HTMLButtonElement>(null);
  const dispatch = useAppDispatch();
  const [isHovered, setIsHovered] = useState(false);
  const [isInsidePopup, setIsInsidePopup] = useState(false);
  const [position, setPosition] = useState({ x: 0, y: 0 });

  function onMouseOver(args: OnMouseOverArgs): void {
    setIsInsidePopup(args.isInside);
  }

  function hidePopup(): void {
    setIsHovered(false);
  }

  function showPopup(): void {
    const rect = ref.current?.closest("button")?.getBoundingClientRect();
    if (rect) {
      setPosition({ x: rect.width / 2, y: 0 });
    }
    setIsHovered(true);
  }

  function handleLikeClick(reaction: LikeReaction): void {
    if (!post.likedByCurrentUser) {
      dispatch(likePost({ postId: post.id, reaction }));
    } else {
      // dispatch(deleteLike({ likeId: }));
    }
    console.log(reaction);
  }

  return (
    <div style={{ position: "relative" }}>
      {(isHovered || isInsidePopup) && (
        <Popup position={position} onMouseOver={onMouseOver}>
          <LikeReactions handleLikeClick={handleLikeClick} />
        </Popup>
      )}
      <button
        ref={ref}
        onMouseEnter={showPopup}
        onMouseLeave={hidePopup}
        className={styles.btnAction}
        style={{
          borderBottom: post.likedByCurrentUser ? "2px solid yellow" : "",
        }}
      >
        Like
      </button>
    </div>
  );
}

export default LikeButton;
