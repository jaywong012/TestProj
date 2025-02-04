import { createSlice } from "@reduxjs/toolkit";

let defaultProduct = {
    id: null,
    name: "",
    price: 0,
    categoryId: "",
    categoryName: "",
};

const productSlice = createSlice({
    name: "product",
    initialState:{
        products: [],
        totalPages: 0,
        currentPage: 1,
        searchKey: "",
        editDetail: defaultProduct,
        loading: false
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
        },
        setEditDetail: (state, action) => {
            state.editDetail = action.payload;
        },
        setEmptyEditDetail: (state) => {
            state.editDetail = defaultProduct;
        },
        setLoading: (state, action) => {
            state.loading = action.payload;
        },
    }
});

export const { setProducts, setPages, setCurrentPage, setSearchKey, setEditDetail, setEmptyEditDetail, setLoading } = productSlice.actions;
export default productSlice.reducer;