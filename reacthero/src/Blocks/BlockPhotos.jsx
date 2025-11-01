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
import ConfirmDialog from "../Shared/ConfirmDialog"; // ✅ استدعاء مكوّن التأكيد

const BlockPhotos = () => {
    const { blockId } = useParams();
    const navigate = useNavigate();
    const token = localStorage.getItem("token");

    const [photos, setPhotos] = useState([]);
    const [loading, setLoading] = useState(false);
    const [uploading, setUploading] = useState(false);
    const [snackbar, setSnackbar] = useState({
        open: false,
        message: "",
        type: "success",
    });

    // 🔹 للحذف
    const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
    const [photoToDelete, setPhotoToDelete] = useState(null);

    // 🔹 جلب الصور الحالية
    const fetchPhotos = async () => {
        try {
            setLoading(true);
            const res = await fetch(`${API_BASE_URL}api/BlockPhoto/GetPhotoByBlockId/?id=${blockId}`);
            const data = await res.json();
            console.log("📸 Block Photos API data:", data);
            setPhotos(data.items || data || []);
        } catch (err) {
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    // 🔹 رفع الصورة للسيرفر ثم حفظ الرابط
    const handleFileChange = async (e) => {
        const file = e.target.files[0];
        if (!file) return;
        try {
            setUploading(true);

            // 1️⃣ رفع الصورة إلى السيرفر
            const formData = new FormData();
            formData.append("file", file);

            const uploadRes = await fetch(`${API_BASE_URL}api/FileUpload/1/1`, {
                method: "POST",
                body: formData,
                headers: { Authorization: `Bearer ${token}` },
            });

            if (!uploadRes.ok) throw new Error(await uploadRes.text());
            const uploadData = await uploadRes.text();
            const fileName = uploadData.replace(/"/g, "");
            const imageUrl = `Files/UploadFiles/BlocksFiles/${fileName}`;

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
            fetchPhotos();
            setSnackbar({
                open: true,
                message: "تم رفع الصورة بنجاح ✅",
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
            e.target.value = ""; // لإعادة تفعيل input مرة أخرى
        }
    };

    // 🔹 حذف صورة (يتم بعد التأكيد)
    const handleDeleteConfirm = async () => {
        if (!photoToDelete) return;
        try {
            setLoading(true);
            const res = await fetch(`${API_BASE_URL}api/BlockPhoto/${photoToDelete}`, {
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
            setDeleteDialogOpen(false);
            setPhotoToDelete(null);
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

            {/* ✅ زر رفع الصورة */}
            <Box mb={3}>
                <Button
                    variant="contained"
                    component="label"
                    startIcon={<AddPhotoAlternateIcon />}
                    disabled={uploading}
                >
                    {uploading ? (
                        <>
                            <CircularProgress size={20} sx={{ mr: 1 }} />
                            جاري الرفع...
                        </>
                    ) : (
                        "رفع صورة جديدة"
                    )}
                    <input type="file" hidden accept="image/*" onChange={handleFileChange} />
                </Button>
            </Box>

            {/* ✅ عرض الصور */}
            {loading ? (
                <Typography>جارٍ تحميل الصور...</Typography>
            ) : photos.length === 0 ? (
                <Typography color="text.secondary">لا توجد صور مرفوعة لهذا البلوك.</Typography>
            ) : (
                <Grid container spacing={2}>
                    {photos.map((p) => {
                        const imagePath = p.image?.replace(/\\/g, "/");
                        const fullUrl = imagePath?.startsWith("http")
                            ? imagePath
                            : `${API_BASE_URL}${imagePath}`;
                        return (
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
                                        src={fullUrl}
                                        alt=""
                                        style={{ width: "100%", height: 180, objectFit: "cover" }}
                                        onError={(e) => {
                                            e.target.src =
                                                "https://via.placeholder.com/180x180?text=No+Image";
                                        }}
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
                                        onClick={() => {
                                            setPhotoToDelete(p.id);
                                            setDeleteDialogOpen(true);
                                        }}
                                    >
                                        ✖
                                    </Button>
                                </Box>
                            </Grid>
                        );
                    })}
                </Grid>
            )}

            {/* ✅ تأكيد الحذف */}
            <ConfirmDialog
                open={deleteDialogOpen}
                onClose={() => setDeleteDialogOpen(false)}
                onConfirm={handleDeleteConfirm}
                title="تأكيد الحذف"
                message="هل أنت متأكد أنك تريد حذف هذه الصورة؟"
                confirmText="حذف"
                cancelText="إلغاء"
                loading={loading}
            />

            {/* ✅ Snackbar */}
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
