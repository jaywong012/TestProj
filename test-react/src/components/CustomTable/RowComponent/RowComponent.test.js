import { fireEvent, render, screen } from "@testing-library/react";
import RowComponent from "./RowComponent";
import "@testing-library/jest-dom";

const handleSetEditDetail = jest.fn();
const handleDelete = jest.fn();

const renderBodyRow = (item) => (
    <td>{item.name}</td>
);

describe("RowComponent", () => {
  test("renders RowComponent click actions", async () => {
    const item1 = { id: 1, name: "Test Item" };
    const { container } = render(
      <RowComponent
        item={item1}
        renderBodyRow={renderBodyRow}
        handleSetEditDetail={handleSetEditDetail}
        handleDelete={handleDelete}
      />
    );

    const pencilSvg = container.querySelector("svg.bi-pencil");
    expect(pencilSvg).toBeInTheDocument();

    fireEvent.click(pencilSvg);
    expect(handleSetEditDetail).toHaveBeenCalledTimes(1);

    const trashSvg = container.querySelector("svg.bi-trash");
    expect(trashSvg).toBeInTheDocument();

    fireEvent.click(trashSvg);
    expect(handleDelete).toHaveBeenCalledTimes(1);
  });

  test("re-renders when item prop changes", () => {
    const item1 = { id: 1, name: "Test Item" };
    const item2 = { id: 1, name: "Updated Item" }; // New object reference, different content

    const { rerender } = render(
      <table>
        <tbody>
          <RowComponent
            item={item1}
            renderBodyRow={renderBodyRow}
            handleSetEditDetail={handleSetEditDetail}
            handleDelete={handleDelete}
          />
        </tbody>
      </table>
    );

    // Ensure the first row is present
    expect(screen.getByText("Test Item")).toBeInTheDocument();

    // Re-render with updated item (should trigger a re-render)
    rerender(
      <table>
        <tbody>
          <RowComponent
            item={item2} // NEW props (different object reference)
            renderBodyRow={renderBodyRow}
            handleSetEditDetail={handleSetEditDetail}
            handleDelete={handleDelete}
          />
        </tbody>
      </table>
    );

    // Ensure the updated content is now rendered
    expect(screen.getByText("Updated Item")).toBeInTheDocument();
  });
});
