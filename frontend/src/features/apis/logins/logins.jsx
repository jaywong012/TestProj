import endPoint from "@/constants/endPoint";
import api from "@/features/apis/apiConfig";

const loginServices = {
    login: async (data) => {
        const response = await api.post(`${endPoint.LOGIN}`, data);
        return response;
    }
}

export default loginServices;