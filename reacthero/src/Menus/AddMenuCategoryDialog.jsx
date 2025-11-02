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
    Autocomplete,
    Box,
} from "@mui/material";
import { useTranslation } from "react-i18next";

const AddMenuCategoryDialog = ({
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
        id: 0,
        nameAr: "",
        nameEn: "",
        IsVisibleUser: true,
        isActive: true,
    });

    useEffect(() => {
        if (open) {
            if (initialData) {
                setFormData({
                    id: initialData.id ?? 0,
                    nameAr: initialData.nameAr || "",
                    nameEn: initialData.nameEn || "",
                    IsVisibleUser: initialData.IsVisibleUser ?? true,
                    isActive: initialData.isActive ?? true,
                });
            } else {
                setFormData({
                    id: 0,
                    nameAr: "",
                    nameEn: "",
                    IsVisibleUser: true,
                    isActive: true,
                });
            }
        }
    }, [initialData, open]);

    const handleChange = (field, value) => {
        setFormData((prev) => ({ ...prev, [field]: value }));
    };

    const handleSubmit = () => {
        if (!formData.nameAr.trim() || !formData.nameEn.trim()) {
            alert(t("pleaseFillAllFields"));
            return;
        }
        onSubmit(formData);
    };

    return (
        <Dialog open={open} onClose={onClose} fullWidth maxWidth="sm">
            <DialogTitle dir={isArabic ? "rtl" : "ltr"}>
                {mode === "add" ? t("addMenuCategory") : t("editMenuCategory")}
            </DialogTitle>

            <DialogContent>
                <Box dir={isArabic ? "rtl" : "ltr"}>
                    <Stack spacing={2} mt={1}>
                        <TextField
                            label={t("nameAr")}
                            value={formData.nameAr}
                            onChange={(e) => handleChange("nameAr", e.target.value)}
                            fullWidth
                            required
                        />
                        <TextField
                            label={t("nameEn")}
                            value={formData.nameEn}
                            onChange={(e) => handleChange("nameEn", e.target.value)}
                            fullWidth
                            required
                        />


                        <FormControlLabel
                            control={
                                <Switch
                                    checked={formData.IsVisibleUser}
                                    onChange={(e) =>
                                        handleChange("IsVisibleUser", e.target.checked)
                                    }
                                />
                            }
                            label={t("IsVisibleUser")}
                            sx={{
                                justifyContent: isArabic ? "flex-end" : "flex-start",
                            }}
                        />

                        <FormControlLabel
                            control={
                                <Switch
                                    checked={formData.isActive}
                                    onChange={(e) =>
                                        handleChange("isActive", e.target.checked)
                                    }
                                />
                            }
                            label={t("isActive")}
                            sx={{
                                justifyContent: isArabic ? "flex-end" : "flex-start",
                            }}
                        />
                    </Stack>
                </Box>
            </DialogContent>

            <DialogActions dir={isArabic ? "rtl" : "ltr"}>
                <Button onClick={onClose}>{t("cancel")}</Button>
                <Button onClick={handleSubmit} variant="contained" disabled={loading}>
                    {loading ? <CircularProgress size={20} /> : mode === "add" ? t("save") : t("update")}
                </Button>
            </DialogActions>
        </Dialog>
    );
};

export default AddMenuCategoryDialog;
