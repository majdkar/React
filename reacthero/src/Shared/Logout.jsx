// Logout.jsx
import React, { useState } from "react";
import { Dialog, DialogTitle, DialogContent, DialogActions, Button, Typography } from "@mui/material";
import { Navigate, useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";

const Logout = () => {
    const [open, setOpen] = useState(true);
    const [loggedOut, setLoggedOut] = useState(false);
    const navigate = useNavigate(); // ✅ لإدارة التوجيه
    const { t } = useTranslation();

    const handleConfirm = () => {
        localStorage.removeItem("token");
        setLoggedOut(true);
    };

    const handleCancel = () => {
        setOpen(false);
        // ✅ نرجع المستخدم للصفحة السابقة أو الرئيسية
        navigate(-1); // أو navigate("/") لو تفضل العودة للرئيسية
    };

    if (loggedOut) return <Navigate to="/login" replace />;

    return (
        <Dialog open={open} onClose={handleCancel}>
            <DialogTitle>{t("Confirm logout")}</DialogTitle>
            <DialogContent>
                <Typography>{t("Are you sure you want to log out?")}</Typography>
            </DialogContent>
            <DialogActions>
                <Button onClick={handleCancel} variant="outlined">{t("cancel")}</Button>
                <Button onClick={handleConfirm} variant="contained" color="error">{t("Log out")}</Button>
            </DialogActions>
        </Dialog>
    );
};

export default Logout;
