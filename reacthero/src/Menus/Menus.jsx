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
import MovieIcon from '@mui/icons-material/Movie';
import Tooltip from '@mui/material/Tooltip';

import { useTranslation } from "react-i18next";
import ConfirmDialog from "../Shared/ConfirmDialog";
import AddMenuCategoryDialog from "../Menus/AddMenuCategoryDialog";
import { useParams } from "react-router-dom"; // 👈 استيراد useParams
import { useNavigate } from "react-router-dom";

const Menus = () => {
    const { t, i18n } = useTranslation();
    const isArabic = i18n.language === "ar";
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down("sm"));
    const navigate = useNavigate();
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [selectedMenuId, setSelectedMenuId] = useState(null);
    const [MenuName, setMenuName] = useState(null);

    const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
    const [selectedMenusId, setSelectedMenusId] = useState(null);
    const [deleting, setDeleting] = useState(false);

    const [snackbarOpen, setSnackbarOpen] = useState(false);
    const [snackbarMsg, setSnackbarMsg] = useState("");
    const [snackbarType, setSnackbarType] = useState("success");


    const { categoryId, categoryName } = useParams(); // 👈 الحصول على رقم الفئة من الـ URL

    const [formDialogOpen, setFormDialogOpen] = useState(false);
    const [formMode, setFormMode] = useState("add"); // "add" أو "edit"
    const [formData, setFormData] = useState(null);
    const [formLoading, setFormLoading] = useState(false);
    const [Menus, setMenus] = useState([]);
    const token = localStorage.getItem("token");

    // 🔹 جلب الدول
    const fetchMenus = async (isInitialLoad = false, MenuId = null) => {
        if (isInitialLoad) setLoading(true);
        try {
            let url = `${API_BASE_URL}api/v1/Menus/GetMaster?categoryId=${categoryId}`;
            if (MenuId) url += `&MenuId=${MenuId}`; // ✅ إذا فيه بلوك، أضف للطلب

            const response = await fetch(url);
            if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
            const data = await response.json();
            setMenus(data.items);
        } catch (err) {
            setError(err.message);
        } finally {
            if (isInitialLoad) setLoading(false);
        }
    };




    useEffect(() => {
        fetchMenus(true);
    }, [token, categoryId]);

    //// 🔹 أزرار الإضافة والتعديل
    //const handleAddClick = () => {
    //    setFormMode("add");
    //    setFormData(null);
    //    setFormDialogOpen(true);
    //};


    const handleViewSubcategories = (MenuId, nameAr) => {
        setMenuName(nameAr); // ✅ خزّن MenuName
        setSelectedMenuId(MenuId); // ✅ خزّن MenuId
        fetchMenus(false, MenuId); // ✅ أعد تحميل البيانات حسب البلوك
    };

    const handleEditClick = (Menu) => {
        navigate(`/Menus/${categoryId}/edit/${Menu.id}`);
    };

    // 🔹 حذف دولة
    const handleDeleteClick = (id) => {
        setSelectedMenusId(id);
        setDeleteDialogOpen(true);
    };

    const handleDeleteConfirm = async () => {
        if (!selectedMenusId) return;
        setDeleting(true);
        try {
            const response = await fetch(
                `${API_BASE_URL }api/v1/Menus/${selectedMenusId}`,
                {
                    method: "DELETE",
                    headers: {
                        Authorization: `Bearer ${token}`,
                        "Content-Type": "application/json",
                    },
                }
            );
            if (!response.ok) throw new Error(await response.text());

            setMenus((prev) => prev.filter((c) => c.id !== selectedMenusId));
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
        { field: "id", headerName: t("id") || "ID", width: 80, flex: 1, headerAlign: "center", align: "center" },
        { field: "nameEn", headerName: t("nameEn") || "Name (EN)", flex: 1, Width: 30, headerAlign: "center", align: "center" },
        { field: "nameAr", headerName: t("nameAr") || "Name (AR)", flex: 1, Width: 30, headerAlign: "center", align: "center" },

        // ✅ العمود الجديد: فعال
        {
            field: "isActive",
            headerName: t("isActive") || "Active",
            width: 80, flex: 1,
            headerAlign: "center",
            align: "center",
            renderCell: (params) => params.value ? "✔️" : "❌",
        },

     

        // ✅ العمود الجديد: صورة
        {
            field: "image1",
            headerName: t("image") || "Image",
            flex: 1,
            width: 90,
            headerAlign: "center",
            align: "center",
            renderCell: (params) => params.value ? (
                <img src={`${API_BASE_URL}Files/UploadFiles/MenusFiles/${params.value}`} alt="img" style={{ width: 50, height: 50, objectFit: "cover", borderRadius: 4 }} />
            ) : "-"
        },


        {
            field: "actions",
            type: "actions",
            headerName: t("actions") || "Actions",
            width: 80,
            flex: 1,
            headerAlign: "center",
            align: "center",
            getActions: (params) => [
                <GridActionsCellItem
                    key="edit"
                    icon={
                        <Tooltip title={t("edit") || "Edit"}>
                            <EditIcon sx={{ color: theme.palette.info.main }} />
                        </Tooltip>
                    }
                    label={t("edit") || "Edit"}
                    onClick={() => handleEditClick(params.row)}
                />,

                <GridActionsCellItem
                    key="delete"
                    icon={
                        <Tooltip title={t("delete") || "Delete"}>
                            <DeleteIcon sx={{ color: theme.palette.error.main }} />
                        </Tooltip>
                    }
                    label={t("delete") || "Delete"}
                    onClick={() => handleDeleteClick(params.id)}
                />,

                <GridActionsCellItem
                    key="subcategories"
                    icon={
                        <Tooltip title={t("subCategories") || "Subcategories"}>
                            <CategoryIcon sx={{ color: theme.palette.dark }} />
                        </Tooltip>
                    }
                    label={t("subCategories") || "Subcategories"}
                    onClick={() => handleViewSubcategories(params.row.id, `${params.row.nameAr} / ${params.row.nameEn}`)}
                />,

                <GridActionsCellItem
                    key="photos"
                    icon={
                        <Tooltip title="ألبوم الصور">
                            <CollectionsIcon sx={{ color: theme.palette.success.main }} />
                        </Tooltip>
                    }
                    label="ألبوم الصور"
                    onClick={() => navigate(`/Menus/${categoryId}/photos/${params.row.id}/${encodeURIComponent(params.row.nameAr + ' / ' + params.row.nameEn)}`)}
                />,

                <GridActionsCellItem
                    key="videos"
                    icon={
                        <Tooltip title="ألبوم الفيديو">
                            <MovieIcon sx={{ color: theme.palette.secondary.main }} />
                        </Tooltip>
                    }
                    label="ألبوم الفيديو"
                    onClick={() => navigate(`/Menus/${categoryId}/videos/${params.row.id}/${encodeURIComponent(params.row.nameAr + ' / ' + params.row.nameEn)}`)}
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
                        {t("Menus") || "Menus"}
                    </Typography>

                    {categoryName && (
                        <Typography variant="h5">
                            {categoryName}
                        </Typography>
                    )}

                    {MenuName && (
                        <Typography variant="h5" sx={{ color: "green" }}>
                            {MenuName}
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
                            const MenuIdToCopy = selectedMenuId || null;
                            // توجه إلى صفحة الإضافة فقط
                            navigate(`/menus/${categoryId}/add?categoryName=${encodeURIComponent(categoryName)}${MenuIdToCopy ? `&parentId=${MenuIdToCopy}` : ""}`);
                        }}
                    >
                        {t("addMenu") || "Add Menu"}
                    </Button>


                    <Button variant="outlined" color="secondary" sx={{ gap: 1 }} endIcon={<RefreshIcon />} onClick={() => fetchMenus(false)}>
                        {t("refresh") || "Refresh"}
                    </Button>

                    {selectedMenuId && (
                        <Button
                            variant="outlined"
                            color="secondary"
                            onClick={() => {
                                setSelectedMenuId(null);
                                fetchMenus(false, null); // ✅ رجع كل البلوكات الأصلية
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
                    rows={Menus}
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

export default Menus;
