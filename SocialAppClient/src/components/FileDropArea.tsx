import { ReactNode, useEffect, useRef, useState } from "react";

interface Props {
  onFileChange: (file: FileList) => void;
  children: ReactNode;
}

function FileDropArea({ onFileChange, children }: Props) {
  const fileAreaRef = useRef<HTMLDivElement>(null);
  const [isInside, setIsInside] = useState(false);

  const inputContainer = {
    display: "flex",
    justifyContent: "center",
    alignItems: "center",
    border: `2px solid ${isInside ? "#fff" : "#444"}`,
    backgroundColor: "#1d1b1a",
    color: "#999",
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
    onFileChange(files);
  }

  useEffect(() => {
    const preventDefault = (e: Event) => e.preventDefault();

    fileAreaRef.current?.addEventListener("dragover", preventDefault);
    fileAreaRef.current?.addEventListener("dtop", preventDefault);

    return () => {
      fileAreaRef.current?.removeEventListener("dragover", preventDefault);
      fileAreaRef.current?.removeEventListener("dtop", preventDefault);
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
