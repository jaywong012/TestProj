import React from "react";
import './CustomTitle.scss';

const CustomTitle = ({ children, textCase = "initial", color }) => {
  return (
    <div className="title-container">
      <h4 style={{ color: color, textTransform: textCase }}>{children}</h4>
    </div>
  );
};

export default CustomTitle;
