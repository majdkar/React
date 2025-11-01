import React, { useEffect, useState } from "react";
import {
    Box,
    Typography,
    Button,
    Stack,
    Snackbar,
    Alert,
    CircularProgress,
    Grid,
} from "@mui/material";
import { API_BASE_URL } from "../../config";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import AddPhotoAlternateIcon from "@mui/icons-material/AddPhotoAlternate";
import { useParams, useNavigate } from "react-router-dom";

const BlockPhotos = () => {
    const { blockId } = useParams();
    const navigate = useNavigate();
    const token = localStorage.getItem("token");

    const [photos, setPhotos] = useState([]);
    const [loading, setLoading] = useState(false);
    const [uploading, setUploading] = useState(false);
    const [file, setFile] = useState(null);
    const [snackbar, setSnackbar] = useState({
        open: false,
        message: "",
        type: "success",
    });

    // 🔹 جلب الصور الحالية
    const fetchPhotos = async () => {
        try {
            setLoading(true);
            const res = await fetch(`${API_BASE_URL}api/BlockPhoto/GetPhotoByBlockId/?id=${blockId}`);
            const data = await res.json();
            console.log("📸 Block Photos API data:", data);
            setPhotos(data || []);
        } catch (err) {
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    // 🔹 رفع الصورة للسيرفر ثم حفظ الرابط
    const handleUploadPhoto = async () => {
        if (!file) return alert("الرجاء اختيار صورة أولاً");

        try {
            setUploading(true);

            // 1️⃣ رفع الصورة للسيرفر
            const formData = new FormData();
            formData.append("file", file);

            const uploadRes = await fetch(
                `${API_BASE_URL}api/FileUpload/1/1`,
                {
                    method: "POST",
                    body: formData,
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                }
            );

            if (!uploadRes.ok) throw new Error(await uploadRes.text());

            const uploadData = await uploadRes.text();
            const imageUrl = "Files\\UploadFiles\\BlocksFiles\\" + ( uploadData.url || uploadData); // حسب شكل الاستجابة

            // 2️⃣ حفظ الرابط في BlockPhoto
            const saveRes = await fetch(`${API_BASE_URL}api/BlockPhoto`, {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    Authorization: `Bearer ${token}`,
                },
                body: JSON.stringify({
                    image: imageUrl,
                    blockId: Number(blockId),
                }),
            });

            if (!saveRes.ok) throw new Error(await saveRes.text());

            setFile(null);
            fetchPhotos();
            setSnackbar({
                open: true,
                message: "تم رفع الصورة وإضافتها للألبوم ✅",
                type: "success",
            });
        } catch (err) {
            console.error(err);
            setSnackbar({
                open: true,
                message: "فشل رفع الصورة ❌",
                type: "error",
            });
        } finally {
            setUploading(false);
        }
    };

    // 🔹 حذف صورة
    const handleDelete = async (photoId) => {
        if (!window.confirm("هل أنت متأكد من حذف الصورة؟")) return;
        try {
            setLoading(true);
            const res = await fetch(`${API_BASE_URL}api/BlockPhoto/${photoId}`, {
                method: "DELETE",
                headers: { Authorization: `Bearer ${token}` },
            });
            if (!res.ok) throw new Error(await res.text());
            fetchPhotos();
            setSnackbar({ open: true, message: "تم حذف الصورة ✅", type: "success" });
        } catch (err) {
            console.error(err);
            setSnackbar({ open: true, message: "فشل في حذف الصورة ❌", type: "error" });
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchPhotos();
    }, [blockId]);

    return (
        <Box sx={{ p: 3 }}>
            {/* ✅ رأس الصفحة */}
            <Stack direction="row" alignItems="center" justifyContent="space-between" mb={3}>
                <Typography variant="h5">📸 ألبوم صور البلوك رقم {blockId}</Typography>
                <Button variant="outlined" startIcon={<ArrowBackIcon />} onClick={() => navigate(-1)}>
                    رجوع
                </Button>
            </Stack>

            {/* ✅ رفع صورة جديدة */}
            <Stack direction={{ xs: "column", sm: "row" }} spacing={2} alignItems="center" mb={3}>
                <input
                    type="file"
                    accept="image/*"
                    onChange={(e) => setFile(e.target.files[0])}
                />
                <Button
                    variant="contained"
                    color="primary"
                    startIcon={<AddPhotoAlternateIcon />}
                    onClick={handleUploadPhoto}
                    disabled={uploading || !file}
                >
                    {uploading ? "جارٍ الرفع..." : "رفع الصورة"}
                </Button>
                {uploading && <CircularProgress size={24} />}
            </Stack>

            {/* ✅ عرض الصور */}
            <Grid container spacing={2}>
                {photos.map((p) => (
                    <Grid item xs={6} sm={4} md={3} key={p.id}>
                        <Box
                            sx={{
                                position: "relative",
                                borderRadius: 2,
                                overflow: "hidden",
                                border: "1px solid #ccc",
                            }}
                        >
                            <img
                                src={
                                    p.image.startsWith("http")
                                        ? p.image
                                        : `${API_BASE_URL}${p.image.replace(/\\/g, "/")}`
                                }
                                alt=""
                                style={{ width: "100%", height: 180, objectFit: "cover" }}
                            />
                            <Button
                                size="small"
                                color="error"
                                variant="contained"
                                sx={{
                                    position: "absolute",
                                    top: 8,
                                    right: 8,
                                    minWidth: 0,
                                    borderRadius: "50%",
                                }}
                                onClick={() => handleDelete(p.id)}
                            >
                                ✖
                            </Button>
                        </Box>
                    </Grid>
                ))}
            </Grid>

            {/* ✅ إشعار */}
            <Snackbar
                open={snackbar.open}
                autoHideDuration={3000}
                onClose={() => setSnackbar({ ...snackbar, open: false })}
                anchorOrigin={{ vertical: "top", horizontal: "center" }}
            >
                <Alert severity={snackbar.type} sx={{ width: "100%" }}>
                    {snackbar.message}
                </Alert>
            </Snackbar>
        </Box>
    );
};

export default BlockPhotos;
