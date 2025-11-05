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


export function isTokenValid() {
    const token = localStorage.getItem("token");
    if (!token) return false;

    try {
        const payload = JSON.parse(atob(token.split(".")[1]));
        const currentTime = Date.now() / 1000; // بالثواني
        if (payload.exp && payload.exp < currentTime) {
            localStorage.removeItem("token");
            return false;
        }
        return true;
    } catch (error) {
        localStorage.removeItem("token");
        return false;
    }
}


const isAuthenticated = ()  => isTokenValid();;

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
