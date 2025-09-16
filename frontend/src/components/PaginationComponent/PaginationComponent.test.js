import React from "react";
import { render, screen, fireEvent } from "@testing-library/react";
import PaginationComponent from "./PaginationComponent";
import '@testing-library/jest-dom';

describe("PaginationComponent", () => {
  const onPageChange = jest.fn();

  test("renders pagination items correctly", () => {
    render(<PaginationComponent currentPage={2} totalPages={5} onPageChange={onPageChange} />);

    expect(screen.getByText("1")).toBeInTheDocument();
    expect(screen.getByText("2")).toBeInTheDocument();
    expect(screen.getByText("3")).toBeInTheDocument();
  });

  test("calls onPageChange with correct page number when a page item is clicked", () => {
    render(<PaginationComponent currentPage={2} totalPages={5} onPageChange={onPageChange} />);

    fireEvent.click(screen.getByText("3"));
    expect(onPageChange).toHaveBeenCalledWith(3);
  });

  test("calls onPageChange with correct page number when prev button is clicked", () => {
    render(<PaginationComponent currentPage={2} totalPages={5} onPageChange={onPageChange} />);

    fireEvent.click(screen.getByText("Previous"));
    expect(onPageChange).toHaveBeenCalledWith(1);
  });

  test("calls onPageChange with correct page number when next button is clicked", () => {
    render(<PaginationComponent currentPage={2} totalPages={5} onPageChange={onPageChange} />);

    fireEvent.click(screen.getByText("Next"));
    expect(onPageChange).toHaveBeenCalledWith(3);
  });

  test("renders ellipsis correctly", () => {
    render(<PaginationComponent currentPage={4} totalPages={10} onPageChange={onPageChange} />);

    expect(screen.getAllByText("…")).toHaveLength(2);
  });

  test('handles left ellipsis click correctly', async () => {
    const onPageChange = jest.fn();
    render(
      <PaginationComponent currentPage={4} totalPages={10} onPageChange={onPageChange} />
    );

    const ellipsisItems = screen.getAllByText('…');
    expect(ellipsisItems).toHaveLength(2);

    fireEvent.click(ellipsisItems[0]);
    expect(onPageChange).toHaveBeenCalledWith(2);
  });

  test('handles right ellipsis click correctly', async () => {
    const onPageChange = jest.fn();

    render(
      <PaginationComponent currentPage={4} totalPages={10} onPageChange={onPageChange} />
    )

    const ellipsisItems = screen.getAllByText('…');
    expect(ellipsisItems).toHaveLength(2);

    fireEvent.click(ellipsisItems[1]);
    expect(onPageChange).toHaveBeenCalledWith(7);
  });

  test('test startPage and endPage equal to 1 and totalPages respectively', () => {
    const onPageChange = jest.fn();

    render(
      <PaginationComponent currentPage={10} totalPages={10} onPageChange={onPageChange} />
    );
    
    expect(screen.getByText("1")).toBeInTheDocument();
  });

  test('test click to first page', async () => {
    const onPageChange = jest.fn();

    render(
      <PaginationComponent currentPage={10} totalPages={10} onPageChange={onPageChange} />
    );
    
    fireEvent.click(screen.getByText("1"));
    expect(onPageChange).toHaveBeenCalledWith(1);
  });

  test('test click to total page', async () => {
    const onPageChange = jest.fn();

    render(
      <PaginationComponent currentPage={1} totalPages={10} onPageChange={onPageChange} />
    );
    
    fireEvent.click(screen.getByText("10"));
    expect(onPageChange).toHaveBeenCalledWith(10);
  });

});