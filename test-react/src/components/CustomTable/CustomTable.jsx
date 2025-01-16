import React, { memo } from "react";
import CustomTitle from "../CustomTitle/CustomTitle";
import SpinnerComponent from "../Spinner/Spinner";
import { Table } from "react-bootstrap";
import { Pencil, Trash } from "react-bootstrap-icons";

const CustomTable = ({
  title,
  headerArray,
  itemArray,
  renderBodyRow,
  handleSetEditDetail,
  handleDelete,
  loading,
}) => {
  return (
    <>
      <CustomTitle>{title}</CustomTitle>
      {loading || !itemArray ? (
        <SpinnerComponent />
      ) : (
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
