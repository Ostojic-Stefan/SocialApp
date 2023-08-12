import { ReactNode, useEffect, useRef, useState } from "react";

export type OnMouseOverArgs = {
  isInside: boolean;
};

interface Props {
  onMouseOver: (args: OnMouseOverArgs) => void;
  position: { x: number; y: number };
  children: ReactNode;
}

function Popup({ onMouseOver, position, children }: Props) {
  const ref = useRef<HTMLDivElement>(null);
  const [dims, setDims] = useState({ width: 0, height: 0 });

  function mouseInside() {
    onMouseOver({ isInside: true });
  }

  function mouseOutside() {
    onMouseOver({ isInside: false });
  }

  useEffect(() => {
    const styles = window.getComputedStyle(ref.current!);
    setDims({
      width: parseFloat(styles.width),
      height: parseFloat(styles.height),
    });
  }, [ref]);

  return (
    <div
      ref={ref}
      onMouseEnter={mouseInside}
      onMouseLeave={mouseOutside}
      style={{
        backgroundColor: "#333",
        borderRadius: "100px",
        padding: "10px 20px",
        right: position.x - dims.width / 2,
        top: position.y - dims.height + 10,
        position: "absolute",
        fontSize: 24,
      }}
    >
      {children}
    </div>
  );
}

export default Popup;
