import { setErrorMessage } from "@/features/redux/slicers/errorMessageSlice";
import React, { useEffect, useState } from "react";
import { Toast, ToastContainer, Button } from "react-bootstrap";
import { useDispatch, useSelector } from "react-redux";

const ToastMessage = () => {
  const [show, setShow] = useState(false);
  const dispatch = useDispatch();
  const errorMessage = useSelector((state) => state.errorMessage.errorMessage);

  useEffect(() => {
    if (errorMessage) {
      setShow(true);
      setTimeout(() => {
        setShow(false);
        dispatch(setErrorMessage("")); // Clear error after closing
      }, 3000);
    }
  }, [errorMessage, dispatch]);
  return (
    <div className="p-3">
      <ToastContainer position="top-end" className="p-3">
        <Toast
          onClose={() => setShow(false)}
          show={show}
          bg={"danger"}
          delay={3500}
          autohide
        >
          <Toast.Body style={{ color: "white" }}>{errorMessage}</Toast.Body>
        </Toast>
      </ToastContainer>
    </div>
  );
};

export default ToastMessage;
