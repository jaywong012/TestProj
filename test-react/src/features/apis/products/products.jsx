import endPoint from "@/constants/endPoint";
import api from "@/features/config";

const productApiServices = {
  getProducts: async () => {
    const response = await api.get(endPoint.PRODUCT);
    return response;
  },
  getProductsByPaging: async ({ searchKey, pageIndex, pageSize }) => {
    const searchKeyParam = searchKey ? `searchKey=${searchKey}&` : "";
    const response = await api.get(
      `${endPoint.PRODUCT}/paged?${searchKeyParam}pageIndex=${pageIndex}&pageSize=${pageSize}`
    );
    return response;
  },
  downloadCsvFile: async (searchKey) => {
    const searchKeyParam = searchKey ? `?searchKey=${searchKey}` : "";
    const response = await api.get(
      `${endPoint.PRODUCT}/download-file${searchKeyParam}`,
      { responseType: "blob" }
    );
    return response;
  },
  addProduct: async (product) => {
    await api.post(endPoint.PRODUCT, product);
  },
  updateProduct: async (product) => {
    await api.put(endPoint.PRODUCT, product);
  },
  deleteProduct: async (id) => {
    await api.delete(endPoint.PRODUCT, id);
  },
};

export default productApiServices;
