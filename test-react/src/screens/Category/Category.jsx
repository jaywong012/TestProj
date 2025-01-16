import React, { useCallback, useEffect, useState } from "react";
import { Container } from "react-bootstrap";
import categoryApiServices from "@/features/apis/categories/categories";
import CategoryList from "./components/CategoryList";
import { useDispatch, useSelector } from "react-redux";
import { setCategories } from "../../features/redux/slicers/categorySlice";
import AddEditCategory from "./components/AddEditCategory";

const Category = () => {
  const categories = useSelector((state) => state.category.categories);
  const dispatch = useDispatch();

  const [categoryName, setCategoryName] = useState("");
  const [loading, setLoading] = useState(true);
  const [editId, setEditId] = useState(null);

  useEffect(() => {
    const fetchCategories = async () => {
      try {
        const result = await categoryApiServices.getAll();
        dispatch(setCategories(result));
      } catch (error) {
        console.error("Error fetching products:", error);
      } finally {
        setLoading(false);
      }
    };
    fetchCategories();
  }, []);

  const handleInputChange = (e) => {
    setCategoryName(e.target.value);
  };

  const handleAdd = async (e) => {
    e.preventDefault();
    let category = {
      name: categoryName,
    };
    await categoryApiServices.addCategory(category);
    resetForm();
    const result = await categoryApiServices.getAll();
    dispatch(setCategories(result));
  };

  const handleEdit = async (e) => {
    e.preventDefault();
    let category = {
      name: categoryName,
    };
    category.id = editId;
    await categoryApiServices.updateCategory(category);
    resetForm();
    const result = await categoryApiServices.getAll();
    dispatch(setCategories(result));
  };

  const handleDelete = async (id) => {
    await categoryApiServices.deleteCategory(id);
    setEditId(null);
    resetForm();
    const result = await categoryApiServices.getAll();
    dispatch(setCategories(result));
  };

  const handleSetEditDetail = (id) => {
    const category = categories.find((category) => category.id === id);
    setCategoryName(category.name);
    setEditId(id);
  };

  const resetForm = () => {
    setCategoryName("");
  };

  const cancelEdit = useCallback(() => {
    resetForm();
    setEditId(null);
  }, []);

  return (
    <Container fluid>
      <Container>
        <AddEditCategory
          categoryName={categoryName}
          handleAdd={handleAdd}
          handleEdit={handleEdit}
          editId={editId}
          handleInputChange={handleInputChange}
          cancelEdit={cancelEdit}
        />
        <CategoryList
          categories={categories}
          handleSetEditDetail={handleSetEditDetail}
          handleDelete={handleDelete}
          loading={loading}
        />
      </Container>
    </Container>
  );
};

export default Category;
