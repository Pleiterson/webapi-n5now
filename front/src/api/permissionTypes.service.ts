import { api } from "./axios";
import type { PermissionType } from "./types";

export const PermissionTypesService = {
  getAll: () => api.get<PermissionType[]>("/permissiontypes"),
  getById: (id: number) => api.get<PermissionType>(`/permissiontypes/${id}`),
  create: (data: Omit<PermissionType, "id">) => api.post("/permissiontypes", data),
  update: (id: number, data: Omit<PermissionType, "id">) =>
    api.put(`/permissiontypes/${id}`, data),
  delete: (id: number) => api.delete(`/permissiontypes/${id}`)
};
