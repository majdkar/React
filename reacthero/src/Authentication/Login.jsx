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
    Box,
    Fade,
} from "@mui/material";
import { Visibility, VisibilityOff, Login as LoginIcon } from "@mui/icons-material";
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
        <Box
            sx={{
                minHeight: "100vh",
                display: "flex",
                alignItems: "center",
                justifyContent: "center",
                background: "linear-gradient(135deg, #1e3c72 0%, #2a5298 100%)",
                p: 2,
            }}
        >
            <Fade in timeout={700}>
                <Card
                    elevation={10}
                    sx={{
                        width: 400,
                        borderRadius: 4,
                        backgroundColor: "rgba(255,255,255,0.95)",
                        backdropFilter: "blur(10px)",
                        boxShadow: "0 8px 25px rgba(0,0,0,0.2)",
                        p: 3,
                    }}
                >
                    <CardContent>
                        <Box textAlign="center" mb={3}>
                            <img
                                src="/logohero.png"
                                alt="Logo"
                                style={{ width: "90px", marginBottom: 10 }}
                            />
                            <Typography variant="h5" fontWeight="bold" color="primary" gutterBottom>
                                Welcome Back
                            </Typography>
                            <Typography variant="body2" color="text.secondary">
                                Please sign in to continue
                            </Typography>
                        </Box>

                        <form onSubmit={handleSubmit}>
                            <Grid container spacing={2}>
                                <Grid item xs={12}>
                                    <TextField
                                        label="Email or Username"
                                        variant="outlined"
                                        fullWidth
                                        required
                                        value={emailOrUserName}
                                        onChange={(e) => setEmailOrUserName(e.target.value)}
                                        InputProps={{
                                            style: { borderRadius: 10, background: "#fff" },
                                        }}
                                    />
                                </Grid>

                                <Grid item xs={12}>
                                    <TextField
                                        label="Password"
                                        variant="outlined"
                                        fullWidth
                                        required
                                        type={showPassword ? "text" : "password"}
                                        value={password}
                                        onChange={(e) => setPassword(e.target.value)}
                                        InputProps={{
                                            endAdornment: (
                                                <InputAdornment position="end">
                                                    <IconButton onClick={togglePasswordVisibility}>
                                                        {showPassword ? <Visibility /> : <VisibilityOff />}
                                                    </IconButton>
                                                </InputAdornment>
                                            ),
                                            style: { borderRadius: 10, background: "#fff" },
                                        }}
                                    />
                                </Grid>

                                <Grid
                                    item
                                    xs={12}
                                    display="flex"
                                    alignItems="center"
                                    justifyContent="space-between"
                                >
                                    <FormControlLabel
                                        control={
                                            <Checkbox
                                                checked={rememberMe}
                                                onChange={(e) => setRememberMe(e.target.checked)}
                                                color="primary"
                                            />
                                        }
                                        label="Remember me"
                                    />
                                    <Button
                                        href="/forgot-password"
                                        size="small"
                                        sx={{ textTransform: "none" }}
                                    >
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
                                        fullWidth
                                        size="large"
                                        endIcon={<LoginIcon />}
                                        sx={{
                                            mt: 1,
                                            textTransform: "none",
                                            fontSize: "16px",
                                            borderRadius: "10px",
                                            py: 1.3,
                                            background:
                                                "linear-gradient(90deg, #1e88e5 0%, #42a5f5 100%)",
                                            "&:hover": {
                                                background:
                                                    "linear-gradient(90deg, #1976d2 0%, #1e88e5 100%)",
                                            },
                                        }}
                                    >
                                        Sign In
                                    </Button>
                                </Grid>
                            </Grid>
                        </form>
                    </CardContent>
                </Card>
            </Fade>
        </Box>
    );
}
