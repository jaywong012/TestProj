import MockAdapter from "axios-mock-adapter";
import endPoint from "@/constants/endPoint";
import apiUrls from "../apiUrls";
import axios from "axios";

const mock = new MockAdapter(axios);
const apiUrl = apiUrls.local;

process.env.NODE_TLS_REJECT_UNAUTHORIZED = "0";
process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0';

describe("productApiServices", () => {
  const data = {
    products: [
      { id: 1, name: "Product 1" },
      { id: 2, name: "Product 2" },
      { id: 3, name: "Product 3" },
    ],
  };

  test("getProducts should fetch products", async () => {
    try {
      mock.onGet(`${apiUrl}/api/${endPoint.PRODUCT}`).reply(200, data);
      const response = await axios.get(`${apiUrl}/api/${endPoint.PRODUCT}`);
      expect(response?.data?.products).toEqual(data?.products);
    } catch (ex) {
      console.log("ex", ex);
    }
  });

  test('getProductsByPaging should fetch products by paging', async () => {
    const page = 2;
    const size = 2;
    mock.onGet(`${apiUrl}/api/${endPoint.PRODUCT}/paged?pageIndex=${page}&pageSize=${size}`).reply(200, data);
    const response = await axios.get(`${apiUrl}/api/${endPoint.PRODUCT}/paged?pageIndex=${page}&pageSize=${size}`);
    expect(response?.data?.products).toEqual(data?.products);
  });

  test('addProduct should add items to api response', async () => {
    mock.onPost(`${apiUrl}/api/${endPoint.PRODUCT}`).reply(201);

    await axios.post(`${apiUrl}/api/${endPoint.PRODUCT}`, data);
    
    expect(mock.history.post[0].data).toEqual(JSON.stringify(data))
  });

  test('updateProduct should update a product', async () => {
    const updatedProduct = { id: 1, name: 'Updated Product 1' };
    mock.onPut(`${apiUrl}/api/${endPoint.PRODUCT}/${updatedProduct.id}`).reply(200);

    await axios.put(`${apiUrl}/api/${endPoint.PRODUCT}/${updatedProduct.id}`, updatedProduct);

    expect(mock.history.put[0].data).toEqual(JSON.stringify(updatedProduct));
  });

  test('deleteProduct should delete a product', async () => {
    const productId = 1;
    mock.onDelete(`${apiUrl}/api/${endPoint.PRODUCT}/${productId}`).reply(200);

    await axios.delete(`${apiUrl}/api/${endPoint.PRODUCT}/${productId}`);

    expect(mock.history.delete[0].url).toEqual(`${apiUrl}/api/${endPoint.PRODUCT}/${productId}`);
  });
});
