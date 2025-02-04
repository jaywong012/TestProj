import React, { useRef, useEffect, useCallback } from "react";
import PropTypes from "prop-types";
import { Col, Container, Form, Row } from "react-bootstrap";
import CustomButton from "@/components/CustomButton/CustomButton";
import { action, emptyGuid } from "@/constants/common";
import { useDispatch, useSelector } from "react-redux";
import {
  setEditDetail,
  setEmptyEditDetail,
} from "@/features/redux/slicers/productSlice";
import productApiServices from "@/features/apis/products/products";

const AddEditProduct = ({getProducts}) => {
  const dispatch = useDispatch();
  const productNameRef = useRef(null);
  const editDetail = useSelector((state) => state.product.editDetail);
  const categories = useSelector((state) => state.category.categories);
  useEffect(() => {
    if (editDetail) {
      productNameRef.current?.focus();
    }
  }, []);

  const handleChange = (e) => {
    e.preventDefault();
    const { name, value } = e.target;
    dispatch(setEditDetail({ ...editDetail, [name]: value }));
  };

  const handleAdd = async (e) => {
    e.preventDefault();
    let product = {
      name: editDetail.name,
      price: Number(editDetail.price),
      categoryId:
        editDetail.categoryId !== "" ? editDetail.categoryId : emptyGuid,
    };
    await productApiServices.addProduct(product);
    await getProducts();
    dispatch(setEmptyEditDetail());
  };

  const handleEdit = async (e) => {
    e.preventDefault();
    let product = {
      id: editDetail.id,
      name: editDetail.name,
      price: Number(editDetail.price),
      categoryId:
        editDetail.categoryId !== "" ? editDetail.categoryId : emptyGuid,
    };
    await productApiServices.updateProduct(product);
    dispatch(setEmptyEditDetail());
    await getProducts();
  };

  const cancelEdit = useCallback(() => {
    dispatch(setEmptyEditDetail());
  }, []);

  return (
    <Container>
      <h3>Product</h3>
      <Form
        className="add-edit-product-form"
        onSubmit={(e) => (editDetail?.id ? handleEdit(e) : handleAdd(e))}
      >
        <div className="flex g-20">
          <Form.Group as={Row} className="flex-1 mb-3" controlId="name">
            <Form.Label>Product Name</Form.Label>
            <Col>
              <Form.Control
                ref={productNameRef}
                type="text"
                name="name"
                placeholder="Enter Product Name"
                value={editDetail?.name}
                onChange={handleChange}
                tabIndex={0}
              />
            </Col>
          </Form.Group>
          <Form.Group as={Row} className="flex-1 mb-3" controlId="price">
            <Form.Label>Product Price</Form.Label>
            <Col>
              <Form.Control
                type="number"
                name="price"
                step="0.00001"
                min="0"
                max="999999999"
                required
                value={editDetail?.price}
                onChange={handleChange}
                tabIndex={0}
              />
            </Col>
          </Form.Group>
          <Form.Group as={Row} className="flex-1 mb-3" controlId="categoryId">
            <Form.Label>Category Name</Form.Label>
            <Col>
              <Form.Select
                aria-label="Default select example"
                value={editDetail?.categoryId}
                onChange={handleChange}
                name="categoryId"
                tabIndex={0}
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
        <div style={{ display: "flex", justifyContent: "flex-end" }}>
          <CustomButton
            action={editDetail?.id ? action.EDIT : action.ADD}
            type={"submit"}
          />
          {editDetail?.id && (
            <CustomButton action={action.CANCEL} onClick={cancelEdit} />
          )}
        </div>
      </Form>
    </Container>
  );
};
AddEditProduct.propTypes = {
  handleAdd: PropTypes.func.isRequired,
  handleEdit: PropTypes.func.isRequired,
};

export default AddEditProduct;
