import React from "react";
import Footer from "@/screens/Footer";
import Product from "@/screens/Product/Product";
import Category from "@/screens/Category/Category";
import MyNavbar from "@/screens/Nav";
import {
  BrowserRouter as Router,
  Route,
  Routes,
} from "react-router-dom";
import Login from "./screens/Login/Login";
import PrivateRoute from "./components/RouteConfigurations/PrivateRoute";

const App = () => {
  return (
    <Router>
      <MyNavbar />
      <Routes>
        <Route
          path="/login"
          element={
            <Login />
          }
        />
        <Route
          path="/"
          element={
            <PrivateRoute>
              <Product />
            </PrivateRoute>
          }
        />
        <Route
          path="/category"
          element={
            <PrivateRoute>
              <Category />
            </PrivateRoute>
          }
        />
      </Routes>
      <Footer />
    </Router>
  );
};

export default App;
