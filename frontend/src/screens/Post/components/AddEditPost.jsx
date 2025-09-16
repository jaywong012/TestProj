import React, { useRef, useEffect, useCallback } from "react";
import PropTypes from "prop-types";
import { Col, Container, Form, Row } from "react-bootstrap";
import CustomButton from "@/components/CustomButton/CustomButton";
import { action, emptyGuid } from "@/constants/common";
import { useDispatch, useSelector } from "react-redux";
import {
  setEditDetail,
  setEmptyEditDetail,
} from "@/features/redux/slicers/postSlice";
import postApiServices from "@/features/apis/posts/posts";
import { socialName } from "@/constants/socialConstant";

const types = [
  { id: 1, name: "Facebook" },
  { id: 2, name: "X" },
];

const AddEditPost = ({ getPosts }) => {
  const dispatch = useDispatch();
  const postUrlRef = useRef(null);
  const editDetail = useSelector((state) => state.post.editDetail);
  useEffect(() => {
    if (editDetail) {
      postUrlRef.current?.focus();
    }
  }, []);
  const handleChange = (e) => {
    e.preventDefault();
    const { name, value } = e.target;
    dispatch(setEditDetail({ ...editDetail, [name]: value }));
  };

  const handleAdd = async (e) => {
    e.preventDefault();
    let post = {
      type: editDetail.type,
      url: editDetail.url,
      title: editDetail.title
    };
    await postApiServices.addPost(post);
    await getPosts();
    dispatch(setEmptyEditDetail());
  };

  const handleEdit = async (e) => {
    e.preventDefault();
    let post = {
      id: editDetail.id,
      type: editDetail.type,
      url: editDetail.url,
      title: editDetail.title
    };
    await postApiServices.updatePost(post);
    dispatch(setEmptyEditDetail());
    await getPosts();
  };

  const cancelEdit = useCallback(() => {
    dispatch(setEmptyEditDetail());
  }, []);
  
  return (
    <Container>
      <h3>Post</h3>
      <Form
        className="add-edit-product-form"
        onSubmit={(e) => (editDetail?.id ? handleEdit(e) : handleAdd(e))}
      >
        <div className="flex g-20">
          <Form.Group as={Row} className="flex-1 mb-3" controlId="type">
            <Form.Label>Type</Form.Label>
            <Col>
              <Form.Select
                // aria-label="Default select example"
                value={editDetail?.type}
                onChange={handleChange}
                name="type"
                tabIndex={0}
              >
                <option value="" className="blur">
                  == Select Type ==
                </option>
                {types?.map((type) => (
                  <option key={type.id} value={type.name}>
                    {type.name}
                  </option>
                ))}
              </Form.Select>
            </Col>
          </Form.Group>
          <Form.Group as={Row} className="flex-1 mb-3" controlId="url">
            <Form.Label>
              {editDetail?.type?.toLowerCase() == socialName.X
                ? "Post Url"
                : "Post Title"}
            </Form.Label>
            <Col>
              <Form.Control
                ref={postUrlRef}
                type="text"
                name={editDetail?.type?.toLowerCase() == socialName.X ? "url" : "title"}
                placeholder={
                  editDetail?.type?.toLowerCase() == socialName.X
                    ? "Enter Post Url"
                    : "Enter Post Title"
                }
                value={editDetail?.type?.toLowerCase() == socialName.X ? editDetail?.url : editDetail?.title}
                onChange={handleChange}
                tabIndex={0}
                disabled={!editDetail?.type}
              />
            </Col>
          </Form.Group>
        </div>
        <div style={{ display: "flex", justifyContent: "flex-end" }}>
          <CustomButton
            action={editDetail?.id ? action.EDIT : action.ADD}
            type={"submit"}
            disabled={!editDetail?.type}
          />
          {editDetail?.id && (
            <CustomButton action={action.CANCEL} onClick={cancelEdit} />
          )}
        </div>
      </Form>
    </Container>
  );
};

export default AddEditPost;
