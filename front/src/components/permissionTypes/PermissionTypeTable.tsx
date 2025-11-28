import { useEffect, useState } from "react";
import {
  Table,
  TableHead,
  TableBody,
  TableRow,
  TableCell,
  TableContainer,
  Paper,
  IconButton,
  Typography,
  Stack,
  CircularProgress,
} from "@mui/material";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";
import { PermissionTypesService } from "../../api/permissionTypes.service";
import type { PermissionType } from "../../api/types";
import "./PermissionTypeTable.scss";

interface PermissionTypeTableProps {
  readonly onEdit: (type: PermissionType) => void;
  readonly reload: number;
}

export default function PermissionTypeTable({ onEdit, reload }: PermissionTypeTableProps) {
  const [types, setTypes] = useState<PermissionType[]>([]);
  const [loading, setLoading] = useState(true);

  async function load() {
    try {
      const response = await PermissionTypesService.getAll();
      setTypes(response.data);
    } catch (err) {
      console.error("Error al cargar los tipos de permisos:", err);
    } finally {
      setLoading(false);
    }
  }

  async function handleDelete(id: number) {
    if (!confirm("¿Realmente desea eliminar este tipo de permiso?")) return;

    try {
      await PermissionTypesService.delete(id);
      setTypes((prev) => prev.filter((t) => t.id !== id));
    } catch (err) {
      console.error(err);
      alert("Error al eliminar el tipo de permiso.");
    }
  }

  useEffect(() => {
    load();
  }, []);

  useEffect(() => {
    load();
  }, [reload]);

  if (loading)
    return (
      <Stack alignItems="center" mt={ 4 }>
        <CircularProgress />
      </Stack>
    );

  return (
    <TableContainer component={ Paper } className="pt-table" elevation={ 2 }>
      <Table>
        <TableHead>
          <TableRow>
            <TableCell><strong>ID</strong></TableCell>
            <TableCell><strong>Descripción</strong></TableCell>
            <TableCell align="center"><strong>Comportamiento</strong></TableCell>
          </TableRow>
        </TableHead>

        <TableBody>
          { types.map((t) => (
            <TableRow key={ t.id }>
              <TableCell>{ t.id }</TableCell>
              <TableCell>{ t.description }</TableCell>

              <TableCell align="center">
                <IconButton color="primary" onClick={ () => onEdit(t) }>
                  <EditIcon />
                </IconButton>

                <IconButton color="error" onClick={ () => handleDelete(t.id) }>
                  <DeleteIcon />
                </IconButton>
              </TableCell>
            </TableRow>
          )) }

          { types.length === 0 && (
            <TableRow>
              <TableCell colSpan={ 3 } align="center">
                <Typography mt={ 2 } color="text.secondary">
                  No se encontró ningún tipo de permiso...
                </Typography>
              </TableCell>
            </TableRow>
          ) }
        </TableBody>
      </Table>
    </TableContainer>
  );
}
