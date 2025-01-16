import React, { useState, memo } from "react";
import CustomTitle from "../CustomTitle/CustomTitle";
import SpinnerComponent from "../Spinner/Spinner";
import { Table } from "react-bootstrap";
import { Pencil, Trash } from "react-bootstrap-icons";
import PaginationComponent from "../PaginationComponent/PaginationComponent";

const CustomTable = ({
  title,
  headerArray,
  itemArray,
  renderBodyRow,
  handleSetEditDetail,
  handleDelete,
  loading,
  totalPages,
  fetchDataByPaging,
}) => {
  const [currentPage, setCurrentPage] = useState(1);

  const handlePageChange = (pageNumber) => {
    if(pageNumber === currentPage) return;
    setCurrentPage(pageNumber);
    fetchDataByPaging(pageNumber);
  };
  return (
    <>
      <CustomTitle>{title}</CustomTitle>
      {loading || !itemArray ? (
        <SpinnerComponent />
      ) : (
        <>
          <Table striped bordered hover>
            <HeaderComponent headerArray={headerArray} />
            <tbody>
              {itemArray.map((item) => (
                <RowComponent
                  key={item.id}
                  item={item}
                  renderBodyRow={renderBodyRow}
                  handleSetEditDetail={handleSetEditDetail}
                  handleDelete={handleDelete}
                />
              ))}
            </tbody>
          </Table>
          {totalPages && (
            <PaginationComponent
              currentPage={currentPage}
              totalPages={totalPages}
              onPageChange={handlePageChange}
            />
          )}
        </>
      )}
    </>
  );
};

const HeaderComponent = memo(({ headerArray }) => {
  return (
    <thead>
      <tr>
        {headerArray.map((item) => (
          <th
            style={{ width: item.width }}
            key={item.name}
            className="text-center"
          >
            {item.name}
          </th>
        ))}
        <th className="text-center">Action</th>
      </tr>
    </thead>
  );
});

const RowComponent = memo(
  ({ item, renderBodyRow, handleSetEditDetail, handleDelete }) => {
    return (
      <tr>
        {renderBodyRow(item)}
        {handleSetEditDetail && handleDelete && (
          <td className="text-center">
            <div className="text-center">
              <Pencil
                onClick={() => handleSetEditDetail(item.id)}
                className="me-2"
                role="button"
              />
              <Trash onClick={() => handleDelete(item.id)} role="button" />
            </div>
          </td>
        )}
      </tr>
    );
  },
  (prevProps, nextProps) => prevProps.item === nextProps.item
);

export default CustomTable;
