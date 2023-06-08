import React, { useState } from 'react';
import { Box, Table, TableBody, TableCell, TableHead, TableRow, TextField, TablePagination, Checkbox, IconButton } from "@mui/material";
import EditIcon from "@mui/icons-material/Edit";
import CheckIcon from "@mui/icons-material/Check";
import { IProduct } from '../../shared';

interface IProductPage {
  orders: IProduct[];
  page: number;
  totalPage: number;
}

interface ItemTableProps {
  products?: IProductPage;
}

const ItemTable: React.FC<ItemTableProps> = ({ products }) => {
  const [searchTerm, setSearchTerm] = useState("");
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(3);
  const [selectedItemIds, setSelectedItemIds] = useState<string[]>([]);
  const [isEditing, setIsEditing] = useState(false);
  const [editedItem, setEditedItem] = useState<IProduct | null>(null);

  const handleSearchChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setSearchTerm(event.target.value);
    setPage(0);
  };

  const handlePageChange = (event: React.MouseEvent<HTMLButtonElement> | null, newPage: number) => {
    setPage(newPage);
  };

  const handleRowsPerPageChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setRowsPerPage(parseInt(event.target.value,10));
    setPage(0);
  };

  const handleRowClick = (id: string) => {
    if (isEditing) return;

    const isSelected = selectedItemIds.includes(id);
    if (isSelected) {
      setSelectedItemIds((prevIds) => prevIds.filter((itemId) => itemId !== id));
    } else {
      setSelectedItemIds((prevIds) => [...prevIds, id]);
    }
  };

  const handleEditClick = () => {
    if (selectedItemIds.length === 1) {
      setIsEditing(true);
      setEditedItem(products?.orders.find((item) => item.id === selectedItemIds[0]) || null);
    }
  };

  const handleSaveClick = () => {
    if (editedItem) {
      // Handle save logic here
      setIsEditing(false);
      setEditedItem(null);
      setSelectedItemIds([]);
    }
  };

  const filteredItems = products?.orders.filter((item) =>
    item.name.toLowerCase().includes(searchTerm.toLowerCase())
  ) || [];

  const paginatedItems = filteredItems.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage);

  return (
    <Box>
      <Box bgcolor="white" borderRadius={3} p={2}>
        <Box
          display="flex"
          justifyContent="space-between"
          alignItems="center"
          marginBottom={2}
        >
          <TextField
            type="text"
            value={searchTerm}
            onChange={handleSearchChange}
            label="Поиск"
            disabled={isEditing}
          />
          {selectedItemIds.length === 1 && !isEditing && (
            <IconButton onClick={handleEditClick}>
              <EditIcon sx={{ color: "#0a1929" }} />
            </IconButton>
          )}
          {isEditing && (
            <IconButton onClick={handleSaveClick}>
              <CheckIcon sx={{ color: "green" }} />
            </IconButton>
          )}
        </Box>

        <Table sx={{ maxWidth: 400, width: "100%", backgroundColor: "white" }}>
          <TableHead>
            <TableRow>
              <TableCell></TableCell>
              <TableCell>ID</TableCell>
              <TableCell>Название</TableCell>
              <TableCell>Цена</TableCell>
              <TableCell>Статус</TableCell>
              {/* Add more table headers here */}
            </TableRow>
          </TableHead>
          <TableBody>
            {paginatedItems.map((item) => (
              <TableRow
                key={item.id}
                selected={selectedItemIds.includes(item.id)}
                onClick={() => handleRowClick(item.id)}
              >
                <TableCell padding="checkbox">
                  <Checkbox
                    checked={selectedItemIds.includes(item.id)}
                    disabled={isEditing}
                    onChange={() => {}}
                  />
                </TableCell>
                <TableCell>{item.id}</TableCell>
                <TableCell>{item.name}</TableCell>
                <TableCell>{item.total}</TableCell>
                <TableCell>{item.status}</TableCell>
                {/* Add more table cells here */}
              </TableRow>
            ))}
          </TableBody>
        </Table>

        <Box sx={{ width: "100%" }}>
          <TablePagination
            component="div"
            count={filteredItems.length}
            page={page}
            rowsPerPageOptions={[1,2,3,5,10]}
            onPageChange={handlePageChange}
            rowsPerPage={rowsPerPage}
            onRowsPerPageChange={handleRowsPerPageChange}
          />
        </Box>
      </Box>
    </Box>
  );
};

export default ItemTable;
