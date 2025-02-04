import CustomButton from "@/components/CustomButton/CustomButton";
import { action } from "@/constants/common"
import categoryApiServices from "@/features/apis/categories/categories";
import { setCategories, setEditDetail, setEmptyEditDetail } from "@/features/redux/slicers/categorySlice";
import { useCallback } from "react";
import { Col, Container, Form, Row } from "react-bootstrap";
import { useDispatch, useSelector } from "react-redux";

const AddEditCategory = () => {
  const dispatch = useDispatch();
  const editDetail = useSelector((state) => state.category.editDetail);

  const handleChange = (e) => {
    e.preventDefault();
    const { name, value } = e.target;
    dispatch(setEditDetail({ ...editDetail, [name]: value }));
  };

  const handleAdd = async (e) => {
    e.preventDefault();
    let category = {
      name: editDetail?.name,
    };
    await categoryApiServices.addCategory(category);
    dispatch(setEmptyEditDetail());
    const result = await categoryApiServices.getAll();
    dispatch(setCategories(result));
  };

  const handleEdit = async (e) => {
    e.preventDefault();
    let category = {
      id: editDetail?.id,
      name: editDetail?.name,
    };
    await categoryApiServices.updateCategory(category);
    dispatch(setEmptyEditDetail());
    const result = await categoryApiServices.getAll();
    dispatch(setCategories(result));
  };

  const cancelEdit = useCallback(() => {
    dispatch(setEmptyEditDetail());
  }, []);

  return (
    <Container>
      <h3>Category</h3>
      <Form onSubmit={editDetail?.id ? handleEdit : handleAdd}>
        <Form.Group as={Row} className="mb-3" controlId="categoryName">
          <Form.Label>Category Name</Form.Label>
          <Col md={4}>
            <Form.Control
              type="text"
              value={editDetail?.name}
              onChange={handleChange}
              required
              placeholder="Enter Category Name"
              name={"name"}
            />
          </Col>
        </Form.Group>
        <CustomButton action={editDetail?.id ? action.EDIT : action.ADD} type={"submit"} />
        {editDetail?.id && <CustomButton action={action.CANCEL} onClick={cancelEdit} />}
      </Form>
    </Container>
  );
};

export default AddEditCategory;
