import { createSlice } from "@reduxjs/toolkit";

let defaultPost = {
  id: null,
  type: "",
  url: "",
  title: ""
};

const postSlice = createSlice({
  name: "post",
  initialState: {
    posts: [],
    totalPages: 0,
    currentPage: 1,
    searchKey: "",
    editDetail: defaultPost,
    loading: false,
  },
  reducers: {
    setPosts: (state, action) => {
      state.posts = action.payload;
    },
    setLoading: (state, action) => {
      state.loading = action.payload;
    },
    setCurrentPage: (state, action) => {
      state.currentPage = action.payload;
    },
    setSearchKey: (state, action) => {
      state.searchKey = action.payload;
    },
    setEditDetail: (state, action) => {
      state.editDetail = action.payload;
    },
    setEmptyEditDetail: (state) => {
      state.editDetail = defaultPost;
    },
    setPages: (state, action) => {
        state.totalPages = action.payload;
    },
  },
});

export const { setPosts, setPages, setCurrentPage, setSearchKey, setEditDetail, setEmptyEditDetail, setLoading } = postSlice.actions;
export default postSlice.reducer;
