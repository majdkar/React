import React, { useEffect, useState } from "react";
import { API_BASE_URL } from "../../config";
import {
    Box,
    useTheme,
    useMediaQuery,
    Typography,
    Button,
    Stack,
    Snackbar,
    Alert,
} from "@mui/material";
import { DataGrid, GridActionsCellItem } from "@mui/x-data-grid";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";
import AddIcon from "@mui/icons-material/Add";
import RefreshIcon from "@mui/icons-material/Refresh";
import CategoryIcon from "@mui/icons-material/Category"; // أيقونة مناسبة
import CollectionsIcon from '@mui/icons-material/Collections';

import { useTranslation } from "react-i18next";
import ConfirmDialog from "../Shared/ConfirmDialog";
import AddBlockCategoryDialog from "../Blocks/AddBlockCategoryDialog";
import { useParams } from "react-router-dom"; // 👈 استيراد useParams
import { useNavigate } from "react-router-dom";

const Blocks = () => {
    const { t, i18n } = useTranslation();
    const isArabic = i18n.language === "ar";
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down("sm"));
    const navigate = useNavigate();
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [selectedBlockId, setSelectedBlockId] = useState(null);
    const [BlockName, setBlockName] = useState(null);

    const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
    const [selectedBlocksId, setSelectedBlocksId] = useState(null);
    const [deleting, setDeleting] = useState(false);

    const [snackbarOpen, setSnackbarOpen] = useState(false);
    const [snackbarMsg, setSnackbarMsg] = useState("");
    const [snackbarType, setSnackbarType] = useState("success");


    const { categoryId, categoryName } = useParams(); // 👈 الحصول على رقم الفئة من الـ URL

    const [formDialogOpen, setFormDialogOpen] = useState(false);
    const [formMode, setFormMode] = useState("add"); // "add" أو "edit"
    const [formData, setFormData] = useState(null);
    const [formLoading, setFormLoading] = useState(false);
    const [Blocks, setBlocks] = useState([]);
    const token = localStorage.getItem("token");

    // 🔹 جلب الدول
    const fetchBlocks = async (isInitialLoad = false, blockId = null) => {
        if (isInitialLoad) setLoading(true);
        try {
            let url = `${API_BASE_URL}api/Blocks/GetMaster?categoryId=${categoryId}`;
            if (blockId) url += `&blockId=${blockId}`; // ✅ إذا فيه بلوك، أضف للطلب

            const response = await fetch(url);
            if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
            const data = await response.json();
            setBlocks(data.items);
        } catch (err) {
            setError(err.message);
        } finally {
            if (isInitialLoad) setLoading(false);
        }
    };




    useEffect(() => {
        fetchBlocks(true);
    }, [token, categoryId]);

    //// 🔹 أزرار الإضافة والتعديل
    //const handleAddClick = () => {
    //    setFormMode("add");
    //    setFormData(null);
    //    setFormDialogOpen(true);
    //};


    const handleViewSubcategories = (blockId, nameAr) => {
        setBlockName(nameAr); // ✅ خزّن BlockName
        setSelectedBlockId(blockId); // ✅ خزّن blockId
        fetchBlocks(false, blockId); // ✅ أعد تحميل البيانات حسب البلوك
    };

    const handleEditClick = (block) => {
        navigate(`/blocks/${categoryId}/edit/${block.id}`);
    };

    // 🔹 حذف دولة
    const handleDeleteClick = (id) => {
        setSelectedBlocksId(id);
        setDeleteDialogOpen(true);
    };

    const handleDeleteConfirm = async () => {
        if (!selectedBlocksId) return;
        setDeleting(true);
        try {
            const response = await fetch(
                `${ API_BASE_URL }api/Blocks/${selectedBlocksId}`,
                {
                    method: "DELETE",
                    headers: {
                        Authorization: `Bearer ${token}`,
                        "Content-Type": "application/json",
                    },
                }
            );
            if (!response.ok) throw new Error(await response.text());

            setBlocks((prev) => prev.filter((c) => c.id !== selectedBlocksId));
            setDeleteDialogOpen(false);
            showSnackbar("تم حذف المدينة بنجاح ✅", "success");
        } catch (err) {
            showSnackbar("فشل في حذف المدينة ❌", "error");
        } finally {
            setDeleting(false);
        }
    };

    // 🔹 Snackbar
    const showSnackbar = (msg, type = "success") => {
        setSnackbarMsg(msg);
        setSnackbarType(type);
        setSnackbarOpen(true);
    };





    // 🔹 الأعمدة
    const columns = [
        { field: "id", headerName: t("id") || "ID", width: 120, headerAlign: "center", align: "center" },
        { field: "nameEn", headerName: t("nameEn") || "Name (EN)", flex: 1, minWidth: 80, headerAlign: "center", align: "center" },
        { field: "nameAr", headerName: t("nameAr") || "Name (AR)", flex: 1, minWidth: 80, headerAlign: "center", align: "center" },

        // ✅ العمود الجديد: فعال
        {
            field: "isActive",
            headerName: t("isActive") || "Active",
            width: 120,
            headerAlign: "center",
            align: "center",
            renderCell: (params) => params.value ? "✔️" : "❌",
        },

     

        // ✅ العمود الجديد: صورة
        {
            field: "image1",
            headerName: t("image") || "Image",
            width: 120,
            headerAlign: "center",
            align: "center",
            renderCell: (params) => params.value ? (
                <img src={`${API_BASE_URL}Files/UploadFiles/BlocksFiles/${params.value}`} alt="img" style={{ width: 50, height: 50, objectFit: "cover", borderRadius: 4 }} />
            ) : "-"
        },


        {
            field: "actions",
            type: "actions",
            headerName: t("actions") || "Actions",
            width: 150,
            headerAlign: "center",
            align: "center",
            getActions: (params) => [
                <GridActionsCellItem
                    key="edit"
                    icon={<EditIcon sx={{ color: theme.palette.info.main }} />}
                    label={t("edit") || "Edit"}
                    onClick={() => handleEditClick(params.row)}
                />,
                <GridActionsCellItem
                    key="delete"
                    icon={<DeleteIcon sx={{ color: theme.palette.error.main }} />}
                    label={t("delete") || "Delete"}
                    onClick={() => handleDeleteClick(params.id)}
                />,
                <GridActionsCellItem
                    key="subcategories"
                    icon={<CategoryIcon sx={{ color: theme.palette.primary.main }} />}
                    label={t("subCategories") || "Subcategories"}
                    onClick={() => handleViewSubcategories(params.row.id, `${params.row.nameAr + ' / ' + params.row.nameEn}`)}
                />,
                // ✅ زر جديد للانتقال إلى صفحة ألبوم الصور
                <GridActionsCellItem
                    key="photos"
                    icon={<CollectionsIcon sx={{ color: theme.palette.success.main }} />}
                    label="ألبوم الصور"
                    onClick={() => navigate(`/blocks/${categoryId}/photos/${params.row.id}/${encodeURIComponent(params.row.nameAr + ' / ' + params.row.nameEn)}`)}
                />,
            ],
        },
    ];

    if (loading) return <Typography>Loading Cities...</Typography>;
    if (error) return <Typography color="error">Error: {error}</Typography>;

    return (
        <Box sx={{ p: 2 }}>
            {/* أزرار التحكم */}
            <Stack direction="row" justifyContent="space-between" alignItems="center" mb={2} flexWrap="wrap" gap={1}>
                <Box sx={{ color: "#000", fontWeight: "bold" }}>
                    <Typography variant="h5">
                        {t("blocks") || "Blocks"}
                    </Typography>

                    {categoryName && (
                        <Typography variant="h5">
                            {categoryName}
                        </Typography>
                    )}

                    {BlockName && (
                        <Typography variant="h5" sx={{ color: "green" }}>
                            {BlockName}
                        </Typography>
                    )}
                </Box>

                <Stack sx={{ gap: 1 }} direction="row" spacing={1}>


                    <Button
                        variant="contained"
                        color="primary"
                        sx={{ gap: 1 }}
                        endIcon={<AddIcon />}
                        onClick={() => {
                            const blockIdToCopy = selectedBlockId || null;
                            // توجه إلى صفحة الإضافة فقط
                            navigate(`/blocks/${categoryId}/add${blockIdToCopy ? `?parentId=${blockIdToCopy}` : ""}/Name${categoryName }`);
                        }}
                    >
                        {t("addBlock") || "Add Block"}
                    </Button>


                    <Button variant="outlined" color="secondary" sx={{ gap: 1 }} endIcon={<RefreshIcon />} onClick={() => fetchBlocks(false)}>
                        {t("refresh") || "Refresh"}
                    </Button>

                    {selectedBlockId && (
                        <Button
                            variant="outlined"
                            color="secondary"
                            onClick={() => {
                                setSelectedBlockId(null);
                                fetchBlocks(false, null); // ✅ رجع كل البلوكات الأصلية
                            }}
                        >
                            رجوع إلى البلوكات
                        </Button>
                    )}
                </Stack>
            </Stack>

            {/* الجدول */}
            <Box
                sx={{
                    width: "100%",
                    height: isMobile ? 400 : 500,
                    "& .MuiDataGrid-root": { border: "none" },
                    "& .MuiDataGrid-cell": { borderBottom: "1px solid #e0e0e0", color: "#000" },
                    "& .MuiDataGrid-columnHeaders": { backgroundColor: theme.palette.primary.light, color: "#000", fontSize: 16, textAlign: "center" },
                    "& .MuiDataGrid-footerContainer": { backgroundColor: "#f9f9f9" },
                    "& .MuiDataGrid-row:hover": { backgroundColor: "#f1faff" },
                }}
            >
                <DataGrid
                    key={i18n.language}
                    rows={Blocks}
                    columns={columns}
                    getRowId={(row) => row.id || row.Id}
                    loading={loading}
                    pageSizeOptions={[5, 10, 20]}
                    initialState={{ pagination: { paginationModel: { pageSize: 5, page: 0 } } }}
                    disableRowSelectionOnClick
                    autoHeight
                    sx={{ direction: isArabic ? "rtl" : "ltr" }}
                />
            </Box>

  



            {/* نافذة تأكيد الحذف */}
            <ConfirmDialog
                open={deleteDialogOpen}
                onClose={() => setDeleteDialogOpen(false)}
                onConfirm={handleDeleteConfirm}
                title="تأكيد الحذف"
                message="هل أنت متأكد أنك تريد حذف هذه المدينة؟"
                confirmText="حذف"
                cancelText="إلغاء"
                loading={deleting}
            />

            {/* Snackbar */}
            <Snackbar
                open={snackbarOpen}
                autoHideDuration={3000}
                onClose={() => setSnackbarOpen(false)}
                anchorOrigin={{ vertical: "top", horizontal: "center" }}
            >
                <Alert onClose={() => setSnackbarOpen(false)} severity={snackbarType} sx={{ width: "100%" }}>
                    {snackbarMsg}
                </Alert>
            </Snackbar>
        </Box>
    );
};

export default Blocks;
