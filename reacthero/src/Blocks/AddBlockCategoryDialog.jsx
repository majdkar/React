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

const AddBlockCategoryDialog = ({
    open,
    onClose,
    onSubmit,
    mode = "add", // "add" أو "edit"
    initialData = null,
    loading = false,
}) => {
    const { t, i18n } = useTranslation();
    const isArabic = i18n.language === "ar";

    const BlockTypeEnum = {
        Blog: t("Blog"),
        Link: t("Link"),
        PhotoGallery: t("PhotoGallery"),
        VideoGallery: t("VideoGallery"),
        HomeSlider: t("HomeSlider"),
    };
    const blockTypeOptions = Object.values(BlockTypeEnum);

    const [formData, setFormData] = useState({
        id: 0,
        nameAr: "",
        nameEn: "",
        blockType: "",
        isActive: true,
    });

    useEffect(() => {
        if (open) {
            if (initialData) {
                setFormData({
                    id: initialData.id ?? 0,
                    nameAr: initialData.nameAr || "",
                    nameEn: initialData.nameEn || "",
                    blockType: initialData.blockType || "",
                    isActive: initialData.isActive ?? true,
                });
            } else {
                setFormData({
                    id: 0,
                    nameAr: "",
                    nameEn: "",
                    blockType: "",
                    isActive: true,
                });
            }
        }
    }, [initialData, open]);

    const handleChange = (field, value) => {
        setFormData((prev) => ({ ...prev, [field]: value }));
    };

    const handleSubmit = () => {
        if (!formData.nameAr.trim() || !formData.nameEn.trim() || !formData.blockType.trim()) {
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

                        <Autocomplete
                            fullWidth
                            options={blockTypeOptions}
                            getOptionLabel={(option) => option}
                            value={formData.blockType || null}
                            onChange={(event, newValue) =>
                                handleChange("blockType", newValue || "")
                            }
                            renderInput={(params) => (
                                <TextField {...params} label={t("selectBlockType")} required />
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

export default AddBlockCategoryDialog;
