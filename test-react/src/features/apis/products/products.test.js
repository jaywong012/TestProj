import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import productApiServices from './products';
import endPoint from '@/constants/endPoint';
import apiUrls from '../../apiUrls';

const mock = new MockAdapter(axios);
const apiUrl = apiUrls.local;

describe('productApiServices', () => {
  const data = { products: [{ id: 1, name: 'Product 1' }, { id: 1, name: 'Product 2' }, { id: 1, name: 'Product 3' }] };

  test('getProducts should fetch products', async () => {
    mock.onGet(`${apiUrl}/api/${endPoint.PRODUCT}`).reply(200, data);

    const response = await productApiServices.getProducts();
    expect(response).toEqual(data);
  });

  test('getProductsByPaging should fetch products by paging', async () => {
    const page = 2;
    const size = 2;
    mock.onGet(`${apiUrl}/api/product/paged?pageIndex=${page}&pageSize=${size}`).reply(200, data);

    const response = await productApiServices.getProductsByPaging(page, size);
    expect(response).toEqual(data);
  });

  test('addProduct should add items to api response', async () => {
    mock.onPost(`${apiUrl}/api/${endPoint.PRODUCT}`).reply(201);

    await productApiServices.addProduct(data);

    expect(mock.history.post[0].data).toEqual(JSON.stringify(data))
  });

  test('updateProduct should update a product', async () => {
    const updatedProduct = { id: 1, name: 'Updated Product 1' };
    mock.onPut(`${apiUrl}/api/${endPoint.PRODUCT}/${updatedProduct.id}`).reply(200);
  
    await productApiServices.updateProduct(updatedProduct);
  
    expect(mock.history.put[0].data).toEqual(JSON.stringify(updatedProduct));
  });

  test('deleteProduct should delete a product', async () => {
    const productId = 1;
    mock.onDelete(`${apiUrl}/api/${endPoint.PRODUCT}/${productId}`).reply(200);
  
    await productApiServices.deleteProduct(productId);
  
    expect(mock.history.delete[0].url).toEqual(`${apiUrl}/api/${endPoint.PRODUCT}/${productId}`);
  });
});