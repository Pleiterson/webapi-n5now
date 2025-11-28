import { api } from "./axios";
import type { Permission } from "./types";

export const PermissionsService = {
  getAll: () => api.get<Permission[]>("/permissions"),
  getById: (id: number) => api.get<Permission>(`/permissions/${id}`),
  create: (data: Omit<Permission, "id">) => api.post("/permissions", data),
  update: (id: number, data: Omit<Permission, "id">) =>
    api.put(`/permissions/${id}`, data),
  delete: (id: number) => api.delete(`/permissions/${id}`)
};
