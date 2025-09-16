import React from "react";
import { Container } from "react-bootstrap";
import CategoryList from "./components/CategoryList";
import AddEditCategory from "./components/AddEditCategory";

const Category = () => {
  return (
    <Container fluid>
      <Container>
        <AddEditCategory />
        <CategoryList />
      </Container>
    </Container>
  );
};

export default Category;
