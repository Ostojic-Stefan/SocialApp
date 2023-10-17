import React from "react";
import styles from "./ButtonStyles.module.css";

interface Props extends React.ButtonHTMLAttributes<HTMLButtonElement> {}

const Button = React.forwardRef<HTMLButtonElement, Props>(
    (props, ref) => {
      return (
        <button className={styles.btn} ref={ref} {...props}>
          {props.children}
        </button>
      );
    }
);

export default Button;
