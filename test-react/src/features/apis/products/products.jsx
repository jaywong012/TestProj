import endPoint from "@/constants/endPoint";
import axiosInstance from "../../config";

const productApiServices = {
  getProducts: async () => {
    const response = await axiosInstance.get(endPoint.PRODUCT);
    return response;
  },
  getProductsByPaging: async ({ searchKey, pageIndex, pageSize }) => {
    const searchKeyParam = searchKey ? `searchKey=${searchKey}&` : "";
    const response = await axiosInstance.get(
      `${endPoint.PRODUCT}/paged?${searchKeyParam}pageIndex=${pageIndex}&pageSize=${pageSize}`
    );
    return response;
  },
  downloadCsvFile: async (searchKey) => {
    const searchKeyParam = searchKey ? `?searchKey=${searchKey}` : "";
    const response = await axiosInstance.get(
      `${endPoint.PRODUCT}/download-file${searchKeyParam}`,
      { responseType: "blob" }
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
