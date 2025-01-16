import axios from "axios";
import apiUrls from "./apiUrls";

const apiUrl = apiUrls.local;

const axiosInstance = {
  get: async (endPoint) => {
    try {
      const response = await axios.get(`${apiUrl}/api/${endPoint}`);
      return response?.data;
    } catch (e) {
      console.error("Error fetching data:", e);
    }
  },
  post: async (endPoint, data) => {
    try {
      await axios.post(`${apiUrl}/api/${endPoint}`, data);
    } catch (e) {
      console.error("Error adding data:", e);
    }
  },
  put: async (endPoint, data) => {
    try {
      const id = data.id;
      await axios.put(`${apiUrl}/api/${endPoint}/${id}`, data);
    } catch (e) {
      console.error("Error updating data:", e);
    }
  },
  delete: async (endPoint, id) => {
    try {
      await axios.delete(`${apiUrl}/api/${endPoint}/${id}`);
    } catch (e) {
      console.error("Error updating data:", e);
    }
  },
};

export default axiosInstance;
