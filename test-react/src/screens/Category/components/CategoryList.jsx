import React, { useMemo } from "react";
import PropTypes from "prop-types";
import CustomTable from "@/components/CustomTable/CustomTable";

const CategoryList = ({
  categories,
  handleSetEditDetail,
  handleDelete,
  loading,
}) => {
  const header = useMemo(
    () => [
      { name: "Name", width: "60%" },
      { name: "Updated Time", width: "20%" },
    ],
    []
  );
  const renderCategory = (category) => {
    return (
      <>
        <td className="text-overflow-ellipse max-width-100">{category.name}</td>
        <td className="text-overflow-ellipse max-width-100 text-center">
          {category.lastSavedTime}
        </td>
      </>
    );
  };
  return (
    <CustomTable
      headerArray={header}
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
