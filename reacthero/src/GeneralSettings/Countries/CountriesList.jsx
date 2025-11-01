import React, { useEffect, useState } from "react";

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
import ConfirmDialog from "../../Shared/ConfirmDialog";
import CountryFormDialog from "./CountryFormDialog";
import { API_BASE_URL } from "../../../config";

const CountriesList = () => {
    const { t, i18n } = useTranslation();
    const isArabic = i18n.language === "ar";
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down("sm"));

    const [countries, setCountries] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);
    const [selectedCountryId, setSelectedCountryId] = useState(null);
    const [deleting, setDeleting] = useState(false);

    const [snackbarOpen, setSnackbarOpen] = useState(false);
    const [snackbarMsg, setSnackbarMsg] = useState("");
    const [snackbarType, setSnackbarType] = useState("success");

    const [formDialogOpen, setFormDialogOpen] = useState(false);
    const [formMode, setFormMode] = useState("add"); // "add" أو "edit"
    const [formData, setFormData] = useState(null);
    const [formLoading, setFormLoading] = useState(false);

    const token = localStorage.getItem("token");

    // 🔹 جلب الدول
    const fetchCountries = async () => {
        setLoading(true);
        try {
            const response = await fetch(
                `${API_BASE_URL}api/v1/Countries/GetAll`,
                {
                    headers: {
                        Authorization: `Bearer ${token}`,
                        "Content-Type": "application/json",
                    },
                }
            );
            if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
            const data = await response.json();
            setCountries(data.data);
        } catch (err) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchCountries();
    }, [token]);

    // 🔹 أزرار الإضافة والتعديل
    const handleAddClick = () => {
        setFormMode("add");
        setFormData(null);
        setFormDialogOpen(true);
    };

    const handleEditClick = (country) => {
        setFormMode("edit");
        setFormData(country);
        setFormDialogOpen(true);
    };

    // 🔹 حذف دولة
    const handleDeleteClick = (id) => {
        setSelectedCountryId(id);
        setDeleteDialogOpen(true);
    };

    const handleDeleteConfirm = async () => {
        if (!selectedCountryId) return;
        setDeleting(true);
        try {
            const response = await fetch(
                `${API_BASE_URL}api/v1/Countries/${selectedCountryId}`,
                {
                    method: "DELETE",
                    headers: {
                        Authorization: `Bearer ${token}`,
                        "Content-Type": "application/json",
                    },
                }
            );
            if (!response.ok) throw new Error(await response.text());

            setCountries((prev) => prev.filter((c) => c.id !== selectedCountryId));
            setDeleteDialogOpen(false);
            showSnackbar("تم حذف الدولة بنجاح ✅", "success");
        } catch (err) {
            showSnackbar("فشل في حذف الدولة ❌", "error");
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
                const response = await fetch("https://localhost:44399/api/v1/Countries", {
                    method: "POST",
                    headers: {
                        Authorization: `Bearer ${token}`,
                        "Content-Type": "application/json",
                    },
                    body: JSON.stringify(data),
                });
                if (!response.ok) throw new Error(await response.text());
                const resData = await response.json();
                setCountries((prev) => [...prev, { ...data, id: resData.data }]);
                showSnackbar("تمت إضافة الدولة بنجاح ✅", "success");
            }



            else if (formMode === "edit") {
                const response = await fetch(
                    `${API_BASE_URL}api/v1/Countries`,
                    {
                        method: "POST",
                        headers: {
                            Authorization: `Bearer ${token}`,
                            "Content-Type": "application/json",
                        },
                        body: JSON.stringify(data),
                    }
                );
                if (!response.ok) throw new Error(await response.text());
                setCountries((prev) =>
                    prev.map((c) => (c.id === data.id ? data : c))
                );
                showSnackbar("تم تحديث الدولة بنجاح ✅", "success");
            }


            setFormDialogOpen(false);
        } catch (err) {
            showSnackbar("فشل في العملية ❌", "error");
        } finally {
            setFormLoading(false);
        }
    };

    // 🔹 الأعمدة
    const columns = [
        { field: "id", headerName: t("id") || "ID", width: 120, headerAlign: "center", align: "center" },
        { field: "nameEn", headerName: t("nameEn") || "Name (EN)", flex: 1, minWidth: 80, headerAlign: "center", align: "center" },
        { field: "nameAr", headerName: t("nameAr") || "Name (AR)", flex: 1, minWidth: 80, headerAlign: "center", align: "center" },
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
            ],
        },
    ];

    if (loading) return <Typography>Loading countries...</Typography>;
    if (error) return <Typography color="error">Error: {error}</Typography>;

    return (
        <Box sx={{ p: 2 }}>
            {/* أزرار التحكم */}
            <Stack direction="row" justifyContent="space-between" alignItems="center" mb={2} flexWrap="wrap" gap={1}>
                <Typography variant="h5" sx={{ color: "#000", fontWeight: "bold" }}>
                    {t("countries") || "Countries"}
                </Typography>
                <Stack sx={{gap :1}} direction="row" spacing={1}>
                    <Button variant="contained" color="primary" sx={{ gap: 1 }} startIcon={<AddIcon />} onClick={handleAddClick}>
                        {t("addCountry") || "Add Country"}
                    </Button>
                    <Button variant="outlined" color="secondary" sx={{ gap: 1 }} startIcon={<RefreshIcon />} onClick={fetchCountries}>
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
                    rows={countries}
                    columns={columns}
                    pageSizeOptions={[5, 10, 20]}
                    initialState={{ pagination: { paginationModel: { pageSize: 5, page: 0 } } }}
                    disableRowSelectionOnClick
                    autoHeight
                    sx={{ direction: isArabic ? "rtl" : "ltr" }}
                />
            </Box>

            {/* نافذة الفورم */}
            <CountryFormDialog
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
                message="هل أنت متأكد أنك تريد حذف هذه الدولة؟"
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

export default CountriesList;
