export interface Permission {
  id: number;
  nombreEmpleado: string;
  apellidoEmpleado: string;
  tipoPermiso: number;
  fechaPermiso: string;
}

export type PermissionFormData = Omit<Permission, "id">;

export interface PermissionType {
  id: number;
  description: string;
}

export interface PermissionTypeFormData {
  description: string;
}
