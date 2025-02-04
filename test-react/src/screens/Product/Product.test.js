import React from "react";
import { render, screen, fireEvent, waitFor } from "@testing-library/react";
import { Provider } from "react-redux";
import configureStore from "redux-mock-store";
import Product from "./Product";
import "@testing-library/jest-dom";
import productApiServices from "@/features/apis/products/products";
import categoryApiServices from "@/features/apis/categories/categories";
import { setProducts, setPages } from "@/features/redux/slicers/productSlice";
import { setCategories } from "@/features/redux/slicers/categorySlice";

// Mock store setup
const mockStore = configureStore([]);
jest.mock("@/features/apis/products/products");
jest.mock("@/features/apis/categories/categories");

describe("Product Component", () => {
  let store;
  let mockProducts;
  let mockCategories;

  beforeEach(() => {
    store = mockStore({
      product: {
        products: [],
        totalPages: 5,
        searchKey: "",
        currentPage: 1,
      },
      category: {
        categories: [],
      },
    });

    store.dispatch = jest.fn(); // Mock dispatch function

    mockProducts = [
      { id: 1, name: "Product A", price: 1000, categoryId: "cat-1" },
      { id: 2, name: "Product B", price: 2000, categoryId: "cat-2" },
    ];

    mockCategories = [
      { id: "cat-1", name: "Category 1" },
      { id: "cat-2", name: "Category 2" },
    ];
  });

  test("fetches products and categories on mount", async () => {
    productApiServices.getProductsByPaging.mockResolvedValue({
      products: mockProducts,
      totalPages: 5,
    });

    categoryApiServices.getAll.mockResolvedValue(mockCategories);

    render(
      <Provider store={store}>
        <Product />
      </Provider>
    );

    await waitFor(() => {
      expect(productApiServices.getProductsByPaging).toHaveBeenCalled();
      expect(productApiServices.getProductsByPaging).toHaveBeenCalledWith({
        searchKey: "",
        pageIndex: 1,
        pageSize: 10,
      });

      expect(categoryApiServices.getAll).toHaveBeenCalled();
    });

    expect(store.dispatch).toHaveBeenCalledWith(setProducts(mockProducts));
    expect(store.dispatch).toHaveBeenCalledWith(setPages(5));
    expect(store.dispatch).toHaveBeenCalledWith(setCategories(mockCategories));
  });

  test("handles adding a new product", async () => {
    productApiServices.addProduct.mockResolvedValue({});
    productApiServices.getProductsByPaging.mockResolvedValue({
      products: mockProducts,
      totalPages: 5,
    });

    const {container} = render(
      <Provider store={store}>
        <Product />
      </Provider>
    );

    const nameInput = screen.getByPlaceholderText("Enter Product Name");
    const priceInput = container.querySelector("input#productPrice");
    const addButton = screen.getByRole("button", { name: /add/i });

    fireEvent.change(nameInput, { target: { value: "New Product" } });
    fireEvent.change(priceInput, { target: { value: "1500" } });

    fireEvent.click(addButton);

    await waitFor(() => {
      expect(productApiServices.addProduct).toHaveBeenCalledWith({
        name: "New Product",
        price: 1500,
        categoryId: "00000000-0000-0000-0000-000000000000", // emptyGuid
      });

      expect(productApiServices.getProductsByPaging).toHaveBeenCalled();
    });
  });

  test("handles editing a product", async () => {
    productApiServices.updateProduct.mockResolvedValue({});
    productApiServices.getProductsByPaging.mockResolvedValue({
      products: mockProducts,
      totalPages: 5,
    });

    const {container} = render(
      <Provider store={store}>
        <Product />
      </Provider>
    );

    // Simulate clicking edit on the first product
    const editButtons = screen.getAllByRole("button", { name: /edit/i });
    fireEvent.click(editButtons[0]);

    // Ensure edit mode is activated
    const nameInput = screen.getByPlaceholderText("Enter Product Name");
    expect(nameInput.value).toBe("Product A");

    // Change product details
    fireEvent.change(nameInput, { target: { value: "Updated Product A" } });

    const saveButton = screen.getByRole("button", { name: /save/i });
    fireEvent.click(saveButton);

    await waitFor(() => {
      expect(productApiServices.updateProduct).toHaveBeenCalledWith({
        id: 1,
        name: "Updated Product A",
        price: 1000,
        categoryId: "cat-1",
      });

      expect(productApiServices.getProductsByPaging).toHaveBeenCalled();
    });
  });

//   test("handles deleting a product", async () => {
//     productApiServices.deleteProduct.mockResolvedValue({});
//     productApiServices.getProductsByPaging.mockResolvedValue({
//       products: mockProducts,
//       totalPages: 5,
//     });

//     render(
//       <Provider store={store}>
//         <Product />
//       </Provider>
//     );

//     // Simulate clicking delete on the first product
//     const deleteButtons = screen.getAllByRole("button", { name: /delete/i });
//     fireEvent.click(deleteButtons[0]);

//     await waitFor(() => {
//       expect(productApiServices.deleteProduct).toHaveBeenCalledWith(1);
//       expect(productApiServices.getProductsByPaging).toHaveBeenCalled();
//     });
//   });

//   test("handles cancel edit correctly", async () => {
//     render(
//       <Provider store={store}>
//         <Product />
//       </Provider>
//     );

//     // Simulate clicking edit on the first product
//     const editButtons = screen.getAllByRole("button", { name: /edit/i });
//     fireEvent.click(editButtons[0]);

//     // Ensure edit mode is activated
//     const nameInput = screen.getByPlaceholderText("Enter product name");
//     expect(nameInput.value).toBe("Product A");

//     // Click cancel button
//     const cancelButton = screen.getByRole("button", { name: /cancel/i });
//     fireEvent.click(cancelButton);

//     // Ensure edit mode is canceled
//     expect(nameInput.value).toBe("");
//   });
});
