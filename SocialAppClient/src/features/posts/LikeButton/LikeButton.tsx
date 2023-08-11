import { useEffect, useRef, useState } from "react";
import LikeReactions, {
  OnMouseOverArgs,
} from "../../../ui/components/LikeReactions";
import styles from "./LikeButton.module.css";

function LikeButton() {
  const ref = useRef<HTMLButtonElement>(null);
  const [isHovered, setIsHovered] = useState(false);
  const [isInside, setIsInside] = useState(false);
  const [position, setPosition] = useState({ x: 0, y: 0 });

  useEffect(() => {
    function showEmojis() {
      const rect = ref.current?.closest("button")?.getBoundingClientRect();
      if (rect) {
        setPosition({ x: rect.width / 2, y: 0 });
      }
      setIsHovered(true);
    }
    function hideEmojis() {
      setIsHovered(false);
    }
    ref.current?.addEventListener("mouseenter", showEmojis);
    ref.current?.addEventListener("mouseleave", hideEmojis);
    return () => {
      ref.current?.removeEventListener("mouseenter", showEmojis);
      ref.current?.removeEventListener("mouseleave", hideEmojis);
    };
  }, [ref]);

  function onMouseOver(args: OnMouseOverArgs): void {
    setIsInside(args.isInside);
  }

  return (
    <div style={{ position: "relative" }}>
      {(isHovered || isInside) && (
        <LikeReactions position={position} onMouseOver={onMouseOver} />
      )}
      <button ref={ref} className={styles.btnAction}>
        Like
      </button>
    </div>
  );
}

export default LikeButton;
