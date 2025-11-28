import { useState } from "react";
import type { Permission } from "../../api/types";
import PermissionForm from "../../components/permissions/PermissionForm";
import PermissionTable from "../../components/permissions/PermissionTable";

export const PermissionsPage = () => {
  const [selected, setSelected] = useState<Permission | null>(null);
  const [reloadTrigger, setReloadTrigger] = useState(0);

  function reloadTable() {
    setReloadTrigger((prev) => prev + 1);
  }
  return (
    <div className="grid-Form-Table">
      <PermissionForm
        selected={ selected }
        onSaved={ () => {
          setSelected(null)
          reloadTable()
        } }
      />
      <PermissionTable onEdit={ (item) => setSelected(item) } reload={ reloadTrigger } />
    </div>
  );
};
