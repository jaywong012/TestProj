import React, {  useEffect } from "react";
import ProductList from "./components/ProductList/ProductList";
import AddEditProduct from "./components/AddEditProduct";
import productApiServices from "@/features/apis/products/products";
import categoryApiServices from "@/features/apis/categories/categories";
import { useDispatch, useSelector } from "react-redux";
import {
  setLoading,
  setPages,
  setProducts,
} from "@/features/redux/slicers/productSlice";
import { setCategories } from "@/features/redux/slicers/categorySlice";
import { Container } from "react-bootstrap";
import { defaultPageIndex, pageSize } from "../../constants/common";

const Product = () => {
  const dispatch = useDispatch();
  const products = useSelector((state) => state.product.products);
  const currentPage = useSelector((state) => state.product.currentPage);
  const searchKey = useSelector((state) => state.product.searchKey);
  const loading = useSelector((state) => state.product.loading);

  useEffect(() => {
    getProducts();
    getCategories();
  }, []);

  const getProducts = async () => {
    try {
      const pageIndex = currentPage ?? defaultPageIndex;
      const searchRequest = {
        searchKey: searchKey,
        pageIndex: pageIndex,
        pageSize: pageSize,
      };
      const result = await productApiServices.getProductsByPaging(
        searchRequest
      );
      dispatch(setProducts(result?.products));
      dispatch(setPages(result?.totalPages));
    } catch (error) {
      console.error("Error fetching products:", error);
    } finally {
      dispatch(setLoading(false));
    }
  };

  const getCategories = async () => {
    try {
      const result = await categoryApiServices.getAll();
      dispatch(setCategories(result));
    } catch (error) {
      console.error("Error fetching products:", error);
    } finally {
      dispatch(setLoading(false));
    }
  };

  const handleDelete = async (id) => {
    await productApiServices.deleteProduct(id);
    await getProducts();
  };

  return (
    <Container fluid>
      <Container>
        <AddEditProduct getProducts={getProducts} />
        <ProductList
          products={products}
          handleDelete={handleDelete}
          loading={loading}
        />
      </Container>
    </Container>
  );
};

export default Product;
