import axios from "axios";
import apiUrls from "./apiUrls";

const apiUrl = apiUrls.local;

const axiosInstance = axios.create({
  baseURL: `${apiUrl}/api/`,
});

axiosInstance.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem('token');
    if(token){
      config.headers['Authorization'] = `Bearer ${token}`;
    }

    return config;
  },
  error => {
    return Promise.reject(new Error(error));
  }
)

const api = {
  get: async (endPoint) => {
    try {
      const response = await axiosInstance.get(endPoint);
      return response?.data;
    } catch (e) {
      console.error("Error fetching data:", e);
    }
  },
  post: async (endPoint, data) => {
    try {
      const response = await axiosInstance.post(endPoint, data);
      return response?.data;
    } catch (e) {
      console.error("Error adding data:", e);
    }
  },
  put: async (endPoint, data) => {
    try {
      const id = data.id;
      await axiosInstance.put(`${endPoint}/${id}`, data);
    } catch (e) {
      console.error("Error updating data:", e);
    }
  },
  delete: async (endPoint, id) => {
    try {
      await axiosInstance.delete(`${endPoint}/${id}`);
    } catch (e) {
      console.error("Error deleting data:", e);
    }
  },
};

export default api;
