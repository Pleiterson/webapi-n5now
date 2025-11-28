import { useState } from "react";
import type { PermissionType } from "../../api/types";
import PermissionTypeForm from "../../components/permissionTypes/PermissionTypeForm";
import PermissionTypeTable from "../../components/permissionTypes/PermissionTypeTable";

export default function PermissionTypesPage() {
  const [selected, setSelected] = useState<PermissionType | null>(null);
  const [reloadTrigger, setReloadTrigger] = useState(0);

  function reloadTable() {
    setReloadTrigger((prev) => prev + 1);
  }

  return (
    <div className="grid-Form-Table">
      <PermissionTypeForm
        selected={ selected }
        onSaved={ () => {
          setSelected(null);
          reloadTable();
        } }
      />

      <PermissionTypeTable onEdit={ (item) => setSelected(item) } reload={ reloadTrigger } />
    </div>
  );
}
