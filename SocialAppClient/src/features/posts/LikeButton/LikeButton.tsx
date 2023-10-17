import { useRef, useState } from "react";
import { OnMouseOverArgs } from "../../../components/Popup";
import styles from "./LikeButton.module.css";
import Popup from "../../../components/Popup";
import LikeReactions from "../../../ui/components/LikeReactions";
import { LikeReaction } from "../../../api/likeService";
import { PostResponse } from "../../../api/postService";
import Button from "../../../ui/components/Button/Button";

interface Props {
  post: PostResponse;
  onClick: (reaction: LikeReaction) => void;
}

function LikeButton({ onClick }: Props) {
  const ref = useRef<HTMLButtonElement>(null);
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
    onClick(reaction);
  }

  return (
    <div style={{ position: "relative" }}>
      {(isHovered || isInsidePopup) && (
        <Popup position={position} onMouseOver={onMouseOver}>
          <LikeReactions handleLikeClick={handleLikeClick} />
        </Popup>
      )}
      <Button 
        ref={ref}
        onMouseEnter={showPopup}
        onMouseLeave={hidePopup}>Like</Button>
      {/* <button
        ref={ref}
        onMouseEnter={showPopup}
        onMouseLeave={hidePopup}
        className={styles.btnAction}
      >
        Like
      </button> */}
    </div>
  );
}

export default LikeButton;
