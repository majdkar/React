import React, { useState, useEffect } from "react";
import {
    Box, Typography, TextField, Button, Stack, Autocomplete, CircularProgress, FormControlLabel, Switch
} from "@mui/material";
import { useNavigate, useParams } from "react-router-dom";
import { API_BASE_URL } from "../../config";
import { Editor } from "@tinymce/tinymce-react";
import { useTranslation } from "react-i18next";

const AddBlockPage = () => {
    const { categoryId } = useParams();
    const navigate = useNavigate();
    const token = localStorage.getItem("token");
    const { t, i18n } = useTranslation();
    const isArabic = i18n.language === "ar";

    // TinyMCE states
    const [descriptionEn, setDescriptionEn] = useState("");
    const [descriptionAr, setDescriptionAr] = useState("");
    const [descriptionEn1, setDescriptionEn1] = useState("");
    const [descriptionAr1, setDescriptionAr1] = useState("");
    const [descriptionEn2, setDescriptionEn2] = useState("");
    const [descriptionAr2, setDescriptionAr2] = useState("");
    const [descriptionEn3, setDescriptionEn3] = useState("");
    const [descriptionAr3, setDescriptionAr3] = useState("");

    // form data
    const [formData, setFormData] = useState({
        nameEn: "",
        nameAr: "",
        categoryId: categoryId || "",
        CreateAt: "",
        recordOrder: 0,
        url: "",
        blockId: null,
        isActive: false,
        isVisible: false
    });

    const [categories, setCategories] = useState([]);
    const [categoriesLoading, setCategoriesLoading] = useState(false);
    const [categoriesError, setCategoriesError] = useState(null);

    const [parentBlocks, setParentBlocks] = useState([]);
    const [parentBlocksLoading, setParentBlocksLoading] = useState(false);
    const [parentBlocksError, setParentBlocksError] = useState(null);

    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");

    // fetch categories
    useEffect(() => {
        const fetchCategories = async () => {
            setCategoriesLoading(true);
            try {
                const response = await fetch(`${API_BASE_URL}/BlockCategories/all`, {
                    headers: {
                        Authorization: `Bearer ${token}`,
                        "Content-Type": "application/json"
                    }
                });
                if (!response.ok) throw new Error(`HTTP error: ${response.status}`);
                const data = await response.json();
                setCategories(data || []);
                setCategoriesError(null);
            } catch (err) {
                setCategoriesError(t("failedToLoadCategories"));
            } finally {
                setCategoriesLoading(false);
            }
        };
        fetchCategories();
    }, [token, t]);

    // fetch parent blocks
    useEffect(() => {
        const fetchParentBlock = async () => {
            setParentBlocksLoading(true);
            try {
                const response = await fetch(`${API_BASE_URL}/Blocks/NoCategory`, {
                    headers: {
                        Authorization: `Bearer ${token}`,
                        "Content-Type": "application/json",
                    },
                });
                if (!response.ok) throw new Error(`HTTP error: ${response.status}`);
                const data = await response.json();
                setParentBlocks(data.items || []);
                setParentBlocksError(null);
            } catch (err) {
                setParentBlocksError(t("failedToLoadParentBlock"));
            } finally {
                setParentBlocksLoading(false);
            }
        };
        fetchParentBlock();
    }, [token, t]);

    // handle form changes
    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({ ...prev, [name]: value }));
    };

    const handleChangeValue = (name, value) => {
        setFormData(prev => ({ ...prev, [name]: value }));
    };

    // submit form
    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);
        setError("");

        try {
            const payload = {
                ...formData,
                descriptionEn,
                descriptionAr,
                descriptionEn1,
                descriptionAr1,
                descriptionEn2,
                descriptionAr2,
                descriptionEn3,
                descriptionAr3
            };

            const response = await fetch(`${API_BASE_URL}/Blocks`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${token}`,
                },
                body: JSON.stringify(payload)
            });

            if (!response.ok) throw new Error(await response.text());

            navigate(`/blocks/${categoryId}`);
        } catch (err) {
            setError("حدث خطأ أثناء الحفظ ❌");
        } finally {
            setLoading(false);
        }
    };

    // editor config
    const editorInit = {
        height: 400,
        menubar: true,
        plugins: [
            "advlist autolink lists link image charmap print preview anchor",
            "searchreplace visualblocks code fullscreen",
            "insertdatetime media table paste code help wordcount"
        ],
        toolbar:
            "undo redo | formatselect | bold italic underline strikethrough | \
            alignleft aligncenter alignright alignjustify | \
            bullist numlist outdent indent | \
            link image media | forecolor backcolor | removeformat | help",
        automatic_uploads: true,
        images_upload_url: `${API_BASE_URL}/upload`,
        license_key: "gpl"
    };

    return (
        <Box sx={{ p: 3, mx: "auto", maxWidth: 1200 }}>
            <Typography variant="h5" sx={{ mb: 2, fontWeight: "bold" }}>
                {t("addBlock") || "إضافة كتلة جديدة"}
            </Typography>

            <form onSubmit={handleSubmit}>
                <Stack spacing={1}  >
                    {/* Name fields side by side */}
                    <Stack direction="row" spacing={0} sx={{ gap: 2 }}>
                        <TextField
                            label={t("nameEn") || "Name (EN)"}
                            name="nameEn"
                            value={formData.nameEn}
                            onChange={handleChange}
                            required
                            sx={{ flex: 1 }}
                        />
                        <TextField
                            label={t("nameAr") || "Name (AR)"}
                            name="nameAr"
                            value={formData.nameAr}
                            onChange={handleChange}
                            required
                            sx={{ flex: 1 }}
                        />
                    </Stack>

                    {/* Editors */}
                    <Editor tinymceScriptSrc="/tinymce/tinymce.min.js" value={descriptionEn} init={editorInit} onEditorChange={setDescriptionEn} />
                    <Editor tinymceScriptSrc="/tinymce/tinymce.min.js" value={descriptionAr} init={editorInit} onEditorChange={setDescriptionAr} />
                    <Editor tinymceScriptSrc="/tinymce/tinymce.min.js" value={descriptionEn1} init={editorInit} onEditorChange={setDescriptionEn1} />
                    <Editor tinymceScriptSrc="/tinymce/tinymce.min.js" value={descriptionAr1} init={editorInit} onEditorChange={setDescriptionAr1} />
                    <Editor tinymceScriptSrc="/tinymce/tinymce.min.js" value={descriptionEn2} init={editorInit} onEditorChange={setDescriptionEn2} />
                    <Editor tinymceScriptSrc="/tinymce/tinymce.min.js" value={descriptionAr2} init={editorInit} onEditorChange={setDescriptionAr2} />
                    <Editor tinymceScriptSrc="/tinymce/tinymce.min.js" value={descriptionEn3} init={editorInit} onEditorChange={setDescriptionEn3} />
                    <Editor tinymceScriptSrc="/tinymce/tinymce.min.js" value={descriptionAr3} init={editorInit} onEditorChange={setDescriptionAr3} />

                    {/* Date & numeric side by side */}
                    <Stack direction="row" spacing={0} sx={{ gap: 2 }}>
                        <TextField
                            label={t("createAt") || "CreateAt"}
                            name="CreateAt"
                            value={formData.CreateAt}
                            onChange={handleChange}
                            required
                            type="date"
                            sx={{ flex: 1 }}
                            InputLabelProps={{ shrink: true }}
                        />
                        <TextField
                            label={t("recordOrder") || "Order"}
                            name="recordOrder"
                            value={formData.recordOrder}
                            onChange={handleChange}
                            required
                            type="number"
                            inputProps={{ min: 0, step: 1 }}
                            sx={{ flex: 1 }}
                        />
                    </Stack>

                    {/* Autocomplete & URL */}
                    <Stack direction="row" spacing={0} sx={{ gap: 2 }}>
                        <Autocomplete
                            fullWidth
                            loading={categoriesLoading}
                            options={categories}
                            getOptionLabel={(option) => `${option.nameAr} (${option.nameEn})`}
                            value={categories.find((c) => c.id === formData.categoryId) || null}
                            onChange={(event, newValue) => handleChangeValue("categoryId", newValue ? newValue.id : "")}
                            renderInput={(params) => (
                                <TextField
                                    {...params}
                                    label={t("selectCategory")}
                                    required
                                    error={!!categoriesError}
                                    helperText={categoriesError || ""}
                                    InputProps={{
                                        ...params.InputProps,
                                        endAdornment: (
                                            <>
                                                {categoriesLoading ? <CircularProgress size={20} /> : null}
                                                {params.InputProps.endAdornment}
                                            </>
                                        ),
                                    }}
                                />
                            )}
                            sx={{ flex: 1 }}
                        />
                        <TextField
                            label={t("url") || "Url"}
                            name="url"
                            value={formData.url}
                            onChange={handleChange}
                            required
                            sx={{ flex: 1 }}
                        />
                    </Stack>

                    {/* Parent block */}
                    <Autocomplete
                        fullWidth
                        loading={parentBlocksLoading}
                        options={parentBlocks}
                        getOptionLabel={(option) => `${option.nameAr} (${option.nameEn})`}
                        value={parentBlocks.find((c) => c.id === formData.parentId) || null}
                        onChange={(event, newValue) => handleChangeValue("parentId", newValue ? newValue.id : null)}
                        renderInput={(params) => (
                            <TextField
                                {...params}
                                label={t("selectparentBlock")}
                                required
                                error={!!parentBlocksError}
                                helperText={parentBlocksError || ""}
                                InputProps={{
                                    ...params.InputProps,
                                    endAdornment: (
                                        <>
                                            {parentBlocksLoading ? <CircularProgress size={20} /> : null}
                                            {params.InputProps.endAdornment}
                                        </>
                                    ),
                                }}
                            />
                        )}
                    />

                    {/* Switches */}
                    <Stack direction="row" spacing={2}>
                        <FormControlLabel
                            control={<Switch checked={formData.isActive} onChange={(e) => handleChangeValue("isActive", e.target.checked)} />}
                            label={t("isActive")}
                            sx={{ justifyContent: isArabic ? "flex-end" : "flex-start" }}
                        />
                        <FormControlLabel
                            control={<Switch checked={formData.isVisible} onChange={(e) => handleChangeValue("isVisible", e.target.checked)} />}
                            label={t("isVisible")}
                            sx={{ justifyContent: isArabic ? "flex-end" : "flex-start" }}
                        />
                    </Stack>

                    {error && <Typography color="error">{error}</Typography>}

                    <Stack direction="row" spacing={2} sx={ {gap:2}}>
                        <Button type="submit" variant="contained" color="primary" disabled={loading}>
                            {loading ? t("proccesing") : t("save")}
                        </Button>
                        <Button variant="outlined" color="secondary" onClick={() => navigate(-1)}>
                            {t("cancel")}
                        </Button>
                    </Stack>
                </Stack>
            </form>
        </Box>
    );
};

export default AddBlockPage;
