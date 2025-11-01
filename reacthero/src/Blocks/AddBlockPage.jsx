import React, { useState, useEffect } from "react";
import {
    Box,
    Typography,
    TextField,
    Button,
    Stack,
    Autocomplete,
    FormControlLabel,
    Switch,
} from "@mui/material";
import { useNavigate, useParams } from "react-router-dom";
import { API_BASE_URL } from "../../config";
import { Editor } from "@tinymce/tinymce-react";
import { useTranslation } from "react-i18next";
import ImageUploader from "../Shared/ImageUploader";

const AddBlockPage = () => {
    const { categoryId, blockId } = useParams();
    const navigate = useNavigate();
    const token = localStorage.getItem("token");
    const { t } = useTranslation();

    const isEditMode = !!blockId;

    // TinyMCE states
    const [descriptionEn, setDescriptionEn] = useState("");
    const [descriptionAr, setDescriptionAr] = useState("");
    const [descriptionEn1, setDescriptionEn1] = useState("");
    const [descriptionAr1, setDescriptionAr1] = useState("");
    const [descriptionEn2, setDescriptionEn2] = useState("");
    const [descriptionAr2, setDescriptionAr2] = useState("");
    const [descriptionEn3, setDescriptionEn3] = useState("");
    const [descriptionAr3, setDescriptionAr3] = useState("");

    const [formData, setFormData] = useState(() => {
        if (!isEditMode) {
            const urlParams = new URLSearchParams(window.location.search);
            const parentIdFromQuery = urlParams.get("parentId");
            return {
                nameEn: "",
                nameAr: "",
                category: categoryId ? { id: parseInt(categoryId) } : null,
                CreateAt: "",
                recordOrder: 0,
                url: "",
                parentId: parentIdFromQuery ? parseInt(parentIdFromQuery) : null,
                isActive: false,
                isVisible: false,
                image1: "",
                image2: "",
                image3: "",
            };
        }
        return null; // سيتم ملؤه عند جلب البيانات في حالة التعديل
    });

    const [categories, setCategories] = useState([]);
    const [categoriesLoading, setCategoriesLoading] = useState(true);
    const [parentBlocks, setParentBlocks] = useState([]);
    const [parentBlocksLoading, setParentBlocksLoading] = useState(true);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");

    // 🔹 fetch categories
    useEffect(() => {
        const fetchCategories = async () => {
            setCategoriesLoading(true);
            try {
                const response = await fetch(`${API_BASE_URL}api/BlockCategories/all`, {
                    headers: { Authorization: `Bearer ${token}` },
                });
                if (!response.ok) throw new Error(`HTTP error: ${response.status}`);
                const data = await response.json();
                setCategories(data || []);
            } catch {
                setError(t("failedToLoadCategories"));
            } finally {
                setCategoriesLoading(false);
            }
        };
        fetchCategories();
    }, [token, t]);

    // 🔹 fetch parent blocks
    useEffect(() => {
        const fetchParentBlocks = async () => {
            setParentBlocksLoading(true);
            try {
                const response = await fetch(`${API_BASE_URL}api/Blocks/NoCategory`, {
                    headers: { Authorization: `Bearer ${token}` },
                });
                if (!response.ok) throw new Error(`HTTP error: ${response.status}`);
                const data = await response.json();
                setParentBlocks(data.items || []);
            } catch {
                setError(t("failedToLoadParentBlock"));
            } finally {
                setParentBlocksLoading(false);
            }
        };
        fetchParentBlocks();
    }, [token, t]);

    // 🔹 fetch block data if editing
    useEffect(() => {
        if (!isEditMode) return;

        const fetchBlock = async () => {
            try {
                const response = await fetch(`${API_BASE_URL}api/Blocks/${blockId}`, {
                    headers: { Authorization: `Bearer ${token}` },
                });
                if (!response.ok) throw new Error(`HTTP error: ${response.status}`);
                const data = await response.json();

                setFormData({
                    nameEn: data.nameEn || "",
                    nameAr: data.nameAr || "",
                    category: data.categoryId ? { id: data.categoryId } : (categoryId ? { id: parseInt(categoryId) } : null),
                    CreateAt: data.createAt ? data.createAt.split("T")[0] : "",
                    recordOrder: data.recordOrder || 0,
                    url: data.url || "",
                    parentId: data.parentId || null,
                    isActive: data.isActive || false,
                    isVisible: data.isVisible || false,
                    image1: data.image1 || "",
                    image2: data.image2 || "",
                    image3: data.image3 || "",
                });

                setDescriptionEn(data.descriptionEn || "");
                setDescriptionAr(data.descriptionAr || "");
                setDescriptionEn1(data.descriptionEn1 || "");
                setDescriptionAr1(data.descriptionAr1 || "");
                setDescriptionEn2(data.descriptionEn2 || "");
                setDescriptionAr2(data.descriptionAr2 || "");
                setDescriptionEn3(data.descriptionEn3 || "");
                setDescriptionAr3(data.descriptionAr3 || "");
            } catch (err) {
                console.error("Failed to load block:", err);
                setError(t("failedToLoadBlock"));
            }
        };

        fetchBlock();
    }, [blockId, categoryId, token, t, isEditMode]);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setFormData(prev => ({ ...prev, [name]: value }));
    };

    const handleChangeValue = (name, value) => {
        setFormData(prev => ({ ...prev, [name]: value }));
    };

    const handleSubmit = async (e) => {
        e.preventDefault();
        if (!formData) return;
        setLoading(true);
        setError("");

        try {
            const payload = {
                ...formData,
                categoryId: formData.category?.id || 0,
                id: isEditMode ? blockId : 0,
                descriptionEn,
                descriptionAr,
                descriptionEn1,
                descriptionAr1,
                descriptionEn2,
                descriptionAr2,
                descriptionEn3,
                descriptionAr3,
            };

            const method = isEditMode ? "PUT" : "POST";
            const url = isEditMode
                ? `${API_BASE_URL}api/Blocks/${blockId}`
                : `${API_BASE_URL}api/Blocks`;

            const response = await fetch(url, {
                method,
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${token}`,
                },
                body: JSON.stringify(payload),
            });

            if (!response.ok) throw new Error(await response.text());
            navigate(`/blocks/${formData.category?.id || ""}`);
        } catch (err) {
            setError("حدث خطأ أثناء الحفظ ❌");
        } finally {
            setLoading(false);
        }
    };

    const editorInit = {
        height: 300,
        menubar: true,
        plugins: [
            "advlist autolink lists link image charmap print preview anchor",
            "searchreplace visualblocks code fullscreen",
            "insertdatetime media table paste code help wordcount",
        ],
        toolbar:
            "undo redo | formatselect | bold italic underline strikethrough | \
      alignleft aligncenter alignright alignjustify | \
      bullist numlist outdent indent | link image media | code | removeformat | help",
        license_key: "gpl",
    };

    if (!formData || categoriesLoading || parentBlocksLoading) {
        return <Typography>جارٍ التحميل...</Typography>;
    }

    return (
        <Box sx={{ p: 3, mx: "auto", maxWidth: 1200 }}>
            <Typography variant="h5" sx={{ mb: 2, fontWeight: "bold" }}>
                {isEditMode ? t("editBlock") || "تعديل كتلة" : t("addBlock") || "إضافة كتلة جديدة"}
            </Typography>

            <form onSubmit={handleSubmit}>
                <Stack spacing={2}>
                    {/* الاسم بالإنجليزية والعربية */}
                    <Stack direction="row" sx={{ gap: 3 }}>
                        <TextField
                            label={t("nameEn") || "Name (EN)"}
                            name="nameEn"
                            value={formData.nameEn}
                            onChange={handleChange}
                            required
                            fullWidth
                        />
                        <TextField
                            label={t("nameAr") || "Name (AR)"}
                            name="nameAr"
                            value={formData.nameAr}
                            onChange={handleChange}
                            required
                            fullWidth
                        />
                    </Stack>

                    {/* المحررات */}
                    {[setDescriptionEn, setDescriptionAr, setDescriptionEn1, setDescriptionAr1,
                        setDescriptionEn2, setDescriptionAr2, setDescriptionEn3, setDescriptionAr3].map((setFn, i) => (
                            <Editor
                                key={i}
                                tinymceScriptSrc="/tinymce/tinymce.min.js"
                                value={[descriptionEn, descriptionAr, descriptionEn1, descriptionAr1,
                                    descriptionEn2, descriptionAr2, descriptionEn3, descriptionAr3][i]}
                                init={editorInit}
                                onEditorChange={setFn}
                            />
                        ))}

                    {/* التاريخ والترتيب */}
                    <Stack direction="row" spacing={0} sx={{ gap: 3 }}>
                        <TextField
                            label={t("createAt") || "CreateAt"}
                            name="CreateAt"
                            value={formData.CreateAt}
                            onChange={handleChange}
                            type="date"
                            fullWidth
                            InputLabelProps={{ shrink: true }}
                        />
                        <TextField
                            label={t("recordOrder") || "Order"}
                            name="recordOrder"
                            value={formData.recordOrder}
                            onChange={handleChange}
                            type="number"
                            fullWidth
                        />
                    </Stack>

                    {/* الفئة والرابط */}
                    <Stack direction="row" spacing={0} sx={{ gap: 3 }}>
                        <Autocomplete
                            fullWidth
                            options={categories}
                            getOptionLabel={(o) => `${o.nameAr} (${o.nameEn})`}
                            value={
                                formData.category
                                    ? categories.find(c => c.id === formData.category.id) || null
                                    : null
                            }
                            onChange={(e, val) => handleChangeValue("category", val)}
                            renderInput={(params) => <TextField {...params} label={t("selectCategory")} />}
                        />
                        <TextField
                            label={t("url") || "Url"}
                            name="url"
                            value={formData.url}
                            onChange={handleChange}
                            fullWidth
                        />
                    </Stack>

                    {/* البلوك الأب */}
                    <Autocomplete
                        fullWidth
                        options={parentBlocks}
                        getOptionLabel={(o) => `${o.nameAr} (${o.nameEn})`}
                        value={formData.parentId ? parentBlocks.find(p => p.id === formData.parentId) || null : null}
                        onChange={(e, val) => handleChangeValue("parentId", val ? val.id : null)}
                        renderInput={(params) => <TextField {...params} label={t("selectParentBlock")} />}
                    />

                    {/* Switches */}
                    <Stack direction="row" spacing={0} sx={{ gap: 3 }}>
                        <FormControlLabel
                            control={<Switch checked={formData.isActive} onChange={(e) => handleChangeValue("isActive", e.target.checked)} />}
                            label={t("isActive")}
                        />
                        <FormControlLabel
                            control={<Switch checked={formData.isVisible} onChange={(e) => handleChangeValue("isVisible", e.target.checked)} />}
                            label={t("isVisible")}
                        />
                    </Stack>

                    {/* رفع الصور */}
                    <Stack direction="row" spacing={0} sx={{ gap: 3 }}>
                        <ImageUploader label="Image 1" value={formData.image1} onChange={(val) => setFormData(prev => ({ ...prev, image1: val }))} apiBaseUrl={API_BASE_URL} token={token} t={t} />
                        <ImageUploader label="Image 2" value={formData.image2} onChange={(val) => setFormData(prev => ({ ...prev, image2: val }))} apiBaseUrl={API_BASE_URL} token={token} t={t} />
                        <ImageUploader label="Image 3" value={formData.image3} onChange={(val) => setFormData(prev => ({ ...prev, image3: val }))} apiBaseUrl={API_BASE_URL} token={token} t={t} />
                    </Stack>

                    {error && <Typography color="error">{error}</Typography>}

                    {/* الأزرار */}
                    <Stack direction="row" spacing={2}>
                        <Button type="submit" variant="contained" color="primary" disabled={loading}>
                            {loading ? t("processing") : isEditMode ? t("update") || "تحديث" : t("save") || "حفظ"}
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
