// Logout.jsx
import React, { useState } from "react";
import { Dialog, DialogTitle, DialogContent, DialogActions, Button, Typography } from "@mui/material";
import { Navigate } from "react-router-dom";

const Logout = () => {
    const [open, setOpen] = useState(true);
    const [loggedOut, setLoggedOut] = useState(false);

    const handleConfirm = () => {
        localStorage.removeItem("token");
        setLoggedOut(true);
    };

    const handleCancel = () => {
        setOpen(false);
    };

    if (loggedOut) return <Navigate to="/login" replace />;

    return (
        <Dialog open={open} onClose={handleCancel}>
            <DialogTitle>تأكيد تسجيل الخروج</DialogTitle>
            <DialogContent>
                <Typography>هل أنت متأكد أنك تريد تسجيل الخروج؟</Typography>
            </DialogContent>
            <DialogActions>
                <Button onClick={handleCancel} variant="outlined">إلغاء</Button>
                <Button onClick={handleConfirm} variant="contained" color="error">تسجيل الخروج</Button>
            </DialogActions>
        </Dialog>
    );
};

export default Logout;
