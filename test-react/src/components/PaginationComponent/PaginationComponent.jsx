import React from "react";
import { Pagination } from "react-bootstrap";

const PaginationComponent = ({ currentPage, totalPages, onPageChange }) => {
  const pageNumbers = [];
  const maxPagesToShow = 3;

  let startPage = Math.max(1, currentPage - Math.floor(maxPagesToShow / 2));
  let endPage = Math.min(totalPages, startPage + maxPagesToShow - 1);

  if (endPage === totalPages) {
    startPage = Math.max(1, endPage - maxPagesToShow + 1);
  }

  for (let i = startPage; i <= endPage; i++) {
    pageNumbers.push(i);
  }

  return (
    <Pagination>
      <Pagination.Prev
        onClick={() => currentPage > 1 && onPageChange(currentPage - 1)}
      />
      {startPage > 1 && (
        <>
          <Pagination.Item onClick={() => onPageChange(1)}>1</Pagination.Item>
          {startPage > 2 && <Pagination.Ellipsis />}
        </>
      )}
      {pageNumbers.map((number) => (
        <Pagination.Item
          key={number}
          active={number === currentPage}
          onClick={() => onPageChange(number)}
        >
          {number}
        </Pagination.Item>
      ))}
      {endPage < totalPages && (
        <>
          {endPage < totalPages - 1 && <Pagination.Ellipsis />}
          <Pagination.Item onClick={() => onPageChange(totalPages)}>
            {totalPages}
          </Pagination.Item>
        </>
      )}
      <Pagination.Next
        onClick={() =>
          currentPage < totalPages && onPageChange(currentPage + 1)
        }
      />
    </Pagination>
  );
};

export default PaginationComponent;
