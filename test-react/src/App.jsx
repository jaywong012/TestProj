import React from "react";
import Footer from "./components/Footer";
import MyNavbar from "./components/Nav";
import Product from "@/screens/Product/Product";
import Category from "@/screens/Category/Category";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";

const App = () => {
  return (
    <Router>
        <MyNavbar />
        <Routes>
          <Route path="/" element={<Product />} />
          <Route path="/category" element={<Category />} />
        </Routes>
        <Footer />
    </Router>
  );
};

export default App;
