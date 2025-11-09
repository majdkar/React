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

const AddPage = () => {

    const { blockId } = useParams();
    console.log(blockId);
    const navigate = useNavigate();
    const token = localStorage.getItem("token");
    const { t } = useTranslation();


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
        if (blockId == 0) {
            const urlParams = new URLSearchParams(window.location.search);
            return {
                nameEn: "",
                nameAr: "",
                recordOrder: 0,
                url: "",
                menuId: null,
                type: "",
                isActive: false,
                image: "",
                image1: "",
                image2: "",
                image3: "",
            };
        }
        return null; // سيتم ملؤه عند جلب البيانات في حالة التعديل
    });
    const [menuAll, setMenuAll] = useState([]);

    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");


    const PageTypeEnum = {
        Blog: "Basic Pages",
        Link: "Contact Us",
    };
    const pageTypeOptions = Object.values(PageTypeEnum);

    // 🔹 fetch block data if editing
    useEffect(() => {
        if (blockId == 0) return;

        const fetchBlock = async () => {
            try {
                const response = await fetch(`${API_BASE_URL}api/Pages/${blockId}`, {
                    headers: { Authorization: `Bearer ${token}` },
                });
                if (!response.ok) throw new Error(`HTTP error: ${response.status}`);
                const data = await response.json();

                setFormData({
                    nameEn: data.nameEn || "",
                    nameAr: data.nameAr || "",
                    recordOrder: data.recordOrder || 0,
                    url: data.url || "",
                    menuId: data.menuId || null,
                    type: data.type || "",
                    isActive: data.isActive || false,
                    image: data.image || "",
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
    }, [blockId, token, t]);


    useEffect(() => {
        if (!token) return; // لا تشغل عند غياب التوكن
        const fetchMenuAll = async () => {
            try {
                const res = await fetch(`${API_BASE_URL}api/v1/Menus/NoCategory`, {
                    headers: { Authorization: `Bearer ${token}` }
                });
                if (!res.ok) throw new Error("Failed to fetch menus");
                const data = await res.json();
                setMenuAll(data.items);
            } catch (err) {
                console.error(err);
            }
        };
        fetchMenuAll();
    }, [token]);

    const handleChange = (field, value) => {
        setFormData((prev) => ({ ...prev, [field]: value }));
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
                id: blockId > 0 ? blockId : 0,
                descriptionEn,
                descriptionAr,
                descriptionEn1,
                descriptionAr1,
                descriptionEn2,
                descriptionAr2,
                descriptionEn3,
                descriptionAr3,
            };

            const method = blockId > 0 ? "PUT" : "POST";
            const url = blockId  > 0 
                ? `${API_BASE_URL}api/Pages/${blockId}`
                : `${API_BASE_URL}api/Pages`;

            const response = await fetch(url, {
                method,
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${token}`,
                },
                body: JSON.stringify(payload),
            });

            if (!response.ok) throw new Error(await response.text());
            navigate(`/Pages/`);
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

    if (!formData) {
        return <Typography>جارٍ التحميل...</Typography>;
    }

    return (
        <Box sx={{ p: 3, mx: "auto", maxWidth: 1200 }}>
            <Typography variant="h5" sx={{ mb: 2, fontWeight: "bold" }}>
                {blockId > 0 ? t("editPage") || "تعديل صفحة" : t("addPage") || "إضافة صفحة "}
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
                            label={t("recordOrder") || "Order"}
                            name="recordOrder"
                            value={formData.recordOrder}
                            onChange={handleChange}
                            type="number"
                            fullWidth
                        />

                        <TextField
                            label={t("url") || "Url"}
                            name="url"
                            value={formData.url}
                            onChange={handleChange}
                            fullWidth
                        />
                    </Stack>

                    {/* الفئة والرابط */}
                    <Stack direction="row" spacing={0} sx={{ gap: 3 }}>
                        <Autocomplete
                            fullWidth
                            options={pageTypeOptions}
                            getOptionLabel={(option) => option}
                            value={formData.type || null}
                            onChange={(event, newValue) => handleChange("type", newValue || "")
                            }
                            renderInput={(params) => (
                                <TextField {...params} label={t("selectBlockType")} required />
                            )}
                        />



                        <Autocomplete
                            fullWidth
                            options={menuAll}
                            getOptionLabel={(o) => `${o.nameAr} (${o.nameEn})`}
                            value={
                                formData.menuId
                                    ? menuAll.find(c => c.id === formData.menuId) || null
                                    : null
                            }
                            onChange={(e, val) => handleChangeValue("menuId", val ? val.id : null)}

                            renderInput={(params) => <TextField {...params} label={t("selectMenu")} />}
                        />

                    </Stack>

                    {/* البلوك الأب */}
        

                    {/* Switches */}
                    <Stack direction="row" spacing={0} sx={{ gap: 3 }}>
                        <FormControlLabel
                            control={<Switch checked={formData.isActive} onChange={(e) => handleChangeValue("isActive", e.target.checked)} />}
                            label={t("isActive")}
                        />
               
                    </Stack>

                    {/* رفع الصور */}
                    <Stack direction="row" spacing={0} sx={{ gap: 3 }}>
                        <ImageUploader label={t("image")} value={formData.image} onChange={(val) => setFormData(prev => ({ ...prev, image: val }))} apiBaseUrl={API_BASE_URL} token={token} t={t} />
                        <ImageUploader label={t("image2")} value={formData.image1} onChange={(val) => setFormData(prev => ({ ...prev, image1: val }))} apiBaseUrl={API_BASE_URL} token={token} t={t} />
                        <ImageUploader label={t("image3")} value={formData.image2} onChange={(val) => setFormData(prev => ({ ...prev, image2: val }))} apiBaseUrl={API_BASE_URL} token={token} t={t} />
                        <ImageUploader label={t("image4")} value={formData.image3} onChange={(val) => setFormData(prev => ({ ...prev, image3: val }))} apiBaseUrl={API_BASE_URL} token={token} t={t} />
                    </Stack>

                    {error && <Typography color="error">{error}</Typography>}

                    {/* الأزرار */}
                    <Stack direction="row" spacing={2}>
                        <Button type="submit" variant="contained" color="primary" disabled={loading}>
                            {loading ? t("processing") : blockId > 0 ? t("update") : t("save") }
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

export default AddPage;
