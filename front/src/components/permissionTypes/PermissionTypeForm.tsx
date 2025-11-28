import { useEffect } from "react";
import { useForm } from "react-hook-form";
import { TextField, Button, Paper } from "@mui/material";
import type { PermissionType, PermissionTypeFormData } from "../../api/types";
import { PermissionTypesService } from "../../api/permissionTypes.service";
import "./PermissionTypeForm.scss";

export interface PermissionTypeFormProps {
  readonly selected: PermissionType | null;
  readonly onSaved: () => void;
}

export default function PermissionTypeForm({ selected, onSaved }: PermissionTypeFormProps) {
  const {
    register,
    handleSubmit,
    reset,
    formState: { errors, isSubmitting }
  } = useForm<PermissionTypeFormData>({
    defaultValues: {
      description: ""
    }
  });

  useEffect(() => {
    if (selected) {
      reset(selected);
    }
  }, [selected, reset]);

  async function onSubmit(data: PermissionTypeFormData) {
    if (selected) {
      await PermissionTypesService.update(selected.id, data);
    } else {
      await PermissionTypesService.create(data);
    }

    onSaved();
    reset();
  }

  function getDescriptionError() {
    if (errors.description?.type === "required") return "Campo obligatorio";
    if (errors.description?.type === "minLength") return "Mínimo de 3 caracteres";
    return "";
  }

  return (
    <Paper elevation={ 3 } className="permissiontype-form">
      <h2>{ selected ? "Editar Tipo de Permiso" : "Nuevo Tipo de Permiso" }</h2>

      <form onSubmit={ handleSubmit(onSubmit) }>
        
        <TextField
          label="Descripción"
          fullWidth
          { ...register("description", { required: true, minLength: 3 }) }
          error={ !!errors.description }
          helperText={ getDescriptionError() }
        />

        <Button type="submit" variant="contained" fullWidth disabled={ isSubmitting }>
          { selected ? "Guardar Cambios" : "Registro" }
        </Button>
      </form>
    </Paper>
  );
}
