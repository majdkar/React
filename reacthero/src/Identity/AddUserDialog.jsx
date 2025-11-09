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

    const [formData, setFormData] = useState({
        id: "",
        firstName: "",
        lastName: "",
        email: "",
        phoneNumber: "",
        password: "",
        pictureUrl: "",
        confirmPassword: "",
        autoConfirmEmail: true,
        isActive: true,
        uploadRequest: null,
    });

    const [previewImage, setPreviewImage] = useState(null);

    // تحويل الملف إلى Base64
    const convertToBase64 = (file) => {
        return new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = () => resolve(reader.result.split(",")[1]); // فقط البيانات بدون prefix
            reader.onerror = (error) => reject(error);
        });
    };

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
                    pictureUrl: initialData.pictureUrl ?? "",
                    uploadRequest: null, // سيبقى null حتى يرفع المستخدم صورة جديدة
                });

                // إعداد المعاينة: إذا هناك صورة جديدة أو URL موجود مسبقاً
                if (initialData.pictureUrl) {
                    setPreviewImage(`${API_BASE_URL}` + initialData.pictureUrl);
                } else {
                    setPreviewImage(null);
                }
            } else {
                setFormData({
                    id: "",
                    firstName: "",
                    lastName: "",
                    email: "",
                    phoneNumber: "",
                    password: "",
                    pictureUrl: "",
                    confirmPassword: "",
                    autoConfirmEmail: true,
                    isActive: true,
                    uploadRequest: null,
                });
                setPreviewImage(null);
            }
        }
    }, [initialData, open]);

    const handleChange = (field, value) => {
        setFormData((prev) => ({ ...prev, [field]: value }));
    };

    const handleSubmit = () => {
        if (mode === "add") {
            if(!formData.password.trim() || !formData.email.trim()) {
                alert(t("pleaseFillAllFields"));
                return;
            }
        }
        onSubmit(formData);
    };

    return (
        <Dialog open={open} onClose={onClose} fullWidth maxWidth="sm">
            <DialogTitle dir={isArabic ? "rtl" : "ltr"}>
                {mode === "add" ? t("addUser") : t("editUser")}
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

                        {mode === "add" ?
                            <TextField
                                label={t("email")}
                                value={formData.email}
                                onChange={(e) => handleChange("email", e.target.value)}
                                fullWidth
                                required
                            /> : ""}


                        {mode === "add" ?  <TextField
                            label={t("password")}
                            value={formData.password}
                            onChange={(e) => handleChange("password", e.target.value)}
                            fullWidth
                            required
                        /> : ""}

                        {mode === "add" ? <TextField
                            label={t("confirmPassword")}
                            value={formData.confirmPassword}
                            onChange={(e) => handleChange("confirmPassword", e.target.value)}
                            fullWidth
                            required
                        /> : "" }



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
                            sx={{ justifyContent: isArabic ? "flex-end" : "flex-start" }}
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
                            sx={{ justifyContent: isArabic ? "flex-end" : "flex-start" }}
                        />

                        <Box mt={2} textAlign="center">
                            <input
                                accept="image/*"
                                id="upload-image"
                                type="file"
                                style={{ display: "none" }}
                                onChange={async (e) => {
                                    const file = e.target.files[0];
                                    if (!file) return;

                                    const base64 = await convertToBase64(file);

                                    handleChange("uploadRequest", {
                                        fileName: file.name,
                                        extension: "." + file.name.split(".").pop(),
                                        uploadType: 0,
                                        data: base64,
                                    });

                                    setPreviewImage(`data:image/${file.name.split(".").pop()};base64,${base64}`);
                                }}
                            />
                            <label htmlFor="upload-image">
                                <Box
                                    sx={{
                                        width: 150,
                                        height: 150,
                                        border: "2px dashed #1976d2",
                                        borderRadius: 2,
                                        cursor: "pointer",
                                        display: "flex",
                                        alignItems: "center",
                                        justifyContent: "center",
                                        overflow: "hidden",
                                        mx: "auto",
                                        position: "relative",
                                    }}
                                >
                                    {previewImage ? (
                                        <img
                                            src={previewImage}
                                            alt="Preview"
                                            style={{ width: "100%", height: "100%", objectFit: "cover" }}
                                        />
                                    ) : (
                                        <span style={{ color: "#1976d2" }}>{t("uploadProfilePicture")}</span>
                                    )}
                                </Box>
                            </label>
                            {previewImage && (
                                <Button
                                    color="error"
                                    size="small"
                                    sx={{ mt: 1 }}
                                    onClick={() => {
                                        setPreviewImage(null);
                                        handleChange("pictureUrl", null);
                                    }}
                                >
                                    {t("remove")}
                                </Button>
                            )}
                        </Box>

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

export default AddUserDialog;
