import React, { useState, useEffect } from "react";
import {
    Dialog,
    DialogTitle,
    DialogContent,
    DialogActions,
    Stack,
    TextField,
    FormControlLabel,
    Switch,
    Button,
    CircularProgress,
    Box,
} from "@mui/material";
import { useTranslation } from "react-i18next";
import { API_BASE_URL } from "../../config";

const AddRoleDialog = ({
    open,
    onClose,
    onSubmit,
    mode = "add", // "add" أو "edit"
    initialData = null,
    loading = false,
}) => {
    const { t, i18n } = useTranslation();
    const isArabic = i18n.language === "ar";

    const [formData, setFormData] = useState({
        id: "",
        name: "",
        description: "",
    });

    useEffect(() => {
        if (open) {
            if (initialData) {
                setFormData({
                    id: initialData.id ?? "",
                    name: initialData.name || "",
                    description: initialData.description || "",
                });
            } else {
                setFormData({
                    id: "",
                    name: "",
                    description: "",
                });
            }
        }
    }, [initialData, open]);

    const handleChange = (field, value) => {
        setFormData((prev) => ({ ...prev, [field]: value }));
    };

    const handleSubmit = () => {
        if (!formData.name.trim()) {
            alert(t("pleaseFillAllFields"));
            return;
        }
        onSubmit(formData);
    };

    return (
        <Dialog open={open} onClose={onClose} fullWidth maxWidth="sm">
            <DialogTitle dir={isArabic ? "rtl" : "ltr"}>
                {mode === "add" ? t("addRole") : t("editRole")}
            </DialogTitle>

            <DialogContent>
                <Box dir={isArabic ? "rtl" : "ltr"}>
                    <Stack spacing={2} mt={1}>
                        <TextField
                            label={t("name")}
                            value={formData.name}
                            onChange={(e) => handleChange("name", e.target.value)}
                            fullWidth
                            required
                        />
                        <TextField
                            label={t("description")}
                            value={formData.description}
                            onChange={(e) => handleChange("description", e.target.value)}
                            fullWidth
                            required
                        />
                    </Stack>
                </Box>
            </DialogContent>

            <DialogActions dir={isArabic ? "rtl" : "ltr"}>
                <Button onClick={onClose}>{t("cancel")}</Button>
                <Button onClick={handleSubmit} variant="contained" disabled={loading}>
                    {loading ? (
                        <CircularProgress size={20} />
                    ) : mode === "add" ? (
                        t("save")
                    ) : (
                        t("update")
                    )}
                </Button>
            </DialogActions>
        </Dialog>
    );
};

export default AddRoleDialog;
