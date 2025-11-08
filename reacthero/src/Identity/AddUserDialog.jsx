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

const AddUserDialog = ({
    open,
    onClose,
    onSubmit,
    mode = "add", // "add" أو "edit"
    initialData = null,
    loading = false,
}) => {
    const { t, i18n } = useTranslation();
    const isArabic = i18n.language === "ar";
    const convertToBase64 = (file) => {
        return new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = () => resolve(reader.result.split(",")[1]); // فقط البيانات بدون prefix
            reader.onerror = (error) => reject(error);
        });
    };
    const BlockTypeEnum = {
        Blog: t("Blog"),
        Link: t("Link"),
        PhotoGallery: t("PhotoGallery"),
        VideoGallery: t("VideoGallery"),
        HomeSlider: t("HomeSlider"),
    };
    const blockTypeOptions = Object.values(BlockTypeEnum);

    const [formData, setFormData] = useState({
        id: "",
        firstName: "",
        lastName: "",
        email: "",
        phoneNumber: "",
        password: "",
        confirmPassword: "",
        autoConfirmEmail: true,
        isActive: true,
        uploadRequest: null, 

    });

    useEffect(() => {
        if (open) {
            if (initialData) {
                setFormData({
                    id: initialData.id ?? "",
                    firstName: initialData.firstName || "",
                    lastName: initialData.lastName || "",
                    email: initialData.email || "",
                    phoneNumber: initialData.phoneNumber || "",
                    password: initialData.password || "",
                    confirmPassword: initialData.confirmPassword || "",
                    autoConfirmEmail: initialData.autoConfirmEmail ?? true,
                    isActive: initialData.isActive ?? true,
                    uploadRequest: initialData.uploadRequest ?? null,
                });
            } else {
                setFormData({
                    id: "",
                    firstName: "",
                    lastName: "",
                    email: "",
                    phoneNumber: "",
                    password: "",
                    confirmPassword: "",
                    autoConfirmEmail: true,
                    isActive: true,
                    uploadRequest: null,
                });
            }
        }
    }, [initialData, open]);

    const handleChange = (field, value) => {
        setFormData((prev) => ({ ...prev, [field]: value }));
    };

    const handleSubmit = () => {
        if (!formData.password.trim() || !formData.email.trim()) {
            alert(t("pleaseFillAllFields"));
            return;
        }
        onSubmit(formData);
    };

    return (
        <Dialog open={open} onClose={onClose} fullWidth maxWidth="sm">
            <DialogTitle dir={isArabic ? "rtl" : "ltr"}>
                {mode === "add" ? t("addBlockCategory") : t("editBlockCategory")}
            </DialogTitle>

            <DialogContent>
                <Box dir={isArabic ? "rtl" : "ltr"}>
                    <Stack spacing={2} mt={1}>
                 
                        <TextField
                            label={t("firstName")}
                            value={formData.firstName}
                            onChange={(e) => handleChange("firstName", e.target.value)}
                            fullWidth
                            required
                        />
                        
                        <TextField
                            label={t("lastName")}
                            value={formData.lastName}
                            onChange={(e) => handleChange("lastName", e.target.value)}
                            fullWidth
                            required
                        />
                        
                        <TextField
                            label={t("phoneNumber")}
                            value={formData.phoneNumber}
                            onChange={(e) => handleChange("phoneNumber", e.target.value)}
                            fullWidth
                            required
                        />
                        
                        <TextField
                            label={t("email")}
                            value={formData.email}
                            onChange={(e) => handleChange("email", e.target.value)}
                            fullWidth
                            required
                        />
                        
                        <TextField
                            label={t("password")}
                            value={formData.password}
                            onChange={(e) => handleChange("password", e.target.value)}
                            fullWidth
                            required
                        />
                        
                        <TextField
                            label={t("confirmPassword")}
                            value={formData.confirmPassword}
                            onChange={(e) => handleChange("confirmPassword", e.target.value)}
                            fullWidth
                            required
                        />


                        <FormControlLabel
                            control={
                                <Switch
                                    checked={formData.autoConfirmEmail}
                                    onChange={(e) =>
                                        handleChange("autoConfirmEmail", e.target.checked)
                                    }
                                />
                            }
                            label={t("autoConfirmEmail")}
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



                        <TextField
                            type="file"
                            label={t("profilePicture")}
                            onChange={async (e) => {
                                const file = e.target.files[0];
                                if (!file) return;

                                const base64 = await convertToBase64(file);

                                handleChange("uploadRequest", {
                                    fileName: file.name,
                                    extension: "." + file.name.split(".").pop(),
                                    uploadType: 0, // حسب نوع التحميل
                                    data: base64,
                                });
                            }}
                            fullWidth
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

export default AddUserDialog;
