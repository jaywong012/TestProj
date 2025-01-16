import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import productApiServices from './products';
import endPoint from '@/constants/endPoint';
import apiUrls from '../../apiUrls';

const mock = new MockAdapter(axios);
const apiUrl = apiUrls.local;

describe('productApiServices', () => {
  afterEach(() => {
    mock.reset();
  });

  test('getProducts should fetch products', async () => {
    const data = { products: [{ id: 1, name: 'Product 1' }] };
    mock.onGet(`${apiUrl}/api/${endPoint.PRODUCT}`).reply(200, data);

    const response = await productApiServices.getProducts();
    expect(response).toEqual(data);
  });

  test('getProductsByPaging should fetch products by paging', async () => {
    const data = { products: [{ id: 1, name: 'Product 1' }] };
    const page = 1;
    const size = 10;
    mock.onGet(`${apiUrl}/api/product/paged?page=${page}&size=${size}`).reply(200, data);

    const response = await productApiServices.getProductsByPaging(page, size);
    expect(response).toEqual(data);
  });

  // Add tests for addProduct, updateProduct, and deleteProduct similarly
});