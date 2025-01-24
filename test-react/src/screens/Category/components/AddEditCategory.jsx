import CustomButton from "@/components/CustomButton/CustomButton";
import { action } from "@/constants/common";
import React from "react";
import { Col, Container, Form, Row } from "react-bootstrap";

const AddEditCategory = ({
    categoryName,
    handleAdd,
    handleEdit,
    handleInputChange,
    editId,
    cancelEdit
}) => {
  return (
    <Container>
      <h3>Category</h3>
      <Form onSubmit={editId ? handleEdit : handleAdd}>
        <Form.Group as={Row} className="mb-3" controlId="categoryName">
          <Form.Label>Category Name</Form.Label>
          <Col md={4}>
            <Form.Control
              type="text"
              value={categoryName}
              onChange={handleInputChange}
              required
              placeholder="Enter Category Name"
            />
          </Col>
        </Form.Group>
        <CustomButton action={editId ? action.EDIT : action.ADD} type={"submit"} />
        {editId && <CustomButton action={action.CANCEL} onClick={cancelEdit} />}
      </Form>
    </Container>
  );
};

export default AddEditCategory;
