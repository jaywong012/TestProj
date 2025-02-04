import { render, screen, fireEvent } from '@testing-library/react';
import CustomTable from './CustomTable';
import '@testing-library/jest-dom';

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
    { id: 1, name: "John Doe" },
    { id: 2, name: "Jane Smith" },
  ];

  const renderBodyRow = (item) => (
      <td>{item.name}</td>
  );

  const handleSetEditDetail = jest.fn();
  const handleDelete = jest.fn();
  const handleDownloadFile = jest.fn();
  const setCurrentPage = jest.fn();
  const fetchDataByPaging = jest.fn();
  
  describe("CustomTable", () => {
    test("renders title and download button when provided", async () => {

      const {container} = render(
        <CustomTable
          title="Test Table"
          headerArray={headerArray}
          itemArray={itemArray}
          renderBodyRow={renderBodyRow}
          handleSetEditDetail={handleSetEditDetail}
          handleDelete={handleDelete}
          handleDownloadFile={handleDownloadFile}
        />
      );
      
      expect(screen.getByText("Test Table")).toBeInTheDocument();
      
      const downloadSvg = container.querySelector('svg.bi-download');
      expect(downloadSvg).toBeInTheDocument();

      const downloadBtn = downloadSvg.closest('button');
      fireEvent.click(downloadBtn);
      expect(handleDownloadFile).toHaveBeenCalledTimes(1);
    });

    test("renders search input when search is enabled", () => {
      render(
        <CustomTable
          title="Test Table"
          headerArray={headerArray}
          itemArray={itemArray}
          renderBodyRow={renderBodyRow}
          handleSetEditDetail={handleSetEditDetail}
          handleDelete={handleDelete}
          handleDownloadFile={handleDownloadFile}
          isSearchable={true}
        />
      );

      expect(screen.getByPlaceholderText("Search...")).toBeInTheDocument();
    });

    test("renders spinner when loading", () => {
      render(
        <CustomTable
          title="Test Table"
          headerArray={headerArray}
          itemArray={itemArray}
          renderBodyRow={renderBodyRow}
          handleSetEditDetail={handleSetEditDetail}
          handleDelete={handleDelete}
          handleDownloadFile={handleDownloadFile}
          loading={true}
        />
      )

      expect(screen.getByText("Loading...")).toBeInTheDocument();
    });


    test("renders pagination when total pages more than 1", async () => {
      const {container} = render(
        <CustomTable
          title="Test Table"
          headerArray={headerArray}
          itemArray={itemArray}
          renderBodyRow={renderBodyRow}
          handleSetEditDetail={handleSetEditDetail}
          handleDelete={handleDelete}
          handleDownloadFile={handleDownloadFile}
          loading={false}
          totalPages={10}
          currentPage={1}
          setCurrentPage={setCurrentPage}
          fetchDataByPaging={fetchDataByPaging}
        />
      )
      const downloadSvg = container.querySelector('svg.bi-download');
      expect(downloadSvg).toBeInTheDocument();

      const paginationComponent = container.querySelector('ul.pagination');
      expect(paginationComponent).toBeInTheDocument();

      fireEvent.click(screen.getByText("10"));

      expect(setCurrentPage).toHaveBeenCalledTimes(1);
      expect(fetchDataByPaging).toHaveBeenCalledTimes(1);

    });
  })
});
