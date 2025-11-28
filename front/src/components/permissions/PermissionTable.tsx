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
  CircularProgress,
  Stack,
} from "@mui/material";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";
import { PermissionsService } from "../../api/permissions.service";
import type { Permission } from "../../api/types";
import Toast from "../feedback/Toast";
import "./PermissionTable.scss";

interface PermissionTableProps {
 readonly  onEdit: (p: Permission) => void;
 readonly reload: number;
}

export default function PermissionTable({ onEdit, reload }: PermissionTableProps) {
  const [permissions, setPermissions] = useState<Permission[]>([]);
  const [loading, setLoading] = useState(true);

  const [toast, setToast] = useState({
    open: false,
    message: "",
    severity: "success" as "success" | "error" | "warning" | "info"
  });

  async function loadData() {
    try {
      const response = await PermissionsService.getAll();
      setPermissions(response.data);
    } catch (err) {
      console.error("Error al cargar permisos.", err);
    } finally {
      setLoading(false);
    }
  }

  async function handleDelete(id: number) {
    if (!confirm("Deseja realmente excluir este registro?")) return;

    try {
      await PermissionsService.delete(id);
      setPermissions((prev) => prev.filter((p) => p.id !== id));

      setToast({
        open: true,
        message: "¡Registro eliminado exitosamente!",
        severity: "success"
      });
    } catch (err) {
      console.error(err);

      setToast({
        open: true,
        message: "¡Error al eliminar registro!",
        severity: "error"
      });
    }
  }

  useEffect(() => {
    loadData();
  }, []);

  useEffect(() => {
    loadData();
  }, [reload]);

  if (loading)
    return (
      <Stack alignItems="center" mt={ 4 }>
        <CircularProgress />
      </Stack>
    );

  return (
    <>
      <TableContainer component={ Paper } elevation={ 2 } sx={{ borderRadius: 2 }}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>
                <strong>ID</strong>
              </TableCell>
              <TableCell>
                <strong>Nombre</strong>
              </TableCell>
              <TableCell>
                <strong>Apellido</strong>
              </TableCell>
              <TableCell>
                <strong>Tipo</strong>
              </TableCell>
              <TableCell>
                <strong>Fecha</strong>
              </TableCell>
              <TableCell align="center">
                <strong>Comportamiento</strong>
              </TableCell>
            </TableRow>
          </TableHead>

          <TableBody>
            { permissions.map((p) => (
              <TableRow key={ p.id }>
                <TableCell>{ p.id }</TableCell>
                <TableCell>{ p.nombreEmpleado }</TableCell>
                <TableCell>{ p.apellidoEmpleado }</TableCell>
                <TableCell>{ p.tipoPermiso }</TableCell>
                <TableCell>
                  { new Date(p.fechaPermiso).toLocaleDateString("pt-BR") }
                </TableCell>

                <TableCell align="center">
                  <IconButton color="primary" onClick={ () => onEdit(p) }>
                    <EditIcon />
                  </IconButton>

                  <IconButton color="error" onClick={ () => handleDelete(p.id) }>
                    <DeleteIcon />
                  </IconButton>
                </TableCell>
              </TableRow>
            )) }

            { permissions.length === 0 && (
              <TableRow>
                <TableCell colSpan={ 6 } align="center">
                  <Typography mt={ 2 } color="text.secondary">
                    No se encontraron registros...
                  </Typography>
                </TableCell>
              </TableRow>
            ) }
          </TableBody>
        </Table>
      </TableContainer>

      <Toast
        open={ toast.open}
        message={ toast.message }
        severity={ toast.severity }
        onClose={() => setToast({ ...toast, open: false }) }
      />
    </>
  );
}
