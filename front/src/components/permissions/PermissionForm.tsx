import { useEffect, useState } from "react";
import { useForm } from "react-hook-form";
import { TextField, Button, MenuItem, Paper, CircularProgress } from "@mui/material";
import { PermissionsService } from "../../api/permissions.service";
import { PermissionTypesService } from "../../api/permissionTypes.service";
import type { Permission, PermissionFormData, PermissionType } from "../../api/types";
import Toast from "../feedback/Toast";
import "./PermissionForm.scss";

export interface PermissionFormProps {
  readonly selected: Permission | null;
  readonly onSaved: () => void;
}

export default function PermissionForm({ selected, onSaved }: PermissionFormProps) {
  const [types, setTypes] = useState<PermissionType[]>([]);
  const [loadingTypes, setLoadingTypes] = useState(true);
  const [toast, setToast] = useState({
    open: false,
    message: "",
    severity: "success" as "success" | "error" | "warning" | "info"
  });

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors, isSubmitting }
  } = useForm({
    defaultValues: {
      nombreEmpleado: "",
      apellidoEmpleado: "",
      tipoPermiso: 1,
      fechaPermiso: ""
    }
  });

    useEffect(() => {
    async function loadTypes() {
      try {
        const response = await PermissionTypesService.getAll();
        setTypes(response.data);
      } catch (err) {
        console.error("Error al cargar los tipos de permisos.", err);
      } finally {
        setLoadingTypes(false);
      }
    }

    loadTypes();
  }, []);

  useEffect(() => {
    if (selected) {
      reset({
        ...selected,
        fechaPermiso: selected.fechaPermiso?.split("T")[0] || ""
      });
    }
  }, [selected, reset]);

  async function onSubmit(data: PermissionFormData) {
    try {
      if (selected) {
        await PermissionsService.update(selected.id, data);

        setToast({
          open: true,
          message: "¡Permiso actualizado exitosamente!",
          severity: "success"
        });
      } else {
        await PermissionsService.create(data);

        setToast({
          open: true,
          message: "¡Permiso creado exitosamente!",
          severity: "success"
        });
      }

      onSaved();
      reset();
    } catch (err) {
      console.error(err);

      setToast({
        open: true,
        message: "¡Error al guardar datos!",
        severity: "error"
      });
    }
  }

  return (
    <>
      <Paper elevation={ 3 } className="permission-form">
        <h2>{ selected ? "Permiso de Edición" : "Nuevo Permiso" }</h2>

        <form onSubmit={ handleSubmit(onSubmit) }>
          <TextField
            label="Nombre del Empleado"
            fullWidth
            { ...register("nombreEmpleado", { required: true }) }
            error={ !!errors.nombreEmpleado }
            helperText={ errors.nombreEmpleado && "Campo obrigatório" }
          />

          <TextField
            label="Apellido"
            fullWidth
            { ...register("apellidoEmpleado", { required: true }) }
            error={ !!errors.apellidoEmpleado }
            helperText={ errors.apellidoEmpleado && "Campo obrigatório" }
          />

          <TextField
            label="Tipo de Permiso"
            select
            fullWidth
            { ...register("tipoPermiso", { required: true }) }
          >
            {loadingTypes ? (
              <MenuItem disabled>
                <CircularProgress size={ 18 } />
                &nbsp; Cargando...
              </MenuItem>
            ) : (
              types.map((t) => (
                <MenuItem key={ t.id } value={ t.id }>
                  { t.description }
                </MenuItem>
              ))
            )}
          </TextField>

          <TextField
            type="date"
            label="Fecha"
            fullWidth
            slotProps={{ inputLabel: { shrink: true } }}
            { ...register("fechaPermiso", { required: true }) }
            error={ !!errors.fechaPermiso }
            helperText={ errors.fechaPermiso && "Campo obrigatório" }
          />

          <Button type="submit" variant="contained" fullWidth disabled={ isSubmitting }>
            { selected ? "Guardar Cambios" : "Registro" }
          </Button>
        </form>
      </Paper>

      <Toast
        open={ toast.open }
        message={ toast.message }
        severity={ toast.severity }
        onClose={ () => setToast({ ...toast, open: false }) }
      />
    </>
  );
}
