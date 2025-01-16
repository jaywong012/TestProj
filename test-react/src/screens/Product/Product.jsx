import React, { useCallback, useEffect, useState } from "react";
import ProductList from "./components/ProductList";
import AddEditProduct from "./components/AddEditProduct";
import productApiServices from "@/features/apis/products/products";
import categoryApiServices from "../../features/apis/categories/categories";
import { useDispatch, useSelector } from "react-redux";
import { setProducts } from "../../features/redux/slicers/productSlice";
import { setCategories } from "../../features/redux/slicers/categorySlice";
import { Container } from "react-bootstrap";
import { emptyGuid } from "../../constants/common";

const Product = () => {
  const defaultData = {
    productName: "",
    productPrice: "",
    productCategory: "",
  };
  const dispatch = useDispatch();
  const products = useSelector((state) => state.product.products);
  const categories = useSelector((state) => state.category.categories);

  const [formData, setFormData] = useState(defaultData);
  const [loading, setLoading] = useState(true);
  const [editId, setEditId] = useState(null);

  useEffect(() => {
    getProducts();
    getCategories();
  }, []);

  const getProducts = async () => {
    try {
      const result = await productApiServices.getProductsByPaging();
      dispatch(setProducts(result?.products));
    } catch (error) {
      console.error("Error fetching products:", error);
    } finally {
      setLoading(false);
    }
  };

  const getCategories = async () => {
    try {
      const result = await categoryApiServices.getAll();
      dispatch(setCategories(result));
    } catch (error) {
      console.error("Error fetching products:", error);
    } finally {
      setLoading(false);
    }
  };

  const resetForm = useCallback(() => {
    setFormData(defaultData);
  }, []);

  const handleAdd = async (e) => {
    e.preventDefault();
    let product = {
      name: formData.productName,
      price: formData.productPrice,
      categoryId:
        formData.productCategory !== "" ? formData.productCategory : emptyGuid,
    };
    await productApiServices.addProduct(product);
    resetForm();
    await getProducts();
  };

  const handleEdit = async (e) => {
    e.preventDefault();
    let product = {
      name: formData.productName,
      price: formData.productPrice,
      categoryId:
        formData.productCategory !== "" ? formData.productCategory : emptyGuid,
    };
    product.id = editId;
    await productApiServices.updateProduct(product);
    setEditId(null);
    resetForm();
    await getProducts();
  };

  const handleDelete = async (id) => {
    await productApiServices.deleteProduct(id);
    await getProducts();
  };

  const handleSetEditDetail = (id) => {
    const product = products.find((product) => product.id === id);
    setFormData({
      productName: product.name,
      productPrice: product.price,
      productCategory: product.categoryId ?? "",
    });
    setEditId(id);
  };

  const cancelEdit = useCallback(() => {
    resetForm();
    setEditId(null);
  }, [resetForm]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value,
    });
  };

  return (
    <Container fluid>
      <Container>
        <AddEditProduct
          handleAdd={handleAdd}
          handleEdit={handleEdit}
          categories={categories}
          handleChange={handleChange}
          cancelEdit={cancelEdit}
          editId={editId}
          form={formData}
        />
        <ProductList
          products={products}
          handleSetEditDetail={handleSetEditDetail}
          handleDelete={handleDelete}
          loading={loading}
        />
      </Container>
    </Container>
  );
};

export default Product;
