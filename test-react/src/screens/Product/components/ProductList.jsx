import React, { useMemo } from "react";
import PropTypes from "prop-types";
import CustomTable from "@/components/CustomTable/CustomTable";
import { useDispatch, useSelector } from "react-redux";
import { pageSize } from "@/constants/common";
import {
  setCurrentPage,
  setPages,
  setProducts,
  setSearchKey,
} from "@/features/redux/slicers/productSlice";
import productApiServices from "@/features/apis/products/products";

const ProductList = ({
  loading,
  products,
  handleSetEditDetail,
  handleDelete,
}) => {
  const header = useMemo(
    () => [
      { name: "Name", width: "30%" },
      { name: "Price", width: "20%" },
      { name: "Category", width: "20%" },
      { name: "Updated Time", width: "20%" },
    ],
    []
  );

  const dispatch = useDispatch();
  const totalPages = useSelector((state) => state.product.totalPages);
  const reduxSearchKey = useSelector((state) => state.product.searchKey);
  const currentPage = useSelector((state) => state.product.currentPage);

  const formatNumber = (value) => {
    const number = parseFloat(value);
    if (isNaN(number)) return "";
    return Math.ceil(number).toLocaleString();
  };

  const renderProduct = (product) => {
    return (
      <>
        <td className="text-overflow-ellipse max-width-100">{product.name}</td>
        <td className="text-overflow-ellipse max-width-100 text-end">
          {formatNumber(product.price)}
        </td>
        <td className="text-overflow-ellipse max-width-100 text-center">
          {product.categoryName}
        </td>
        <td className="text-overflow-ellipse max-width-100 text-center">
          {product.lastSavedTime}
        </td>
      </>
    );
  };

  const fetchDataByPaging = async (currentPage, searchValue = "") => {
    const searchText = searchValue ?? reduxSearchKey;
    const searchRequest = {
      searchKey: searchText,
      pageIndex: currentPage,
      pageSize: pageSize,
    };
    const result = await productApiServices.getProductsByPaging(searchRequest);
    dispatch(setProducts(result?.products));
    dispatch(setPages(result?.totalPages));
    dispatch(setCurrentPage(currentPage));
  };

  const handleSearchChange = async (value) => {
    const firstPage = 1;
    dispatch(setSearchKey(value));
    fetchDataByPaging(firstPage, value);
  };

  const handleDownloadFile = async () => {
    const response = await productApiServices.downloadCsvFile(reduxSearchKey);
    const url = window.URL.createObjectURL(new Blob([response]));
    const link = document.createElement("a");
    link.href = url;
    link.setAttribute("download", "product.csv");
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
  };

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
      searchKey={reduxSearchKey}
      handleSearchChange={(e) => handleSearchChange(e.target.value)}
      currentPage={currentPage}
      setCurrentPage={() => dispatch(setCurrentPage(currentPage))}
      isSearchable={true}
      handleDownloadFile={handleDownloadFile}
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
