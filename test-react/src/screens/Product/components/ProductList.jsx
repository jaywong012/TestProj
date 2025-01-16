import React, { useMemo } from "react";
import PropTypes from "prop-types";
import CustomTable from "@/components/CustomTable/CustomTable";
import { useDispatch, useSelector } from "react-redux";
import { pageSize } from "@/constants/common";
import { setCurrentPage, setPages, setProducts } from "@/features/redux/slicers/productSlice";
import productApiServices from "@/features/apis/products/products";

const ProductList = ({
  loading,
  products,
  handleSetEditDetail,
  handleDelete,
}) => {
  const dispatch = useDispatch();
  const totalPages = useSelector((state) => state.product.totalPages);
  const header = useMemo(
    () => [
      { name: "Name", width: "30%" },
      { name: "Price", width: "20%" },
      { name: "Category", width: "20%" },
      { name: "Updated Time", width: "20%" },
    ],
    []
  );

  const formatNumber = (value) => {
    const number = parseFloat(value);
    if (isNaN(number)) return "";
    return Math.ceil(number).toLocaleString();
  };

  const renderProduct = (product) => {
    return (
      <>
        <td className="text-overflow-ellipse max-width-100">{product.name}</td>
        <td className="text-overflow-ellipse max-width-100 text-end">{formatNumber(product.price)}</td>
        <td className="text-overflow-ellipse max-width-100">
          {product.categoryName}
        </td>
        <td className="text-overflow-ellipse max-width-100 text-center">
          {product.lastSavedTime}
        </td>
      </>
    );
  };

  const fetchDataByPaging = async (currentPage) => {
    const result = await productApiServices.getProductsByPaging(currentPage, pageSize);
    dispatch(setProducts(result?.products));
    dispatch(setPages(result?.totalPages));
    dispatch(setCurrentPage(currentPage));
  }

  return (
    <CustomTable
      headerArray={header}
      handleDelete={handleDelete}
      handleSetEditDetail={handleSetEditDetail}
      itemArray={products}
      renderBodyRow={renderProduct}
      title={"List Products"}
      loading={loading}
      totalPages={totalPages}
      fetchDataByPaging={fetchDataByPaging}
    />
  );
};

ProductList.propTypes = {
  products: PropTypes.arrayOf(
    PropTypes.shape({
      id: PropTypes.number.isRequired,
      name: PropTypes.string.isRequired,
      price: PropTypes.number.isRequired,
    })
  ).isRequired,
};

export default ProductList;
