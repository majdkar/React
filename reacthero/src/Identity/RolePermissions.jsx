import React, { useEffect, useState } from "react";
import { API_BASE_URL } from "../../config";
import {
    Box,
    useTheme,
    useMediaQuery,
    Typography,
    Stack,
    Snackbar,
    Alert,
    Tabs,
    Tab,
    Button,
    Checkbox
} from "@mui/material";
import { DataGrid } from "@mui/x-data-grid";
import { useTranslation } from "react-i18next";
import { useParams } from "react-router-dom";
import { Badge } from "@mui/material";
import CheckCircleIcon from "@mui/icons-material/CheckCircle";



const RolePermissions = () => {

    const { t, i18n } = useTranslation();
    const isArabic = i18n.language === "ar";
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down("sm"));
    const { roleId } = useParams();
    const token = localStorage.getItem("token");

    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);
    const [pages, setPages] = useState([]);
    const [tabValue, setTabValue] = useState(0);
    const [groups, setGroups] = useState([]);
    const [snackbarOpen, setSnackbarOpen] = useState(false);
    const [snackbarMsg, setSnackbarMsg] = useState("");
    const [snackbarType, setSnackbarType] = useState("success");

    // 🔹 جلب البيانات
    const fetchPages = async () => {
        setLoading(true);
        try {
            const url = `${API_BASE_URL}api/identity/role/permissions/${roleId}`;
            const response = await fetch(url, {
                headers: {
                    Authorization: `Bearer ${token}`,
                    "Content-Type": "application/json",
                },
            });
            if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);
            const data = await response.json();

            const formatted = data.data.roleClaims.map((item, index) => ({
                ...item,
                id: index + 1,
            }));

            setPages(formatted);
            const uniqueGroups = [...new Set(formatted.map((item) => item.group))];
            setGroups(uniqueGroups);
        } catch (err) {
            setError(err.message);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchPages();
    }, [token, roleId]);

    const showSnackbar = (msg, type = "success") => {
        setSnackbarMsg(msg);
        setSnackbarType(type);
        setSnackbarOpen(true);
    };

    // ✅ تعديل حالة الصلاحية محليًا
    const handleTogglePermission = (id) => {
        setPages((prev) =>
            prev.map((item) =>
                item.id === id ? { ...item, selected: !item.selected } : item
            )
        );
    };

    // ✅ إرسال التحديث إلى الـ API
    const handleSavePermissions = async () => {
        try {
            const roleClaims = pages.map(({ id, type, value, description, group, selected }) => ({
                id: id || 0,
                roleId,
                type,
                value,
                description: description || "",
                group: group || "",
                selected,
            }));

            const payload = {
                roleId,
                roleClaims,
            };

            const url = `${API_BASE_URL}api/identity/role/permissions/updateall`;
            const response = await fetch(url, {
                method: "POST",
                headers: {
                    Authorization: `Bearer ${token}`,
                    "Content-Type": "application/json",
                },
                body: JSON.stringify(payload),
            });

            if (!response.ok) throw new Error(`HTTP error! status: ${response.status}`);

            showSnackbar("تم حفظ الصلاحيات بنجاح ✅", "success");
        } catch (error) {
            showSnackbar("حدث خطأ أثناء حفظ الصلاحيات ❌", "error");
        }
    };


    const columns = [
        { field: "type", headerName: t("type") || "Type", flex: 1, headerAlign: "center", align: "center" },
        { field: "value", headerName: t("value") || "Value", flex: 1, headerAlign: "center", align: "center" },
        {
            field: "selected",
            headerName: t("selected") || "Selected",
            flex: 0.6,
            headerAlign: "center",
            align: "center",
            renderCell: (params) => (
                <Checkbox
                    checked={params.row.selected || false}
                    onChange={() => handleTogglePermission(params.row.id)}
                    color="primary"
                />
            ),
        },
    ];

    if (loading) return <Typography>Loading Permissions...</Typography>;
    if (error) return <Typography color="error">Error: {error}</Typography>;

    const currentGroup = groups[tabValue];
    const filteredRows = pages.filter((item) => item.group === currentGroup);

    return (
        <Box sx={{ p: 2 }}>
            <Stack direction="row" justifyContent="space-between" alignItems="center" mb={2}>
                <Typography variant="h5">
                    {t("rolePermissions") || "Role Permissions"}
                </Typography>

                {/* 🔹 زر تعديل الصلاحيات */}
                <Button
                    variant="contained"
                    color="primary"
                    onClick={handleSavePermissions}
                >
                    {t("editPermissions") || "تعديل الصلاحيات"}
                </Button>
            </Stack>

            {/* Tabs */}
            <Box sx={{ overflowX: "auto" }}>
            <Tabs 
                value={tabValue}
                onChange={(e, newValue) => setTabValue(newValue)}
                variant="scrollable"
                scrollButtons="auto"
                sx={{
                    borderBottom: 1,
                    borderColor: "divider",
                    mb: 2,
                    display: "flex",
                    alignItems: "center",
                    minWidth: "fit-content",
                    textAlign: isArabic ? "right" : "left",
                }}
            >
                {groups.map((group, index) => {
                    const groupItems = pages.filter((item) => item.group === group);
                    const selectedCount = groupItems.filter((item) => item.selected).length;
                    const totalCount = groupItems.length;

                    // 🎨 اللون حسب الحالة
                    let badgeColor = "default";
                    if (selectedCount === totalCount && totalCount > 0) badgeColor = "success";
                    else if (selectedCount > 0) badgeColor = "warning";
                    else badgeColor = "error";

                    return (
                        <Tab
                            key={index}
                            label={
                                <Stack direction="row" spacing={1} alignItems="center">
                                    <Typography>{group}</Typography>
                                    <Badge
                                        badgeContent={selectedCount}
                                        color={badgeColor}
                                        sx={{
                                            "& .MuiBadge-badge": {
                                                right: 8,
                                                top: -12,
                                                fontSize: "0.75rem",
                                                fontWeight: "bold",
                                                minWidth: 22,
                                                height: 22,
                                                borderRadius: "50%",
                                            },
                                        }}
                                    />
                                </Stack>
                            }
                        />
                    );
                })}
            </Tabs>
            </Box>

            {/* DataGrid */}
            <Box
                sx={{
                    width: "100%",
                    "& .MuiDataGrid-root": { border: "none" },
                    "& .MuiDataGrid-cell": { borderBottom: "1px solid #e0e0e0", color: "#000" },
                    "& .MuiDataGrid-columnHeaders": { backgroundColor: theme.palette.primary.light, color: "#000", fontSize: 16 },
                    "& .MuiDataGrid-row:hover": { backgroundColor: "#f1faff" },
                }}
            >
                <DataGrid
                    key={tabValue + i18n.language}
                    rows={filteredRows}
                    columns={columns}
                    getRowId={(row) => row.id}
                    pageSizeOptions={[5, 10, 20]}
                    autoHeight
                    disableRowSelectionOnClick
                    sx={{ direction: isArabic ? "rtl" : "ltr" }}
                />
            </Box>

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

export default RolePermissions;
