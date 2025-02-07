import endPoint from "@/constants/endPoint";
import api from "@/features/apis/apiConfig";

const socialAccessInfoApiServices = {
    get: async () => {
        const response = await api.get(`${endPoint.SOCIALACCESSINFO}`);
        return response;
    },
    create: async (data) => {
        const response = await api.post(`${endPoint.SOCIALACCESSINFO}`, data);
        return response;
    },
    delete: async (data) => {
        const response = await api.delete(`${endPoint.SOCIALACCESSINFO}`, data);
        return response;
    }
}

export default socialAccessInfoApiServices;