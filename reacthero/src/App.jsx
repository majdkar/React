// App.jsx
import React from "react";
import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";
import { createTheme, ThemeProvider } from "@mui/material/styles";
import CssBaseline from "@mui/material/CssBaseline";
import i18n from './i18n';
import Login from "./Authentication/Login";
import MainLayout from "./Shared/MainLayout";

const theme = createTheme({
  typography: {
    fontFamily: `'Tajawal', system-ui, Avenir, Helvetica, Arial, sans-serif`,
  },
});

const isAuthenticated = () => !!localStorage.getItem("token");

export default function App() {
  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <Router>
        <Routes>
          <Route path="/login" element={<LoginWrapper />} />
          <Route
            path="/*"
            element={
              isAuthenticated() ? <MainLayout /> : <Navigate to="/login" replace />
            }
          />
        </Routes>
      </Router>
    </ThemeProvider>
  );
}

function LoginWrapper() {
  const token = localStorage.getItem("token");
  if (token) {
    return <Navigate to="/" replace />;
  }
  return <Login />;
}
