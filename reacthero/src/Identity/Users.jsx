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
import { useTranslation } from "react-i18next";
import ConfirmDialog from "../Shared/ConfirmDialog";
import AddUserDialog from "../Identity/AddUserDialog";
import CategoryIcon from "@mui/icons-material/Category"; // أيقونة مناسبة
import { useNavigate } from "react-router-dom";


const Users = () => {
    const { t, i18n } = useTranslation();
    const isArabic = i18n.language === "ar";
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down("sm"));
    const navigate = useNavigate();

    const [Users, setUsers] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
    const [selectedBlockCategoriesId, setSelectedBlockCategoriesId] = useState(null);
    const [deleting, setDeleting] = useState(false);

    const [snackbarOpen, setSnackbarOpen] = useState(false);
    const [snackbarMsg, setSnackbarMsg] = useState("");
    const [snackbarType, setSnackbarType] = useState("success");

    const [formDialogOpen, setFormDialogOpen] = useState(false);
    const [formMode, setFormMode] = useState("add"); // "add" أو "edit"
    const [formData, setFormData] = useState(null);
    const [formLoading, setFormLoading] = useState(false);
    const [refreshblockCategories, setRefreshBlockCategories] = useState([]);
    const token = localStorage.getItem("token");

    // 🔹 جلب الدول
    const fetchBlockCategories = async (isInitialLoad = false) => {
        if (isInitialLoad) setLoading(true);
        try {
            const response = await fetch(
                `${API_BASE_URL}api/identity/user`,
                {
                    headers: {
                        Authorization: `Bearer ${token}`,
                        "Content-Type": "application/json",
                    },
                }
            );
            if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
            const data = await response.json();
            setUsers(data.data);
        } catch (err) {
            setError(err.message);
        } finally {
            if (isInitialLoad) setLoading(false);
        }
    };

    useEffect(() => {
        fetchBlockCategories(true);
    }, [token]);

    // 🔹 أزرار الإضافة والتعديل
    const handleAddClick = () => {
        setFormMode("add");
        setFormData(null);
        setFormDialogOpen(true);
    };

    const handleEditClick = (City) => {
        setFormMode("edit");
        setFormData(City);
        setFormDialogOpen(true);
    };

    // 🔹 حذف دولة
    const handleDeleteClick = (id) => {
        setSelectedBlockCategoriesId(id);
        setDeleteDialogOpen(true);
    };

    const handleDeleteConfirm = async () => {
        if (!selectedBlockCategoriesId) return;
        setDeleting(true);
        try {
            const response = await fetch(
                `${API_BASE_URL }api/identity/user/?userId=${selectedBlockCategoriesId}`,
                {
                    method: "DELETE",
                    headers: {
                        Authorization: `Bearer ${token}`,
                        "Content-Type": "application/json",
                    },
                }
            );
            if (!response.ok) throw new Error(await response.text());

            setDeleteDialogOpen(false);
            showSnackbar("تم حذف المستخدم بنجاح ✅", "success");

            await fetchBlockCategories();
        }
        catch (err) {
            showSnackbar("فشل في حذف المستخدم ❌", "error");
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


    // 🔹 حفظ البيانات (إضافة أو تعديل)
    const handleFormSubmit = async (data) => {
        setFormLoading(true);
        try {
            if (formMode === "add") {
                const response = await fetch(`${API_BASE_URL}api/identity/user`, {
                    method: "POST",
                    headers: {
                        Authorization: `Bearer ${token}`,
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify(data),
                });
                if (!response.ok) throw new Error(await response.text());
                const resData = await response.json();
          

                showSnackbar("تمت إضافة المستخدم بنجاح ✅", "success");

                await fetchBlockCategories();
            }

            
            else if (formMode === "edit") {
                const response = await fetch(
                    `${API_BASE_URL}api/identity/user/?id=${formData.id}`,
                    {
                        method: "Put",
                        headers: {
                            Authorization: `Bearer ${token}`,
                            "Content-Type": "application/json",
                        },
                        body: JSON.stringify(data),
                    }
                );
                if (!response.ok) throw new Error(await response.text());

                const resData = await response.json();
                setUsers(prev =>
                    prev.map(c => (c.id === resData.data.id ? resData.data : c))
                );
                showSnackbar("تم تحديث المستخدم بنجاح ✅", "success");
            }


            setFormDialogOpen(false);
        } catch (err) {
            console.error(err);
            showSnackbar("فشل في العملية ❌", "error");
        } finally {
            setFormLoading(false);
        }
    };




    const handleUserRoleClick = (userId) => {
        navigate(`/UserRoles/${userId}`);
    };





    // 🔹 الأعمدة
    const columns = [
        { field: "firstName", headerName: t("firstName") || "First Name", flex: 1, minWidth: 80, headerAlign: "center", align: "center" },
        { field: "lastName", headerName: t("lastName") || "Last Name", flex: 1, minWidth: 80, headerAlign: "center", align: "center" },
        { field: "email", headerName: t("email") || "Email", flex: 1, minWidth: 80, headerAlign: "center", align: "center" },
        { field: "phoneNumber", headerName: t("phoneNumber") || "Phone Number", flex: 1, minWidth: 80, headerAlign: "center", align: "center" },
        { field: "emailConfirmed", headerName: t("emailConfirmed") || "Email Confirmed", flex: 1, minWidth: 80, headerAlign: "center", align: "center" },
        { field: "isActive", headerName: t("isActive") || "Is Active", flex: 1, minWidth: 80, headerAlign: "center", align: "center" },
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
                    key="userRole"
                    icon={<CategoryIcon sx={{ color: theme.palette.success.main }} />}
                    label={t("userRole") || "User Role"}
                    onClick={() => handleUserRoleClick(params.id)}
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
                <Typography variant="h5" sx={{ color: "#000", fontWeight: "bold" }}>
                    {t("users") || "Users"}
                </Typography>
                <Stack sx={{ gap: 1 }} direction="row" spacing={1}>
                    <Button variant="contained" color="primary" sx={{ gap: 1}} endIcon={<AddIcon />} onClick={handleAddClick}>
                        {t("addUser") || "Add User"}
                    </Button>
                    <Button variant="outlined" color="secondary" sx={{ gap: 1 }} endIcon={<RefreshIcon />} onClick={() => fetchBlockCategories(false)}>
                        {t("refresh") || "Refresh"}
                    </Button>
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
                    rows={Users}
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

            {/* نافذة الفورم */}
            <AddUserDialog
                open={formDialogOpen}
                onClose={() => setFormDialogOpen(false)}
                onSubmit={handleFormSubmit}
                mode={formMode}
                initialData={formData}
                loading={formLoading}
            />




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

export default Users;
