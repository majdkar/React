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
    LinearProgress,
    Checkbox,
} from "@mui/material";
import { API_BASE_URL } from "../../config";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import AddPhotoAlternateIcon from "@mui/icons-material/AddPhotoAlternate";
import DeleteIcon from "@mui/icons-material/Delete";
import { useParams, useNavigate } from "react-router-dom";
import ConfirmDialog from "../Shared/ConfirmDialog";
import axios from "axios";
import { useTranslation } from "react-i18next";

const PagePhotos = () => {
    const { pageId, pageName } = useParams();

    const { t } = useTranslation();

    const navigate = useNavigate();
    const token = localStorage.getItem("token");
    const [photos, setPhotos] = useState([]);
    const [loading, setLoading] = useState(false);
    const [uploadingFiles, setUploadingFiles] = useState([]);
    const [snackbar, setSnackbar] = useState({ open: false, message: "", type: "success" });

    const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
    const [selectedPhotos, setSelectedPhotos] = useState([]); // ✅ لتحديد الصور للحذف

    // 🔹 جلب الصور الحالية
    const fetchPhotos = async () => {
        try {
            setLoading(true);
            const res = await fetch(`${API_BASE_URL}api/PagePhoto/GetPhotoByPageId/?id=${pageId}`);
            const data = await res.json();
            setPhotos(data.items || data || []);
        } catch (err) {
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    // 🔹 رفع عدة صور مع Progress
    const handleFileChange = async (e) => {
        const files = Array.from(e.target.files);
        if (!files.length) return;

        const uploads = files.map(f => ({ name: f.name, progress: 0 }));
        setUploadingFiles(uploads);

        for (let i = 0; i < files.length; i++) {
            const file = files[i];
            try {
                const formData = new FormData();
                formData.append("file", file);

                const uploadRes = await axios.post(
                    `${API_BASE_URL}api/FileUpload/3/1`,
                    formData,
                    {
                        headers: { Authorization: `Bearer ${token}` },
                        onUploadProgress: (progressEvent) => {
                            const percent = Math.round((progressEvent.loaded * 100) / progressEvent.total);
                            setUploadingFiles(prev => {
                                const copy = [...prev];
                                copy[i].progress = percent;
                                return copy;
                            });
                        },
                    }
                );

                const fileName = uploadRes.data.replace(/"/g, "");
                const imageUrl = `Files/UploadFiles/PagesFiles/${fileName}`;

                await fetch(`${API_BASE_URL}api/PagePhoto`, {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                        Authorization: `Bearer ${token}`,
                    },
                    body: JSON.stringify({ image: imageUrl, pageId: Number(pageId) }),
                });

            } catch (err) {
                console.error(t("Upload failed") + "❌", file.name, err);
            }
        }

        setUploadingFiles([]);
        fetchPhotos();
        setSnackbar({
            open: true, message: t("has been uploaded") + `${files.length}` +  t("image successfully"), type: "success" });
        e.target.value = "";
    };

    // 🔹 حذف الصور المحددة
    const handleDeleteConfirm = async () => {
        if (!selectedPhotos.length) return;

        try {
            setLoading(true);
            for (const photoId of selectedPhotos) {
                const res = await fetch(`${API_BASE_URL}api/PagePhoto/${photoId}`, {
                    method: "DELETE",
                    headers: { Authorization: `Bearer ${token}` },
                });
                if (!res.ok) console.error(await res.text());
            }
            fetchPhotos();
            setSnackbar({ open: true, message: t("The selected photos have been deleted") + "✅", type: "success" });
        } catch (err) {
            console.error(err);
            setSnackbar({ open: true, message: t("Failed to delete photos") + "❌", type: "error" });
        } finally {
            setLoading(false);
            setDeleteDialogOpen(false);
            setSelectedPhotos([]);
        }
    };

    useEffect(() => {
        fetchPhotos();
    }, [pageId]);

    // 🔹 toggle selection
    const toggleSelect = (id) => {
        setSelectedPhotos(prev => prev.includes(id) ? prev.filter(x => x !== id) : [...prev, id]);
    };

    return (
        <Box sx={{ p: 3 }}>
            <Stack direction="row" alignItems="center" justifyContent="space-between" mb={3}>
                <Typography variant="h5">📸 {t("Page photo album")}  {pageName}</Typography>
                <Button variant="outlined" startIcon={<ArrowBackIcon />} onClick={() => navigate(-1)}>{t("back")}</Button>
            </Stack>

            {/* رفع صور */}
            <Box mb={3}>
                <Button variant="contained" component="label" startIcon={<AddPhotoAlternateIcon />}>
                    {t("upload Photos")} 
                    <input type="file" hidden multiple accept="image/*" onChange={handleFileChange} />
                </Button>

                {uploadingFiles.map(f => (
                    <Box key={f.name} sx={{ mt: 1 }}>
                        <Typography variant="body2">{f.name}</Typography>
                        <LinearProgress variant="determinate" value={f.progress} />
                    </Box>
                ))}
            </Box>

            {/* زر حذف الصور المحددة */}
            {selectedPhotos.length > 0 && (
                <Box mb={2}>
                    <Button
                        variant="contained"
                        color="error"
                        startIcon={<DeleteIcon />}
                        onClick={() => setDeleteDialogOpen(true)}
                    >
                        {t("delete") + ' ' + `${selectedPhotos.length}` + ' ' + t("image")}
                    </Button>
                </Box>
            )}

            {/* عرض الصور */}
            {loading ? (
                <Typography>{t("Loading images...") }</Typography>
            ) : photos.length === 0 ? (
                    <Typography color="text.secondary">{t("No images have been uploaded for this page")}</Typography>
            ) : (
                        <Grid container spacing={2}>
                            {photos.map(p => {
                                const imagePath = p.image?.replace(/\\/g, "/");
                                const fullUrl = imagePath?.startsWith("http") ? imagePath : `${API_BASE_URL}${imagePath}`;
                                return (
                                    <Grid item xs={6} sm={4} md={3} key={p.id}>
                                        <Box
                                            sx={{
                                                position: "relative",
                                                borderRadius: 2,
                                                overflow: "hidden",
                                                border: "1px solid #ccc",
                                                width: "300px",
                                                height: 180, // ✅ ارتفاع ثابت لجميع الصور
                                                backgroundColor: "#f5f5f5",
                                            }}
                                        >
                                            <img
                                                src={fullUrl}
                                                alt=""
                                                style={{
                                                    width: "100%",
                                                    height: "100%",
                                                    objectFit: "cover", // ✅ يحافظ على النسبة ويملأ الصندوق
                                                }}
                                                onError={(e) => { e.target.src = "https://via.placeholder.com/180x180?text=No+Image"; }}
                                            />

                                            {/* Checkbox */}
                                            <Checkbox
                                                checked={selectedPhotos.includes(p.id)}
                                                onChange={() => toggleSelect(p.id)}
                                                sx={{
                                                    position: "absolute",
                                                    top: 8,
                                                    left: 8,
                                                    backgroundColor: "rgba(255,255,255,0.7)",
                                                    borderRadius: "50%",
                                                }}
                                            />

                                            {/* زر حذف فردي */}
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
                                                    setSelectedPhotos([p.id]);
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

            <ConfirmDialog
                open={deleteDialogOpen}
                onClose={() => setDeleteDialogOpen(false)}
                onConfirm={handleDeleteConfirm}
                title={t("confirmDeletion")}
                message={t("Are you sure you deleted the image?")}
                confirmText={t("delete")}
                cancelText={t("cancel")}
                loading={loading}
            />

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

export default PagePhotos;
