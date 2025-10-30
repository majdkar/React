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
    Box,
} from "@mui/material";
import { useTranslation } from "react-i18next";

const CountryFormDialog = ({
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
        nameAr: "",
        nameEn: "",
        alpha2Code: "",
        alpha3Code: "",
        phoneCode: "",
        isActive: true,
    });

    useEffect(() => {
        if (initialData) setFormData(initialData);
        else
            setFormData({
                nameAr: "",
                nameEn: "",
                alpha2Code: "",
                alpha3Code: "",
                phoneCode: "",
                isActive: true,
            });
    }, [initialData, open]);

    const handleChange = (field, value) => {
        setFormData({ ...formData, [field]: value });
    };

    const handleSubmit = () => onSubmit(formData);

    return (
        <Dialog open={open} onClose={onClose} fullWidth maxWidth="sm">
            <DialogTitle dir={isArabic ? "rtl" : "ltr"}>
                {mode === "add" ? t("addCountry") : t("editCountry")}
            </DialogTitle>
            <DialogContent>
                <Box dir={isArabic ? "rtl" : "ltr"}>
                    <Stack spacing={2} mt={1}>
                        <TextField
                            label={t("nameAr")}
                            value={formData.nameAr}
                            onChange={(e) => handleChange("nameAr", e.target.value)}
                            fullWidth
                        />
                        <TextField
                            label={t("nameEn")}
                            value={formData.nameEn}
                            onChange={(e) => handleChange("nameEn", e.target.value)}
                            fullWidth
                        />
               
                        <TextField
                            label={t("CountryCode")}
                            value={formData.phoneCode}
                            onChange={(e) => handleChange("CountryCode", e.target.value)}
                            fullWidth
                        />
                        <FormControlLabel
                            control={
                                <Switch
                                    checked={formData.isActive}
                                    onChange={(e) => handleChange("isActive", e.target.checked)}
                                />
                            }
                            label={t("isActive")}
                            sx={{ justifyContent: isArabic ? "flex-end" : "flex-start" }}
                        />
                    </Stack>
                </Box>
            </DialogContent>
            <DialogActions dir={isArabic ? "rtl" : "ltr"}>
                <Button onClick={onClose}>{t("cancel")}</Button>
                <Button onClick={handleSubmit} variant="contained" disabled={loading}>
                    {loading
                        ? t("saving")
                        : mode === "add"
                            ? t("save")
                            : t("saveChanges")}
                </Button>
            </DialogActions>
        </Dialog>
    );
};

export default CountryFormDialog;
