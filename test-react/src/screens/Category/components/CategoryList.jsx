import React, { useEffect, useMemo, useState } from "react";
import CustomTable from "@/components/CustomTable/CustomTable";
import { setCategories, setEditDetail } from "@/features/redux/slicers/categorySlice";
import { useDispatch, useSelector } from "react-redux";
import categoryApiServices from "@/features/apis/categories/categories";

const CategoryList = () => {
  const header = useMemo(
    () => [
      { name: "Name", width: "60%" },
      { name: "Updated Time", width: "20%" },
    ],
    []
  );

  const dispatch = useDispatch();
  const [loading, setLoading] = useState(true);
  const categories = useSelector((state) => state.category.categories);

  useEffect(() => {
    const fetchCategories = async () => {
      try {
        const result = await categoryApiServices.getAll();
        dispatch(setCategories(result));
      } catch (error) {
        console.error("Error fetching products:", error);
      } finally {
        setLoading(false);
      }
    };
    fetchCategories();
  }, []);

  const handleSetEditDetail = (id) => {
    const category = categories.find((category) => category.id === id);
    dispatch(setEditDetail({ id: category.id, name: category.name }));
  };
  
  const handleDelete = async (id) => {
    await categoryApiServices.deleteCategory(id);
    const result = await categoryApiServices.getAll();
    dispatch(setCategories(result));
  };

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

export default CategoryList;
