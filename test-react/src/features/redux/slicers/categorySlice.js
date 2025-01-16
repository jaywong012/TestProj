import { createSlice } from "@reduxjs/toolkit";

const categorySlice = createSlice({
    name: "category",
    initialState:{
        categories: [],
        searchKey: ""
    },
    reducers:{
        setCategories: (state, action) => {
            state.categories = action.payload;
        },
        setSearchKey: (state, action) => {
            state.searchKey = action.payload
        }
    }
});

export const { setCategories, setSearchKey } = categorySlice.actions;
export default categorySlice.reducer;