import { createSlice } from "@reduxjs/toolkit";

const productSlice = createSlice({
    name: "product",
    initialState:{
        products: [],
        totalPages: 0,
        currentPage: 0
    },
    reducers:{
        setProducts: (state, action) => {
            state.products = action.payload;
        },
        setPages: (state, action) => {
            state.totalPages = action.payload;
        },
        setCurrentPage: (state, action) => {
            state.currentPage = action.payload;
        }
    }
});

export const { setProducts, setPages, setCurrentPage } = productSlice.actions;
export default productSlice.reducer;