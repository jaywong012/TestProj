import React, { useMemo } from "react";
import PropTypes from "prop-types";
import CustomTable from "@/components/CustomTable/CustomTable";

const ProductList = ({
  loading,
  products,
  handleSetEditDetail,
  handleDelete,
}) => {
  const header = useMemo(
    () => [
      { name: "Name", width: "50%" },
      { name: "Price", width: "20%" },
      { name: "Category", width: "20%" },
    ],
    []
  );

  const renderProduct = (product) => {
    return (
      <>
        <td className="text-overflow-ellipse max-width-100">{product.name}</td>
        <td className="text-overflow-ellipse max-width-100 text-end">{product.price}</td>
        <td className="text-overflow-ellipse max-width-100">
          {product.categoryName}
        </td>
      </>
    );
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
