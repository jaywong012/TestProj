import { createSlice } from "@reduxjs/toolkit";

const productSlice = createSlice({
    name: "product",
    initialState:{
        products: [],
        totalPages: 0,
        currentPage: 1,
        searchKey: ""
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
        },
        setSearchKey: (state, action) => {
            state.searchKey = action.payload
        }
    }
});

export const { setProducts, setPages, setCurrentPage, setSearchKey } = productSlice.actions;
export default productSlice.reducer;