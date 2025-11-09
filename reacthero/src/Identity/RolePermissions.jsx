import React, { useEffect, useState, useRef } from "react";
import { API_BASE_URL } from "../../config";
import {
    Box,
    useTheme,
    Typography,
    Stack,
    Snackbar,
    Alert,
    Tabs,
    Tab,
    Button,
    Checkbox,
    Badge,
    IconButton
} from "@mui/material";
import { DataGrid } from "@mui/x-data-grid";
import { useTranslation } from "react-i18next";
import { useParams } from "react-router-dom";
import ChevronLeftIcon from "@mui/icons-material/ChevronLeft";
import ChevronRightIcon from "@mui/icons-material/ChevronRight";

const RolePermissions = () => {
    const { t, i18n } = useTranslation();
    const isArabic = i18n.language === "ar";
    const theme = useTheme();
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

    const tabsRef = useRef(null);

    // 🔹 Fetch pages
    const fetchPages = async () => {
        setLoading(true);
        try {
            const url = `${API_BASE_URL}api/identity/role/permissions/${roleId}`;
            const response = await fetch(url, {
                headers: { Authorization: `Bearer ${token}`, "Content-Type": "application/json" },
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

    useEffect(() => { fetchPages(); }, [token, roleId]);

    const showSnackbar = (msg, type = "success") => {
        setSnackbarMsg(msg);
        setSnackbarType(type);
        setSnackbarOpen(true);
    };

    const handleTogglePermission = (id) => {
        setPages(prev =>
            prev.map(item => item.id === id ? { ...item, selected: !item.selected } : item)
        );
    };

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
            const payload = { roleId, roleClaims };
            const url = `${API_BASE_URL}api/identity/role/permissions/updateall`;
            const response = await fetch(url, {
                method: "POST",
                headers: { Authorization: `Bearer ${token}`, "Content-Type": "application/json" },
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
        { field: "group", headerName: t("group") || "Group", flex: 1, headerAlign: "center", align: "center" },
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

    if (loading) return <Typography>Loading Permissions...</Typography>;
    if (error) return <Typography color="error">Error: {error}</Typography>;

    const currentGroup = tabValue === 0 ? "all" : groups[tabValue - 1];
    const filteredRows = currentGroup === "all" ? pages : pages.filter(item => item.group === currentGroup);

    const scrollTabs = (direction) => {
        if (tabsRef.current) {
            tabsRef.current.scrollLeft += direction * 150;
        }
    };

    return (
        <Box sx={{ p: 2 }}>
            <Stack direction="row" justifyContent="space-between" alignItems="center" mb={2}>
                <Typography variant="h5">{t("rolePermissions") || "Role Permissions"}</Typography>
                <Button variant="contained" color="primary" onClick={handleSavePermissions}>{t("editPermissions") || "تعديل الصلاحيات"}</Button>
            </Stack>

            {/* Tabs with arrows */}
            <Box sx={{ display: "flex", alignItems: "center", mb: 2 }}>
                <IconButton onClick={() => scrollTabs(-1)}><ChevronLeftIcon /></IconButton>
                <Box sx={{ overflowX: "hidden", flex: 1, whiteSpace: "nowrap" }} ref={tabsRef}>
                    <Tabs
                        value={tabValue}
                        onChange={(e, newValue) => setTabValue(newValue)}
                        variant="scrollable"
                        scrollButtons="off"
                        sx={{ display: "inline-flex", borderBottom: 1, borderColor: "divider" }}
                    >
                        {/* All Tab */}
                        <Tab
                            key={-1}
                            sx={{ minWidth: 150 }}
                            label={
                                <Stack direction="row" spacing={1} alignItems="center">
                                    <Typography>{isArabic ? "الكل" : "All"}</Typography>
                                    <Badge badgeContent={pages.filter(p => p.selected).length} color="primary"
                                        sx={{
                                            "& .MuiBadge-badge": {
                                                fontSize: "0.75rem", top: -8,
                                                right: 10, fontWeight: "bold", minWidth: 22, height: 22, borderRadius: "50%" } }} />
                                </Stack>
                            }
                        />
                        {groups.map((group, index) => {
                            const groupItems = pages.filter(p => p.group === group);
                            const selectedCount = groupItems.filter(p => p.selected).length;
                            let badgeColor = "default";
                            if (selectedCount === groupItems.length && groupItems.length > 0) badgeColor = "success";
                            else if (selectedCount > 0) badgeColor = "warning";
                            else badgeColor = "error";
                            return (
                                <Tab
                                    key={index}
                                    sx={{ minWidth: 150 }}
                                    label={
                                        <Stack direction="row" spacing={1} alignItems="center">
                                            <Typography>{group}</Typography>
                                            <Badge badgeContent={selectedCount} color={badgeColor}
                                                sx={{ "& .MuiBadge-badge": { fontSize: "0.75rem", fontWeight: "bold", minWidth: 22, height: 22, borderRadius: "50%" } }} />
                                        </Stack>
                                    }
                                />
                            );
                        })}
                    </Tabs>
                </Box>
                <IconButton onClick={() => scrollTabs(1)}><ChevronRightIcon /></IconButton>
            </Box>

            {/* DataGrid */}
            <Box sx={{
                width: "100%",
                "& .MuiDataGrid-root": { border: "none" },
                "& .MuiDataGrid-cell": { borderBottom: "1px solid #e0e0e0", color: "#000" },
                "& .MuiDataGrid-columnHeaders": { backgroundColor: theme.palette.primary.light, color: "#000", fontSize: 16 },
                "& .MuiDataGrid-row:hover": { backgroundColor: "#f1faff" },
            }}>
                <DataGrid
                    key={tabValue + i18n.language}
                    rows={filteredRows}
                    columns={columns}
                    getRowId={row => row.id}
                    pageSizeOptions={[5, 10, 20]}
                    autoHeight
                    disableRowSelectionOnClick
                    sx={{ direction: isArabic ? "rtl" : "ltr" }}
                />
            </Box>

            <Snackbar open={snackbarOpen} autoHideDuration={3000} onClose={() => setSnackbarOpen(false)}
                anchorOrigin={{ vertical: "top", horizontal: "center" }}>
                <Alert onClose={() => setSnackbarOpen(false)} severity={snackbarType} sx={{ width: "100%" }}>
                    {snackbarMsg}
                </Alert>
            </Snackbar>
        </Box>
    );
};

export default RolePermissions;
