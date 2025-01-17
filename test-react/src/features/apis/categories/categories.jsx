import endPoint from "@/constants/endPoint";
import api from "@/features/apis/config";

const categoryApiServices = {
  getAll: async () => {
    const response = await api.get(endPoint.CATEGORY);
    return response;
  },
  addCategory: async (category) => {
    await api.post(endPoint.CATEGORY, category);
  },
  updateCategory: async (category) => {
    await api.put(endPoint.CATEGORY, category);
  },
  deleteCategory: async (id) => {
    await api.delete(endPoint.CATEGORY, id);
  },
};
export default categoryApiServices;
