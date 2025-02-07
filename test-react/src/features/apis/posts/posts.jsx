import endPoint from "@/constants/endPoint";
import api from "../apiConfig";

const postApiServices = {
  getPostsByPaging: async ({ searchKey, pageIndex, pageSize }) => {
    const searchKeyParam = searchKey ? `searchKey=${searchKey}&` : "";
    const response = await api.get(
      `${endPoint.POST}?${searchKeyParam}pageIndex=${pageIndex}&pageSize=${pageSize}`
    );
    return response;
  },
  addPost: async (data) => {
    await api.post(endPoint.POST, data);
  },
  updatePost: async (data) => {
    await api.put(endPoint.POST, data);
  },
  deletePost: async (id) => {
    await api.delete(endPoint.POST, id);
  },
  checkRetweet: async (data) => {
    await api.post(`${endPoint.POST}/check-retweet`, data);
  }
};

export default postApiServices;
