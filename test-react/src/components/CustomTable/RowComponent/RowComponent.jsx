import { memo } from "react";
import { Pencil, Trash } from "react-bootstrap-icons";

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

export default RowComponent;
