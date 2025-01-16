import React from "react";
import { Button } from "react-bootstrap";
import "./CustomButton.scss";

const CustomButton = ({ action, type, onClick, style }) => {
  const variants = {
    ADD: "primary",
    EDIT: "primary",
    CANCEL: "danger",
  };

  const texts = {
    ADD: "Add",
    EDIT: "Edit",
    CANCEL: "Cancel",
  };

  const variant = variants[action] || "primary";
  const text = texts[action] || "Add";

  return (
    <Button
      className="button-width"
      style={{
        ...(text === texts.EDIT ? { marginRight: 10 } : {}),
        ...style,
      }}
      role="button"
      variant={variant}
      type={type}
      onClick={onClick}
    >
      {text}
    </Button>
  );
};

export default CustomButton;
