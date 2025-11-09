import React, { useEffect, useState } from "react";
import {
    Box,
    Typography,
    Button,
    Stack,
    Snackbar,
    Alert,
    Grid,
    LinearProgress,
    Checkbox,
} from "@mui/material";
import { API_BASE_URL } from "../../config";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";
import AttachFileIcon from "@mui/icons-material/AttachFile";
import DeleteIcon from "@mui/icons-material/Delete";
import { useParams, useNavigate } from "react-router-dom";
import ConfirmDialog from "../Shared/ConfirmDialog";
import axios from "axios";
import { useTranslation } from "react-i18next";

const PageAttachments = () => {
    const { pageId, pageName } = useParams();
    const navigate = useNavigate();
    const token = localStorage.getItem("token");
    const { t } = useTranslation();

    const [attachments, setAttachments] = useState([]);
    const [loading, setLoading] = useState(false);
    const [uploadingFiles, setUploadingFiles] = useState([]);
    const [snackbar, setSnackbar] = useState({
        open: false,
        message: "",
        type: "success",
    });
    const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
    const [selectedAttachments, setSelectedAttachments] = useState([]);

    // 🔹 جلب الملفات الحالية
    const fetchAttachments = async () => {
        try {
            setLoading(true);
            const res = await fetch(

            `${API_BASE_URL}api/PageAttachement/GetAttachementByPageId/?id=${pageId}`
            );
            const data = await res.json();
            setAttachments(data.items || data || []);
        } catch (err) {
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    // 🔹 رفع ملفات متعددة مع Progress
    const handleFileChange = async (e) => {
        const files = Array.from(e.target.files);
        if (!files.length) return;

        const uploads = files.map((f) => ({ name: f.name, progress: 0 }));
        setUploadingFiles(uploads);

        for (let i = 0; i < files.length; i++) {
            const file = files[i];
            try {
                const formData = new FormData();
                formData.append("file", file);

                // 🔸 رفع الملف فعليًا
                const uploadRes = await axios.post(
                    `${API_BASE_URL}api/FileUpload/3/2`,
                    formData,
                    {
                        headers: { Authorization: `Bearer ${token}` },
                        onUploadProgress: (progressEvent) => {
                            const percent = Math.round(
                                (progressEvent.loaded * 100) / progressEvent.total
                            );
                            setUploadingFiles((prev) => {
                                const copy = [...prev];
                                copy[i].progress = percent;
                                return copy;
                            });
                        },
                    }
                );

                const fileName = uploadRes.data.replace(/"/g, "");
                const fileUrl = `Files/UploadFiles/PagesFiles/${fileName}`;

                // 🔸 حفظ المسار في قاعدة البيانات
                await fetch(`${API_BASE_URL}api/PageAttachement`, {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json",
                        Authorization: `Bearer ${token}`,
                    },
                    body: JSON.stringify({ file: fileUrl, name: fileUrl, pageId: Number(pageId) }),
                });
            } catch (err) {
                console.error(t("Upload failed") + "❌", file.name, err);
            }
        }

        setUploadingFiles([]);
        fetchAttachments();
        setSnackbar({
            open: true,
            message: t("has been uploaded") + ` ${files.length}` + t("file successfully"),
            type: "success",
        });
        e.target.value = "";
    };

    // 🔹 حذف الملفات المحددة
    const handleDeleteConfirm = async () => {
        if (!selectedAttachments.length) return;

        try {
            setLoading(true);
            for (const attachmentId of selectedAttachments) {
                const res = await fetch(
                    `${API_BASE_URL}api/PageAttachement/${attachmentId}`,
                    {
                        method: "DELETE",
                        headers: { Authorization: `Bearer ${token}` },
                    }
                );
                if (!res.ok) console.error(await res.text());
            }
            fetchAttachments();
            setSnackbar({
                open: true,
                message: t("The selected files have been deleted") + "✅",
                type: "success",
            });
        } catch (err) {
            console.error(err);
            setSnackbar({
                open: true,
                message: t("Failed to delete files") + "❌",
                type: "error",
            });
        } finally {
            setLoading(false);
            setDeleteDialogOpen(false);
            setSelectedAttachments([]);
        }
    };

    useEffect(() => {
        fetchAttachments();
    }, [pageId]);

    // 🔹 toggle selection
    const toggleSelect = (id) => {
        setSelectedAttachments((prev) =>
            prev.includes(id) ? prev.filter((x) => x !== id) : [...prev, id]
        );
    };

    return (
        <Box sx={{ p: 3 }}>
            <Stack direction="row" alignItems="center" justifyContent="space-between" mb={3}>
                <Typography variant="h5">📎 {t("page Attachments")} {pageName}</Typography>
                <Button
                    variant="outlined"
                    startIcon={<ArrowBackIcon />}
                    onClick={() => navigate(-1)}
                >
                    {t("back")}
                </Button>
            </Stack>

            {/* رفع ملفات */}
            <Box mb={3}>
                <Button
                    variant="contained"
                    component="label"
                    startIcon={<AttachFileIcon />}
                >
                    {t("upload Files")}
                    <input
                        type="file"
                        hidden
                        multiple
                        onChange={handleFileChange}
                        accept=".pdf,.doc,.docx,.xls,.xlsx,.zip,.rar,.txt"
                    />
                </Button>

                {uploadingFiles.map((f) => (
                    <Box key={f.name} sx={{ mt: 1 }}>
                        <Typography variant="body2">{f.name}</Typography>
                        <LinearProgress variant="determinate" value={f.progress} />
                    </Box>
                ))}
            </Box>

            {/* زر حذف الملفات المحددة */}
            {selectedAttachments.length > 0 && (
                <Box mb={2}>
                    <Button
                        variant="contained"
                        color="error"
                        startIcon={<DeleteIcon />}
                        onClick={() => setDeleteDialogOpen(true)}
                    >
                        {t("delete") + '  ' + `${selectedAttachments.length}` + '  '  + t("file")} 
                    </Button>
                </Box>
            )}

            {/* عرض الملفات */}
            {loading ? (
                <Typography>{t("Loading files...")}</Typography>
            ) : attachments.length === 0 ? (
                <Typography color="text.secondary">
                        {t("No attachments have been uploaded for this page")}
                </Typography>
            ) : (
                <Grid container spacing={2}>
                            {attachments.map((p) => {
                                const filePath = p.file?.replace(/\\/g, "/");
                        const fullUrl = filePath?.startsWith("http")
                            ? filePath
                            : `${API_BASE_URL}${filePath}`;

                        return (
                            <Grid item xs={12} sm={6} md={4} key={p.id}>
                                <Box
                                    sx={{
                                        position: "relative",
                                        borderRadius: 2,
                                        border: "1px solid #ccc",
                                        p: 2,
                                        backgroundColor: "#f9f9f9",
                                    }}
                                >
                                    <Typography
                                        variant="body2"
                                        sx={{
                                            mb: 1,
                                            wordBreak: "break-word",
                                        }}
                                    >
                                        📄 {p.filePath?.split("/").pop()}
                                    </Typography>

                                    <Stack direction="row" spacing={1}>
                                        <Button
                                            variant="outlined"
                                            size="small"
                                            href={fullUrl}
                                            target="_blank"
                                        >
                                            {t("View/Download")}
                                        </Button>
                                        <Checkbox
                                            checked={selectedAttachments.includes(p.id)}
                                            onChange={() => toggleSelect(p.id)}
                                        />
                                        <Button
                                            size="small"
                                            color="error"
                                            variant="contained"
                                            sx={{ minWidth: 0 }}
                                            onClick={() => {
                                                setSelectedAttachments([p.id]);
                                                setDeleteDialogOpen(true);
                                            }}
                                        >
                                            ✖
                                        </Button>
                                    </Stack>
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
                title={t("confirmDeletion") }
                message={t("Are you sure you deleted the file?")}
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

export default PageAttachments;
