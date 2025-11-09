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

const ResetPasswordAndEmail = ({
    open,
    onClose,
    onSubmit,
    initialData = null,
    loading = false,
}) => {
    const { t, i18n } = useTranslation();
    const isArabic = i18n.language === "ar";

    const [formData, setFormData] = useState({
        OldEmail: "",
        NewEmail: "",
        Password: "",
        ConfirmPassword: "",
    });

    useEffect(() => {
        if (open) {
            if (initialData) {
                setFormData({
                    OldEmail: initialData.email ?? "",
                    NewEmail: initialData.email || "",
                    Password: initialData.Password || "",
                    ConfirmPassword: initialData.ConfirmPassword || "",
                });
            } else {
                setFormData({
                    id: "",
                    name: "",
                    description: "",
                    ConfirmPassword: "",
                });
            }
        }
    }, [initialData, open]);

    const handleChange = (field, value) => {
        setFormData((prev) => ({ ...prev, [field]: value }));
    };

    const handleSubmit = () => {
        if (!formData.Password.trim() && !formData.Con.trim()) {
            alert(t("pleaseFillAllFields"));
            return;
        }
        onSubmit(formData);
    };

    return (
        <Dialog open={open} onClose={onClose} fullWidth maxWidth="sm">
            <DialogTitle dir={isArabic ? "rtl" : "ltr"}>
                {t("Reset Password") }
            </DialogTitle>

            <DialogContent>
                <Box dir={isArabic ? "rtl" : "ltr"}>
                    <Stack spacing={2} mt={1}>
                        <TextField
                            label={t("Email")}
                            value={formData.OldEmail}
                            onChange={(e) => handleChange("OldEmail", e.target.value)}
                            fullWidth
                            required
                        />
                        <TextField
                            label={t("Password")}
                            value={formData.Password}
                            onChange={(e) => handleChange("Password", e.target.value)}
                            fullWidth
                            required
                        />
                        <TextField
                            label={t("ConfirmPassword")}
                            value={formData.ConfirmPassword}
                            onChange={(e) => handleChange("ConfirmPassword", e.target.value)}
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
                    ) : 
                        t("Reset")  }
                </Button>
            </DialogActions>
        </Dialog>
    );
};

export default ResetPasswordAndEmail;
