import { useEffect, useRef, useState } from "react";

interface LikeReactionProps {
  emoji: string;
}

function LikeReaction({ emoji }: LikeReactionProps) {
  return (
    <div
      style={{
        cursor: "pointer",
      }}
    >
      {emoji}
    </div>
  );
}

export type OnMouseOverArgs = {
  isInside: boolean;
};

interface Props {
  onMouseOver: (args: OnMouseOverArgs) => void;
  position: { x: number; y: number };
}

function LikeReactions({ onMouseOver, position }: Props) {
  const ref = useRef<HTMLDivElement>(null);
  const [dims, setDims] = useState({ width: 0, height: 0 });
  const emojis = ["👍", "💖", "😀", "😢", "😂"];

  useEffect(() => {
    const styles = window.getComputedStyle(ref.current!);

    setDims({
      width: parseFloat(styles.width),
      height: parseFloat(styles.height),
    });

    function mouseInside() {
      onMouseOver({ isInside: true });
    }
    function mouseOutside() {
      onMouseOver({ isInside: false });
    }
    ref.current?.addEventListener("mouseenter", mouseInside);
    ref.current?.addEventListener("mouseleave", mouseOutside);
    return () => {
      ref.current?.removeEventListener("mouseenter", mouseInside);
      ref.current?.removeEventListener("mouseleave", mouseOutside);
    };
  }, [ref]);

  return (
    <div
      ref={ref}
      style={{
        backgroundColor: "#333",
        borderRadius: "5px",
        padding: "10px 20px",
        display: "flex",
        gap: "10px",
        alignItems: "center",
        justifyContent: "center",
        right: position.x - dims.width / 2,
        top: position.y - dims.height + 10,
        position: "absolute",
        fontSize: 28,
      }}
    >
      {emojis.map((emoji, idx) => (
        <LikeReaction key={idx} emoji={emoji} />
      ))}
    </div>
  );
}

export default LikeReactions;
