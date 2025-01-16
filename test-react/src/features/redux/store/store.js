import { configureStore } from "@reduxjs/toolkit";
import productReducer from "../slicers/productSlice";
import categoryReducer from "../slicers/categorySlice";

const store = configureStore({
    reducer: {
        product: productReducer,
        category: categoryReducer
    }
});

export default store;