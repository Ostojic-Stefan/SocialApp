declare module '*.module.css' {
    interface Styles {
      [className: string]: string;
    }
    const styles: Styles;
    export default styles;
  }
  