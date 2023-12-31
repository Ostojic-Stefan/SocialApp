import { ReactNode, useEffect, useRef, useState } from "react";

interface Props {
  onDrop: (file: FileList) => void;
  children: ReactNode;
}

function FileDropArea({ onDrop, children }: Props) {
  const fileAreaRef = useRef<HTMLDivElement>(null);
  const [isInside, setIsInside] = useState(false);

  const inputContainer = {
    display: "flex",
    justifyContent: "center",
    alignItems: "center",
    border: `2px solid ${isInside ? "#fff" : "#444"}`,
    backgroundColor: "var(--main-dark-gray)",
    color: "var(--main-color)",
    height: "200px",
  };

  function handleDragEnter(event: React.DragEvent<HTMLDivElement>): void {
    event.preventDefault();
    setIsInside(true);
  }

  function handleOnDragLeave(event: React.DragEvent<HTMLDivElement>): void {
    event.preventDefault();
    setIsInside(false);
  }

  function handleDrop(event: React.DragEvent<HTMLDivElement>): void {
    event.preventDefault();
    setIsInside(false);

    const files = event.dataTransfer.files;
    onDrop(files);
  }

  useEffect(() => {
    const preventDefault = (e: Event) => e.preventDefault();

    fileAreaRef.current?.addEventListener("dragover", preventDefault);
    fileAreaRef.current?.addEventListener("drop", preventDefault);

    return () => {
      fileAreaRef.current?.removeEventListener("dragover", preventDefault);
      fileAreaRef.current?.removeEventListener("drop", preventDefault);
    };
  }, []);

  return (
    <div
      onDragEnter={handleDragEnter}
      onDragOver={handleDragEnter}
      onDragLeave={handleOnDragLeave}
      onDrop={handleDrop}
      ref={fileAreaRef}
      style={inputContainer}
    >
      <div>{children}</div>
    </div>
  );
}

export default FileDropArea;
