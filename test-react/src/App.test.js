import React from 'react';
import { render, screen } from '@testing-library/react';
import App from './App';

// --- Mocks ---
// Replace the original implementations with simple test doubles that render identifiable elements.
jest.mock('@/screens/Nav', () => () => <div data-testid="navbar">Navbar</div>);
jest.mock('@/screens/Footer', () => () => <div data-testid="footer">Footer</div>);
jest.mock('@/screens/Product/Product', () => () => <div data-testid="product">Product</div>);
jest.mock('@/screens/Category/Category', () => () => <div data-testid="category">Category</div>);
jest.mock('./screens/Login/Login', () => () => <div data-testid="login">Login</div>);

describe('App Routing', () => {
  // Clear localStorage before each test to avoid test cross-contamination.
  beforeEach(() => {
    localStorage.clear();
  });

  test('redirects to login when token is not present and route is "/"', async () => {
    // Set the current URL to "/"
    window.history.pushState({}, 'Test page', '/');

    render(<App />);

    // Since no token exists, the PrivateRoute should redirect to /login.
    expect(await screen.findByTestId('login')).toBeInTheDocument();

    // You can also verify that the protected content is NOT rendered.
    expect(screen.queryByTestId('product')).not.toBeInTheDocument();
  });

  test('renders Product when token is present and route is "/"', async () => {
    // Add a fake token so that PrivateRoute allows access.
    localStorage.setItem('token', 'fake-token');

    window.history.pushState({}, 'Test page', '/');

    render(<App />);

    // With a token, the Product component should be rendered.
    expect(await screen.findByTestId('product')).toBeInTheDocument();

    // The login page should not be rendered.
    expect(screen.queryByTestId('login')).not.toBeInTheDocument();
  });

  test('redirects to login when token is not present and route is "/category"', async () => {
    window.history.pushState({}, 'Test page', '/category');

    render(<App />);

    // Expect redirection to /login because no token exists.
    expect(await screen.findByTestId('login')).toBeInTheDocument();

    // Ensure the Category component is not displayed.
    expect(screen.queryByTestId('category')).not.toBeInTheDocument();
  });

  test('renders Category when token is present and route is "/category"', async () => {
    localStorage.setItem('token', 'fake-token');

    window.history.pushState({}, 'Test page', '/category');

    render(<App />);

    // With a token, the Category component should be rendered.
    expect(await screen.findByTestId('category')).toBeInTheDocument();

    // The login page should not be rendered.
    expect(screen.queryByTestId('login')).not.toBeInTheDocument();
  });

  test('always renders Navbar and Footer regardless of authentication', () => {
    // You can test with any route; here we use the root route.
    window.history.pushState({}, 'Test page', '/');

    render(<App />);

    expect(screen.getByTestId('navbar')).toBeInTheDocument();
    expect(screen.getByTestId('footer')).toBeInTheDocument();
  });
});
