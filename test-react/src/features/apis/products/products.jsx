import endPoint from "@/constants/endPoint";
import axiosInstance from "../../config";

const productApiServices = {
  getProducts: async () => {
    const response = await axiosInstance.get(endPoint.PRODUCT);
    return response;
  },
  getProductsByPaging: async (page, size) => {
    const response = await axiosInstance.get(
      `${endPoint.PRODUCT}/paged?page=${page}&size=${size}`
    );
    return response;
  },
  addProduct: async (product) => {
    await axiosInstance.post(endPoint.PRODUCT, product);
  },
  updateProduct: async (product) => {
    await axiosInstance.put(endPoint.PRODUCT, product);
  },
  deleteProduct: async (id) => {
    await axiosInstance.delete(endPoint.PRODUCT, id);
  },
};

export default productApiServices;
