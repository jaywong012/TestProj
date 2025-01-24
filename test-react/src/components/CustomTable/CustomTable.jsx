import React, { memo } from "react";
import CustomTitle from "../CustomTitle/CustomTitle";
import SpinnerComponent from "../Spinner/Spinner";
import { Table, Form, Row, Col, Button, Container } from "react-bootstrap";
import { Download, Pencil, Trash } from "react-bootstrap-icons";
import PaginationComponent from "../PaginationComponent/PaginationComponent";
import "./CustomTable.scss";

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
  searchKey,
  handleSearchChange,
  currentPage,
  setCurrentPage,
  isSearchable = false,
  handleDownloadFile,
}) => {
  const handlePageChange = (pageNumber) => {
    if (pageNumber === currentPage) return;
    setCurrentPage(pageNumber);
    fetchDataByPaging(pageNumber);
  };
  const placeholderCount = 10 - itemArray?.length;
  return (
    <Container>
      <div
        style={{
          display: "flex",
          flexDirection: "row",
          alignItems: "center",
          gap: 12,
        }}
      >
        <CustomTitle>{title}</CustomTitle>
        {handleDownloadFile && (
          <Button onClick={() => handleDownloadFile()}>
            <Download />
          </Button>
        )}
      </div>
      {isSearchable && (
        <Form.Group as={Row} className="mb-3">
          <Col sm="4">
            <Form.Control
              type="text"
              placeholder="Search..."
              value={searchKey}
              onChange={handleSearchChange}
            />
          </Col>
        </Form.Group>
      )}
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
              {placeholderCount > 0 &&
                Array.from({ length: placeholderCount }).map((_, index) => (
                  <tr
                    key={`${placeholderCount}-${index}`}
                    className="hidden-tr"
                  >
                    <td colSpan={headerArray.length + 1} className="hidden-td">
                      &nbsp;
                    </td>
                  </tr>
                ))}
            </tbody>
          </Table>
          {totalPages > 0 && (
            <PaginationComponent
              currentPage={currentPage}
              totalPages={totalPages}
              onPageChange={handlePageChange}
            />
          )}
        </>
      )}
    </Container>
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
