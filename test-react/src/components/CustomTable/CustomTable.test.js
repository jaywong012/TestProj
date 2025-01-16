import { render, screen, fireEvent } from "@testing-library/react";
import CustomTable from "./CustomTable";
import React from "react";

// Mock components
jest.mock("../CustomTitle/CustomTitle", () => ({ children }) => (
  <div>{children}</div>
));
jest.mock("../Spinner/Spinner", () => () => <div>Loading...</div>);

describe("CustomTable", () => {
  const headerArray = [
    { name: "Name", width: "30%" },
    { name: "Age", width: "30%" },
    { name: "Location", width: "40%" },
  ];

  const itemArray = [
    { id: 1, name: "John Doe", age: 30, location: "New York" },
    { id: 2, name: "Jane Smith", age: 25, location: "London" },
  ];

  const renderBodyRow = (item) => (
    <>
      <td>{item.name}</td>
      <td>{item.age}</td>
      <td>{item.location}</td>
    </>
  );

  const handleSetEditDetail = jest.fn();
  const handleDelete = jest.fn();

  test("should render the title and table header", () => {
    render(
      <CustomTable
        title="User List"
        headerArray={headerArray}
        itemArray={itemArray}
        renderBodyRow={renderBodyRow}
        handleSetEditDetail={handleSetEditDetail}
        handleDelete={handleDelete}
        loading={false}
      />
    );

    expect(screen.getByText("User List")).toBeInTheDocument();

    expect(screen.getByText("Name")).toBeInTheDocument();
    expect(screen.getByText("Age")).toBeInTheDocument();
    expect(screen.getByText("Location")).toBeInTheDocument();
  });

  test("should display loading spinner when loading is true", () => {
    render(
      <CustomTable
        title="User List"
        headerArray={headerArray}
        itemArray={itemArray}
        renderBodyRow={renderBodyRow}
        handleSetEditDetail={handleSetEditDetail}
        handleDelete={handleDelete}
        loading={true}
      />
    );

    expect(screen.getByText("Loading...")).toBeInTheDocument();
  });

  test("should render table rows", () => {
    render(
      <CustomTable
        title="User List"
        headerArray={headerArray}
        itemArray={itemArray}
        renderBodyRow={renderBodyRow}
        handleSetEditDetail={handleSetEditDetail}
        handleDelete={handleDelete}
        loading={false}
      />
    );

    // Check if the table rows are rendered
    expect(screen.getByText("John Doe")).toBeInTheDocument();
    expect(screen.getByText("Jane Smith")).toBeInTheDocument();
  });

  test("should call handleSetEditDetail when edit button is clicked", () => {
    render(
      <CustomTable
        title="User List"
        headerArray={headerArray}
        itemArray={itemArray}
        renderBodyRow={renderBodyRow}
        handleSetEditDetail={handleSetEditDetail}
        handleDelete={handleDelete}
        loading={false}
      />
    );

    fireEvent.click(screen.getAllByRole("button")[0]); // Click on the first edit button
    expect(handleSetEditDetail).toHaveBeenCalledWith(1);
  });

  test("should call handleDelete when delete button is clicked", () => {
    render(
      <CustomTable
        title="User List"
        headerArray={headerArray}
        itemArray={itemArray}
        renderBodyRow={renderBodyRow}
        handleSetEditDetail={handleSetEditDetail}
        handleDelete={handleDelete}
        loading={false}
      />
    );

    fireEvent.click(screen.getAllByRole("button")[1]); // Click on the first delete button
    expect(handleDelete).toHaveBeenCalledWith(1);
  });
});
