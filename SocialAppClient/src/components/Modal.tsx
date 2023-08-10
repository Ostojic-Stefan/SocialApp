import {
  ReactElement,
  ReactNode,
  cloneElement,
  createContext,
  useContext,
  useEffect,
  useRef,
  useState,
} from "react";
import ReactDOM from "react-dom";

interface ModalProps {
  children: ReactNode;
}

interface ModalStyles {
  position: "fixed";
  top: string;
  left: string;
  transform: string;
  backgroundColor: string;
  padding: string;
  zIndex: number;
}

interface OverlayStyles {
  position: "fixed";
  backgroundColor: string;
  zIndex: number;
  top: number;
  bottom: number;
  left: number;
  right: number;
}

const modalStyles: ModalStyles = {
  position: "fixed",
  top: "50%",
  left: "50%",
  transform: "translate(-50%, -50%)",
  backgroundColor: "#272523e0",
  padding: "50px",
  zIndex: 9999,
};

const overlayStyles: OverlayStyles = {
  position: "fixed",
  backgroundColor: "rgba(0, 0, 0, 0.7)",
  zIndex: 9999,
  top: 0,
  bottom: 0,
  left: 0,
  right: 0,
};

const initialState = {
  isOpen: false,
  setOpen: (_val: boolean) => {},
};

const ModalContext = createContext(initialState);

function Modal({ children }: ModalProps) {
  const [isOpen, setIsOpen] = useState(false);

  const setOpen = (val: boolean) => setIsOpen(val);

  return (
    <ModalContext.Provider value={{ isOpen, setOpen }}>
      {children}
    </ModalContext.Provider>
  );
}

interface OpenModalProps {
  children: ReactNode;
}

function Open({ children }: OpenModalProps) {
  const { setOpen } = useContext(ModalContext);

  return cloneElement(children as ReactElement, {
    onClick: () => {
      setOpen(true);
    },
  });
}

interface ModalContentProps {
  children: ReactNode;
}

function Content({ children }: ModalContentProps) {
  const { isOpen, setOpen } = useContext(ModalContext);
  const ref = useRef<HTMLDivElement>(null);

  useEffect(() => {
    function handler(event: any) {
      if (ref.current && !ref.current.contains(event.target)) {
        setOpen(false);
      }
    }
    document.addEventListener("click", handler, true);
    return () => document.removeEventListener("click", handler);
  }, [ref]);

  if (!isOpen) return null;

  return ReactDOM.createPortal(
    <>
      <div style={overlayStyles} />
      <div ref={ref} style={modalStyles}>
        <button onClick={() => setOpen(false)}>Close Modal</button>
        {children}
      </div>
    </>,
    document.body
  );
}
Modal.Open = Open;
Modal.Content = Content;

export default Modal;
