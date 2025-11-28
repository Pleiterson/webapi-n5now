import { Snackbar, Alert } from "@mui/material";

interface ToastProps {
  readonly open: boolean;
  readonly message: string;
  readonly severity?: "success" | "error" | "warning" | "info";
  readonly onClose: () => void;
}

export default function Toast({
  open,
  message,
  severity = "success",
  onClose
}: ToastProps) {
  return (
    <Snackbar
      open={ open }
      autoHideDuration={ 3000 }
      onClose={ onClose }
      anchorOrigin={ { vertical: "top", horizontal: "right" } }
    >
      <Alert onClose={ onClose } severity={ severity } variant="filled">
        { message }
      </Alert>
    </Snackbar>
  );
}
