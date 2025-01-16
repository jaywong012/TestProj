import React from "react";
import PropTypes from "prop-types";
import { Col, Form, Row } from "react-bootstrap";
import CustomButton from "@/components/CustomButton/CustomButton";
import { action } from "@/constants/common";

const AddEditProduct = ({
  handleAdd,
  handleEdit,
  categories,
  editId,
  cancelEdit,
  handleChange,
  form,
}) => {
  return (
    <div>
      <h3>Product</h3>
      <Form
        className="add-edit-product-form"
        onSubmit={(e) => (editId ? handleEdit(e) : handleAdd(e))}
      >
        <div className="flex g-20">
          <Form.Group as={Row} className="flex-1 mb-3" controlId="productName">
            <Form.Label>Product Name</Form.Label>
            <Col>
              <Form.Control
                type="text"
                name="productName"
                value={form.productName}
                onChange={handleChange}
              />
            </Col>
          </Form.Group>
          <Form.Group as={Row} className="flex-1 mb-3" controlId="productPrice">
            <Form.Label>Product Price</Form.Label>
            <Col>
              <Form.Control
                type="number"
                name="productPrice"
                step="0.00001"
                min="0"
                max="999999999"
                required
                value={form.productPrice}
                onChange={handleChange}
              />
            </Col>
          </Form.Group>
          <Form.Group
            as={Row}
            className="flex-1 mb-3"
            controlId="productCategory"
          >
            <Form.Label>Category Name</Form.Label>
            <Col>
              <Form.Select
                aria-label="Default select example"
                value={form.productCategory}
                onChange={handleChange}
                name="productCategory"
              >
                <option value="" className="blur"></option>
                {categories?.map((category) => (
                  <option key={category.id} value={category.id}>
                    {category.name}
                  </option>
                ))}
              </Form.Select>
            </Col>
          </Form.Group>
        </div>
        <CustomButton
          action={editId ? action.EDIT : action.ADD}
          type={"submit"}
        />
        {editId && <CustomButton action={action.CANCEL} onClick={cancelEdit} />}
      </Form>
    </div>
  );
};
AddEditProduct.propTypes = {
  handleAdd: PropTypes.func.isRequired,
  handleEdit: PropTypes.func.isRequired,
};

export default AddEditProduct;
