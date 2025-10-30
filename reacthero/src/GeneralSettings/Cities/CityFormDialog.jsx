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

const CityFormDialog = ({
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
        countryId: "",
        isActive: true,
    });

    const [countries, setCountries] = useState([]);
    const [countriesLoading, setCountriesLoading] = useState(false);
    const [countriesError, setCountriesError] = useState(null);

    const token = localStorage.getItem("token");

    // 🔹 جلب الدول من الـ API
    useEffect(() => {
        const fetchCountries = async () => {
            setCountriesLoading(true);
            try {
                const response = await fetch("https://localhost:44399/api/v1/Countries/GetAll", {
                    headers: {
                        Authorization: `Bearer ${token}`,
                        "Content-Type": "application/json",
                    },
                });
                if (!response.ok) throw new Error(`HTTP error: ${response.status}`);
                const data = await response.json();
                setCountries(data.data || []);
                setCountriesError(null);
            } catch (err) {
                console.error("❌ فشل تحميل الدول:", err);
                setCountriesError(t("failedToLoadCountries"));
            } finally {
                setCountriesLoading(false);
            }
        };
        fetchCountries();
    }, [token, t]);

    // 🔹 تعبئة البيانات عند التعديل
    useEffect(() => {
        if (initialData)
            setFormData({
                id: initialData.id,
                nameAr: initialData.nameAr || "",
                nameEn: initialData.nameEn || "",
                countryId: initialData.countryId || "",
                isActive: initialData.isActive ?? true,
            });
        else
            setFormData({
                id: 0,
                nameAr: "",
                nameEn: "",
                countryId: "",
                isActive: true,
            });
    }, [initialData, open]);

    const handleChange = (field, value) => {
        setFormData({ ...formData, [field]: value });
    };

    const handleSubmit = () => {
        if (!formData.nameAr || !formData.nameEn || !formData.countryId) {
            alert(t("fillAllFields"));
            return;
        }
        onSubmit(formData);
    };

    return (
        <Dialog open={open} onClose={onClose} fullWidth maxWidth="sm">
            <DialogTitle dir={isArabic ? "rtl" : "ltr"}>
                {mode === "add" ? t("addCity") : t("editCity")}
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

                        {/* 🔍 قائمة الدول مع البحث */}
                        <Autocomplete
                            fullWidth
                            loading={countriesLoading}
                            options={countries}
                            getOptionLabel={(option) => `${option.nameAr} (${option.nameEn})`}
                            value={countries.find((c) => c.id === formData.countryId) || null}
                            onChange={(event, newValue) =>
                                handleChange("countryId", newValue ? newValue.id : "")
                            }
                            renderInput={(params) => (
                                <TextField
                                    {...params}
                                    label={t("selectCountry")}
                                    required
                                    error={!!countriesError}
                                    helperText={countriesError || ""}
                                    InputProps={{
                                        ...params.InputProps,
                                        endAdornment: (
                                            <>
                                                {countriesLoading ? (
                                                    <CircularProgress size={20} />
                                                ) : null}
                                                {params.InputProps.endAdornment}
                                            </>
                                        ),
                                    }}
                                />
                            )}
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

export default CityFormDialog;
