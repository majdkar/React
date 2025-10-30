import React from "react";
import {
    Dialog,
    DialogTitle,
    DialogContent,
    DialogActions,
    Button,
    Typography,
    Stack,
    IconButton,
    CircularProgress,
} from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import WarningAmberIcon from "@mui/icons-material/WarningAmber";

const ConfirmDialog = ({
    open,
    onClose,
    onConfirm,
    title = "تأكيد العملية",
    message = "هل أنت متأكد من تنفيذ هذا الإجراء؟",
    confirmText = "تأكيد",
    cancelText = "إلغاء",
    loading = false,
    icon = <WarningAmberIcon sx={{ color: "error.main", fontSize: 36 }} />,
}) => {
    return (
        <Dialog
            open={open}
            onClose={onClose}
            maxWidth="sm"
            fullWidth
            PaperProps={{
                sx: { borderRadius: 3, p: 2 },
            }}
        >
            <Stack direction="row" alignItems="center" justifyContent="space-between">
                <DialogTitle
                    sx={{
                        fontWeight: "bold",
                        display: "flex",
                        alignItems: "center",
                        gap: 1,
                    }}
                >
                    {icon}
                    {title}
                </DialogTitle>
                <IconButton onClick={onClose}>
                    <CloseIcon />
                </IconButton>
            </Stack>

            <DialogContent sx={{ textAlign: "center" }}>
                <Typography sx={{ mt: 1, fontSize: 18 }}>{message}</Typography>
            </DialogContent>

            <DialogActions sx={{ justifyContent: "center", pb: 2 }}>
                <Button
                    onClick={onClose}
                    variant="outlined"
                    sx={{ minWidth: 100 }}
                    disabled={loading}
                >
                    {cancelText}
                </Button>
                <Button
                    color="error"
                    variant="contained"
                    onClick={onConfirm}
                    sx={{ minWidth: 100 }}
                    disabled={loading}
                >
                    {loading ? <CircularProgress size={24} color="inherit" /> : confirmText}
                </Button>
            </DialogActions>
        </Dialog>
    );
};

export default ConfirmDialog;
