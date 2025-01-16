import endPoint from "@/constants/endPoint";
import axiosInstance from "../../config";

const categoryApiServices = {
  getAll: async () => {
    const response = await axiosInstance.get(endPoint.CATEGORY);
    return response;
  },
  addCategory: async (category) => {
    await axiosInstance.post(endPoint.CATEGORY, category);
  },
  updateCategory: async (category) => {
    await axiosInstance.put(endPoint.CATEGORY, category);
  },
  deleteCategory: async (id) => {
    await axiosInstance.delete(endPoint.CATEGORY, id);
  },
};
export default categoryApiServices;
