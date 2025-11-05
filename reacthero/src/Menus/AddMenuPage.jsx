import React, { useState, useEffect } from "react";
import {
    Box,
    Typography,
    TextField,
    Button,
    Stack,
    Autocomplete,
    FormControlLabel,
    Switch, Paper, 
} from "@mui/material";
import { useNavigate, useParams } from "react-router-dom";
import { API_BASE_URL } from "../../config";
import { Editor } from "@tinymce/tinymce-react";
import { useTranslation } from "react-i18next";
import ImageUploader from "../Shared/ImageUploader";

const AddMenuPage = () => {
    const { categoryId, MenuId } = useParams();
    const navigate = useNavigate();
    const token = localStorage.getItem("token");
    const { t } = useTranslation();

    const isEditMode = !!MenuId;


    const MenuTypeEnum = {
        Blog: "Pages",
        Link: "Drop Down Menu",
        PhotoGallery: "Internal Link",
        VideoGallery: "External Link",
        HomeSlider: "Downloaded File",
    };
    const menuTypeOptions = Object.values(MenuTypeEnum);

    // TinyMCE states
    const [descriptionEn, setDescriptionEn] = useState("");
    const [descriptionAr, setDescriptionAr] = useState("");


    const [formData, setFormData] = useState(() => {
        if (!isEditMode) {
            const urlParams = new URLSearchParams(window.location.search);
            const parentIdFromQuery = urlParams.get("parentId");
            return {
                nameEn: "",
                nameAr: "",
                category: categoryId ? { id: parseInt(categoryId) } : null,
                levelOrder: 0,
                url: "",
                type: "",
                parentId: parentIdFromQuery ? parseInt(parentIdFromQuery) : null,
                isActive: false,
                isHome: false,
                isFooter: false,
                isHomeFooter: false,
                image: "",
                file: "",
            };
        }
        return null; // سيتم ملؤه عند جلب البيانات في حالة التعديل
    });

    const [categories, setCategories] = useState([]);
    const [categoriesLoading, setCategoriesLoading] = useState(true);
    const [parentMenus, setParentMenus] = useState([]);
    const [parentMenusLoading, setParentMenusLoading] = useState(true);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");

    // 🔹 fetch categories
    useEffect(() => {
        const fetchCategories = async () => {
            setCategoriesLoading(true);
            try {
                const response = await fetch(`${API_BASE_URL}api/v1/MenuCategories/all`, {
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

    // 🔹 fetch parent Menus
    useEffect(() => {
        const fetchParentMenus = async () => {
            setParentMenusLoading(true);
            try {
                const response = await fetch(`${API_BASE_URL}api/v1/Menus/NoCategory`, {
                    headers: { Authorization: `Bearer ${token}` },
                });
                if (!response.ok) throw new Error(`HTTP error: ${response.status}`);
                const data = await response.json();
                setParentMenus(data.items || []);
            } catch {
                setError(t("failedToLoadParentMenu"));
            } finally {
                setParentMenusLoading(false);
            }
        };
        fetchParentMenus();
    }, [token, t]);

    // 🔹 fetch Menu data if editing
    useEffect(() => {
        if (!isEditMode) return;

        const fetchMenu = async () => {
            try {
                const response = await fetch(`${API_BASE_URL}api/v1/Menus/${MenuId}`, {
                    headers: { Authorization: `Bearer ${token}` },
                });
                if (!response.ok) throw new Error(`HTTP error: ${response.status}`);
                const data = await response.json();

                setFormData({
                    nameEn: data.nameEn || "",
                    nameAr: data.nameAr || "",
                    category: data.categoryId ? { id: data.categoryId } : (categoryId ? { id: parseInt(categoryId) } : null),
                    CreateAt: data.createAt ? data.createAt.split("T")[0] : "",
                    levelOrder: data.levelOrder || 0,
                    url: data.url || "",
                    parentId: data.parentId || null,
                    isActive: data.isActive || false,
                    isHome: data.isHome || false,
                    isFooter: data.isFooter || false,
                    isHomeFooter: data.isHomeFooter || false,
                    type: data.type || false,
                    image: data.image || "",
                    file: data.file || "",
                });

                setDescriptionEn(data.descriptionEn || "");
                setDescriptionAr(data.descriptionAr || "");
      
            } catch (err) {
                console.error("Failed to load Menu:", err);
                setError(t("failedToLoadMenu"));
            }
        };

        fetchMenu();
    }, [MenuId, categoryId, token, t, isEditMode]);

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
                id: isEditMode ? MenuId : 0,
                descriptionEn,
                descriptionAr,
            };

            const method = isEditMode ? "PUT" : "POST";
            const url = isEditMode
                ? `${API_BASE_URL}api/v1/Menus/${MenuId}`
                : `${API_BASE_URL}api/v1/Menus`;

            const response = await fetch(url, {
                method,
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${token}`,
                },
                body: JSON.stringify(payload),
            });

            if (!response.ok) throw new Error(await response.text());
            navigate(-1);

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
            "searchreplace visualMenus code fullscreen",
            "insertdatetime media table paste code help wordcount",
        ],
        toolbar:
            "undo redo | formatselect | bold italic underline strikethrough | \
      alignleft aligncenter alignright alignjustify | \
      bullist numlist outdent indent | link image media | code | removeformat | help",
        license_key: "gpl",
    };

    if (!formData || categoriesLoading || parentMenusLoading) {
        return <Typography>جارٍ التحميل...</Typography>;
    }

    return (
        <Box sx={{ p: 3, mx: "auto", maxWidth: 1200 }}>
            <Typography variant="h5" sx={{ mb: 2, fontWeight: "bold" }}>
                {isEditMode ? t("editMenu") || "تعديل كتلة" : t("addMenu") || "إضافة كتلة جديدة"}
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
                    {[setDescriptionEn, setDescriptionAr].map((setFn, i) => (
                            <Editor
                                key={i}
                                tinymceScriptSrc="/tinymce/tinymce.min.js"
                                value={[descriptionEn, descriptionAr][i]}
                                init={editorInit}
                                onEditorChange={setFn}
                            />
                        ))}



                    <Paper
                        elevation={3}
                        sx={{
                            p: 2,
                            borderRadius: 2,
                            border: "1px solid #e0e0e0",
                            backgroundColor: "#fafafa",
                        }}
                    >
                        <Typography variant="h6" gutterBottom sx={{ fontWeight: "bold", color: "#1976d2" }}>
                            {t("Display Options")}
                        </Typography>

                        <Stack direction="row" spacing={3}>
                            <FormControlLabel
                                control={
                                    <Switch
                                        checked={formData.isHome}
                                        onChange={(e) => handleChangeValue("isHome", e.target.checked)}
                                    />
                                }
                                label={t("IsHome")}
                            />
                            <FormControlLabel
                                control={
                                    <Switch
                                        checked={formData.isFooter}
                                        onChange={(e) => handleChangeValue("isFooter", e.target.checked)}
                                    />
                                }
                                label={t("IsFooter")}
                            />
                            <FormControlLabel
                                control={
                                    <Switch
                                        checked={formData.isHomeFooter}
                                        onChange={(e) => handleChangeValue("isHomeFooter", e.target.checked)}
                                    />
                                }
                                label={t("IsHomeFooter")}
                            />
                        </Stack>
                    </Paper>


                    {/* التاريخ والترتيب */}
                    <Stack direction="row" spacing={0} sx={{ gap: 3 }}>
                        <Autocomplete
                            fullWidth
                            options={menuTypeOptions}
                            getOptionLabel={(option) => option}
                            value={formData.type || null}
                            onChange={(event, newValue) =>
                                handleChangeValue("type", newValue || "")
                            }
                            renderInput={(params) => (
                                <TextField {...params} label={t("selectType")} required />
                            )}
                        />
                        <TextField
                            label={t("levelOrder") || "Order"}
                            name="levelOrder"
                            value={formData.levelOrder}
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
                        options={parentMenus}
                        getOptionLabel={(o) => `${o.nameAr} (${o.nameEn})`}
                        value={formData.parentId ? parentMenus.find(p => p.id === formData.parentId) || null : null}
                        onChange={(e, val) => handleChangeValue("parentId", val ? val.id : null)}
                        renderInput={(params) => <TextField {...params} label={t("selectParentMenu")} />}
                    />

                    {/* Switches */}
                    <Stack direction="row" spacing={0} sx={{ gap: 3 }}>
                        <FormControlLabel
                            control={<Switch checked={formData.isActive} onChange={(e) => handleChangeValue("isActive", e.target.checked)} />}
                            label={t("isActive")}
                        />
              
                    </Stack>

                    {/* رفع الصور */}
                    <Stack direction="row" spacing={0} sx={{ gap: 3 }}>
                        <ImageUploader label="Image 1" value={formData.image} onChange={(val) => setFormData(prev => ({ ...prev, image: val }))} apiBaseUrl={API_BASE_URL} token={token} t={t} />
                        <ImageUploader label="File 1" value={formData.file} onChange={(val) => setFormData(prev => ({ ...prev, file: val }))} apiBaseUrl={API_BASE_URL} token={token} t={t} />
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

export default AddMenuPage;
