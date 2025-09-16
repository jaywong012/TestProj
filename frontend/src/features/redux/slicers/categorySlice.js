import { createSlice } from "@reduxjs/toolkit";

let defaultCategory = {
  id: null,
  name: "",
};

const categorySlice = createSlice({
  name: "category",
  initialState: {
    categories: [],
    searchKey: "",
    editDetail: defaultCategory,
  },
  reducers: {
    setCategories: (state, action) => {
      state.categories = action.payload;
    },
    setSearchKey: (state, action) => {
      state.searchKey = action.payload;
    },
    setEditDetail: (state, action) => {
      state.editDetail = action.payload;
    },
    setEmptyEditDetail: (state) => {
      state.editDetail = defaultCategory;
    },
  },
});

export const { setCategories, setSearchKey, setEditDetail, setEmptyEditDetail } = categorySlice.actions;
export default categorySlice.reducer;
