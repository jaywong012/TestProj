import React from "react";
import { render, screen, fireEvent, waitFor } from "@testing-library/react";
import { BrowserRouter, useNavigate } from "react-router-dom";
import Login from "./Login";
import "@testing-library/jest-dom";
import loginServices from "@/features/apis/logins/logins";

// Mock useNavigate from react-router-dom
jest.mock("react-router-dom", () => ({
  ...jest.requireActual("react-router-dom"),
  useNavigate: jest.fn(),
}));

// Mock login API
jest.mock("@/features/apis/logins/logins");

describe("Login Component", () => {
  let navigate;

  beforeEach(() => {
    // Mock navigate function
    navigate = jest.fn();
    useNavigate.mockReturnValue(navigate);

    // Clear localStorage before each test
    localStorage.clear();
  });

  test("redirects to home if token exists in localStorage", () => {
    localStorage.setItem("token", "mockToken");

    render(
      <BrowserRouter>
        <Login />
      </BrowserRouter>
    );

    expect(navigate).toHaveBeenCalledWith("/");
  });

  test("allows typing into username and password fields", () => {
    render(
      <BrowserRouter>
        <Login />
      </BrowserRouter>
    );

    const usernameInput = screen.getByPlaceholderText("Enter username");
    const passwordInput = screen.getByPlaceholderText("Enter password");

    fireEvent.change(usernameInput, { target: { value: "testuser" } });
    fireEvent.change(passwordInput, { target: { value: "password123" } });

    expect(usernameInput.value).toBe("testuser");
    expect(passwordInput.value).toBe("password123");
  });

  test("calls login API and stores token on success", async () => {
    const mockToken = "mockToken";
    loginServices.login.mockResolvedValue(mockToken);

    render(
      <BrowserRouter>
        <Login />
      </BrowserRouter>
    );

    const usernameInput = screen.getByPlaceholderText("Enter username");
    const passwordInput = screen.getByPlaceholderText("Enter password");
    const loginButton = screen.getByRole("button", { name: /login/i });

    fireEvent.change(usernameInput, { target: { value: "testuser" } });
    fireEvent.change(passwordInput, { target: { value: "password123" } });
    fireEvent.click(loginButton);

    await waitFor(() => {
      expect(localStorage.getItem("token")).toBe(mockToken);
      expect(navigate).toHaveBeenCalledWith("/");
    });
  });

  test("displays an error message on login failure", async () => {
    loginServices.login.mockRejectedValue(new Error("Invalid credentials"));

    render(
      <BrowserRouter>
        <Login />
      </BrowserRouter>
    );

    const usernameInput = screen.getByPlaceholderText("Enter username");
    const passwordInput = screen.getByPlaceholderText("Enter password");
    const loginButton = screen.getByRole("button", { name: /login/i });

    fireEvent.change(usernameInput, { target: { value: "testuser" } });
    fireEvent.change(passwordInput, { target: { value: "wrongpassword" } });
    fireEvent.click(loginButton);

    await waitFor(() => {
      expect(screen.getByText("Invalid credentials")).toBeInTheDocument();
    });
  });
});
