import { configureStore } from "@reduxjs/toolkit";
import productReducer from "../slicers/productSlice";
import categoryReducer from "../slicers/categorySlice";
import postReducer from "../slicers/postSlice";
import errorMessageReducer from "../slicers/errorMessageSlice";
import socialAccessInfoReducer from "../slicers/socialAccessInfoSlice";

const store = configureStore({
    reducer: {
        product: productReducer,
        category: categoryReducer,
        post: postReducer,
        errorMessage : errorMessageReducer,
        socialAccessInfo : socialAccessInfoReducer
    }
});

export default store;