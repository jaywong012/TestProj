import { createSlice } from "@reduxjs/toolkit";

let defaultUsers = {
  x: {
    userName: "",
    userId: "",
  },
  facebook:{
    userName: "",
    userId: "",
  }
};

const socialAccessInfoSlice = createSlice({
  name: "socialAccessInfo",
  initialState: {
    userDetails: defaultUsers
  },
  reducers: {
    setUserDetail: (state, action) => {
      const { platform, data } = action.payload;
      state.userDetails[platform] = { ...data };
    },
  },
});

export const { setUserDetail } = socialAccessInfoSlice.actions;
export default socialAccessInfoSlice.reducer;
