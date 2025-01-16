import React from "react";
import PropTypes from "prop-types";
import CustomTable from "@/components/CustomTable/CustomTable";

const CategoryList = ({
  categories,
  handleSetEditDetail,
  handleDelete,
  loading,
}) => {
  const renderCategory = (category) => {
    return (
      <td className="text-overflow-ellipse max-width-100">{category.name}</td>
    );
  };
  return (
    <CustomTable
      headerArray={[{ name: "Name", width: "90%" }]}
      handleDelete={handleDelete}
      handleSetEditDetail={handleSetEditDetail}
      itemArray={categories}
      renderBodyRow={renderCategory}
      title={"List Categories"}
      loading={loading}
    />
  );
};

CategoryList.propTypes = {
  categories: PropTypes.arrayOf(
    PropTypes.shape({
      id: PropTypes.number.isRequired,
      name: PropTypes.string.isRequired,
    })
  ).isRequired,
};

export default CategoryList;
