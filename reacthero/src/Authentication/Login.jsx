import React, { useState } from "react";
import {
    TextField,
    Button,
    Checkbox,
    FormControlLabel,
    IconButton,
    InputAdornment,
    Grid,
    Typography,
    Card,
    CardContent,
} from "@mui/material";
import { Visibility, VisibilityOff } from "@mui/icons-material";
import { useNavigate } from "react-router-dom";

export default function Login() {
    const navigate = useNavigate();
    const [emailOrUserName, setEmailOrUserName] = useState("");
    const [password, setPassword] = useState("");
    const [showPassword, setShowPassword] = useState(false);
    const [rememberMe, setRememberMe] = useState(false);
    const [error, setError] = useState("");

    const togglePasswordVisibility = () => setShowPassword((prev) => !prev);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError("");

        try {
            const response = await fetch("https://localhost:44399/api/identity/token", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({ emailOrUserName, password }),
            });
            const data = await response.json();

            if (response.ok && data.succeeded) {
                localStorage.setItem("token", data.data.token);
                localStorage.setItem("userId", data.data.userId);
                navigate("/", { replace: true });
            } else {
                setError(data.messages?.join(", ") || "Login failed");
            }
        } catch (err) {
            console.error(err);
            setError("Something went wrong!");
        }
    };

    return (
        <div
            style={{
                minHeight: "100vh",
                display: "flex",
                justifyContent: "center",
                alignItems: "center",
                background: "linear-gradient(135deg, #6a11cb 0%, #2575fc 100%)",
            }}
        >
            <Card
                sx={{
                    width: 400,
                    padding: 4,
                    borderRadius: 4,
                    boxShadow: 5,
                    background: "rgba(255, 255, 255, 0.9)",
                    backdropFilter: "blur(6px)",
                }}
            >
                <CardContent>
                    <Grid container spacing={2} justifyContent="center">
                        <Grid item xs={12} style={{ textAlign: "center" }}>
                            <img
                                src="/logohero.png"
                                alt="Logo"
                                style={{ width: "100px", marginBottom: 16 }}
                            />
                            <Typography variant="h5" fontWeight="bold" gutterBottom>
                                Welcome Back
                            </Typography>
                            <Typography variant="body2" color="text.secondary">
                                Please login to your account
                            </Typography>
                        </Grid>

                        <Grid item xs={12}>
                            <TextField
                                label="Email / Username"
                                variant="outlined"
                                fullWidth
                                value={emailOrUserName}
                                onChange={(e) => setEmailOrUserName(e.target.value)}
                                required
                               
                            />
                        </Grid>

                        <Grid item xs={12}>
                            <TextField
                                label="Password"
                                variant="outlined"
                                fullWidth
                                type={showPassword ? "text" : "password"}
                                value={password}
                                onChange={(e) => setPassword(e.target.value)}
                                required
                                InputProps={{
                                    endAdornment: (
                                        <InputAdornment position="end">
                                            <IconButton onClick={togglePasswordVisibility}>
                                                {showPassword ? <Visibility /> : <VisibilityOff />}
                                            </IconButton>
                                        </InputAdornment>
                                    ),
                                }}
                            />
                        </Grid>

                        <Grid
                            item
                            xs={12}
                            container
                            justifyContent="space-between"
                            alignItems="center"
                        >
                            <FormControlLabel
                                control={
                                    <Checkbox
                                        checked={rememberMe}
                                        onChange={(e) => setRememberMe(e.target.checked)}
                                    />
                                }
                                label="Remember me"
                            />
                            <Button href="/forgot-password" size="small">
                                Forgot password?
                            </Button>
                        </Grid>

                        {error && (
                            <Grid item xs={12}>
                                <Typography color="error" align="center">
                                    {error}
                                </Typography>
                            </Grid>
                        )}

                        <Grid item xs={12}>
                            <Button
                                type="submit"
                                variant="contained"
                                color="primary"
                                fullWidth
                                size="large"
                                onClick={handleSubmit}
                                sx={{
                                    textTransform: "none",
                                    fontSize: "16px",
                                    borderRadius: "8px",
                                }}
                            >
                                Sign In
                            </Button>
                        </Grid>
                    </Grid>
                </CardContent>
            </Card>
        </div>
    );
}
