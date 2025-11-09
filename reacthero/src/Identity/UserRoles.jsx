import React, { useEffect, useState } from "react";
import { API_BASE_URL } from "../../config";
import {
    Box,
    useTheme,
    useMediaQuery,
    Typography,
    Button,
    Checkbox,
    Stack,
    Snackbar,
    Alert,
} from "@mui/material";
import { DataGrid, GridActionsCellItem } from "@mui/x-data-grid";
import EditIcon from "@mui/icons-material/Edit";
import AddIcon from "@mui/icons-material/Add";
import { useTranslation } from "react-i18next";
import { useNavigate, useParams } from "react-router-dom";


const UserRoles = () => {
    const { t, i18n } = useTranslation();
    const isArabic = i18n.language === "ar";
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down("sm"));
    const navigate = useNavigate();

    const [BlockCategories, setBlockCategories] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
    const [selectedBlockCategoriesId, setSelectedBlockCategoriesId] = useState(null);

    const [snackbarOpen, setSnackbarOpen] = useState(false);
    const [snackbarMsg, setSnackbarMsg] = useState("");
    const [snackbarType, setSnackbarType] = useState("success");
    const { userId } = useParams();

    const token = localStorage.getItem("token");

    // 🔹 جلب الدول
    const fetchBlockCategories = async (isInitialLoad = false) => {
        if (isInitialLoad) setLoading(true);
        try {
            const response = await fetch(
                `${API_BASE_URL}api/identity/user/roles/${userId}`,
                {
                    headers: {
                        Authorization: `Bearer ${token}`,
                        "Content-Type": "application/json",
                    },
                }
            );
            if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
            const data = await response.json();

            const formatted = data.data.userRoles.map((item, index) => ({
                ...item,
                id: index + 1,
            }));

            setBlockCategories(formatted);

        } catch (err) {
            setError(err.message);
        } finally {
            if (isInitialLoad) setLoading(false);
        }
    };

    useEffect(() => {
        fetchBlockCategories(true);
    }, [token]);



    // 🔹 Snackbar
    const showSnackbar = (msg, type = "success") => {
        setSnackbarMsg(msg);
        setSnackbarType(type);
        setSnackbarOpen(true);
    };


    // 🔹 حفظ البيانات (إضافة أو تعديل)
    const handleAddClick = async () => {
        try {

            const userRoles = BlockCategories.map(({ roleName, roleDescription, selected }) => ({
                roleName,
                roleDescription,
                selected,
            }));
            const payload = { userId, userRoles };
            const url = `${API_BASE_URL}api/identity/user/roles/${userId}`;
            const response = await fetch(url, {
                method: "PUT",
                headers: { Authorization: `Bearer ${token}`, "Content-Type": "application/json" },
                body: JSON.stringify(payload),
            });

            if (!response.ok) throw new Error(await response.text());
            const resData = await response.json();

            showSnackbar("تمت تغيير الرول بنجاح ✅", "success");

            await fetchBlockCategories();

        } catch (err) {
            showSnackbar("فشل في العملية ❌", err);
        } 
    };


    const handleTogglePermission = (id) => {
        setBlockCategories(prev =>
            prev.map(item => item.id === id ? { ...item, selected: !item.selected } : item)
        );
    };


    // 🔹 الأعمدة
    const columns = [
        { field: "roleName", headerName: t("roleName") || "Role Name", flex: 1, minWidth: 80, headerAlign: "center", align: "center" },
        { field: "roleDescription", headerName: t("roleDescription") || "Role Description", flex: 1, minWidth: 80, headerAlign: "center", align: "center" },
        {
            field: "selected",
            headerName: t("status") || "Status",
            flex: 0.6,
            headerAlign: "center",
            align: "center",
            renderCell: (params) => (
                <Checkbox checked={params.row.selected || false} onChange={() => handleTogglePermission(params.row.id)} color="primary" />
            ),
        },
     
    ];

    if (loading) return <Typography>Loading Cities...</Typography>;
    if (error) return <Typography color="error">Error: {error}</Typography>;

    return (
        <Box sx={{ p: 2 }}>
            {/* أزرار التحكم */}
            <Stack direction="row" justifyContent="space-between" alignItems="center" mb={2} flexWrap="wrap" gap={1}>
                <Typography variant="h5" sx={{ color: "#000", fontWeight: "bold" }}>
                    {t("role") || "Role"}
                </Typography>
                <Stack sx={{ gap: 1 }} direction="row" spacing={1}>
                    <Button variant="contained" color="primary" sx={{ gap: 1}} endIcon={<AddIcon />} onClick={handleAddClick}>
                        {t("editRole") || "Edit Role"}
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
                    rows={BlockCategories}
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

export default UserRoles;
