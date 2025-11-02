import React, { useState, useEffect } from "react";
import {
    AppBar,
    Toolbar,
    IconButton,
    Typography,
    Drawer,
    Box,
    List,
    ListItemButton,
    ListItemIcon,
    ListItemText,
    Avatar,
    Menu,
    MenuItem,
    Collapse,
    CssBaseline,
    useMediaQuery,
    Tooltip,
} from "@mui/material";
import MenuIcon from "@mui/icons-material/Menu";
import TranslateIcon from "@mui/icons-material/Translate";
import ExpandLess from "@mui/icons-material/ExpandLess";
import ExpandMore from "@mui/icons-material/ExpandMore";
import SettingsIcon from "@mui/icons-material/Settings";
import HomeIcon from "@mui/icons-material/Home";
import PublicIcon from '@mui/icons-material/Public';
import LocationCityIcon from '@mui/icons-material/LocationCity';
import DescriptionIcon from '@mui/icons-material/Description';
import ViewModuleIcon from '@mui/icons-material/ViewModule';
import MenuBookIcon from '@mui/icons-material/MenuBook';
import LanguageIcon from '@mui/icons-material/Language';
import LogoutIcon from "@mui/icons-material/Logout";
import { useTheme } from "@mui/material/styles";
import { useTranslation } from "react-i18next";
import { Link, Routes, Route } from "react-router-dom";
import CountriesList from "../GeneralSettings/Countries/CountriesList";
import CitiesList from "../GeneralSettings/Cities/CitiesList";
import BlockCatgories from "../Blocks/BlockCatgories";
import MenuCatgories from "../Menus/MenuCategories";
import Blocks from "../Blocks/Blocks";
import AddBlockPage from "../Blocks/AddBlockPage";
import BlockPhotos from "../Blocks/BlockPhotos";
import BlockVideos from "../Blocks/BlockVideos";
import { API_BASE_URL } from "../../config";
import Logout from "./Logout";
const drawerWidth = 250;
const miniDrawerWidth = 60;

export default function NavbarWithMiniDrawer() {
    const { t, i18n } = useTranslation();
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down("md"));
    
    const [drawerOpen, setDrawerOpen] = useState(!isMobile);
    const [drawerMini, setDrawerMini] = useState(false);
    const [anchorEl, setAnchorEl] = useState(null);
    const [settingsOpen, setSettingsOpen] = useState(false);
    const [websitemanagmentsOpen, setwebsitemanagmentsOpen] = useState(false);
    const [BlocksOpen, setBlocksOpen] = useState(false);
    const [BlocksListOpen, setBlocksListOpen] = useState(false);
    const [MenusOpen, setMenusOpen] = useState(false);
    const [MenusListOpen, setMenusListOpen] = useState(false);

    const handleLangMenu = (e) => setAnchorEl(e.currentTarget);
    const handleCloseMenu = () => setAnchorEl(null);
    const handleChangeLanguage = (lang) => {
        i18n.changeLanguage(lang);
        document.dir = lang === "ar" ? "rtl" : "ltr";
        setAnchorEl(null);
    };

    const isArabic = i18n.language === "ar";


    const [blockCategories, setBlockCategories] = useState([]);
    const [menuCategories, setMenuCategories] = useState([]);
    const token = localStorage.getItem("token");

    useEffect(() => {
        const fetchBlockCategories = async () => {
            try {
                const res = await fetch(`${API_BASE_URL}api/BlockCategories/all`, {
                    headers: { Authorization: `Bearer ${token}` }
                });
                if (!res.ok) throw new Error("Failed to fetch block categories");
                const data = await res.json();
                setBlockCategories(data); // أو data.data حسب الاستجابة
            } catch (err) {
                console.error(err);
            }
        };
        fetchBlockCategories();
    }, [token]);


    useEffect(() => {
        const fetchMenuCategories = async () => {
            try {
                const res = await fetch(`${API_BASE_URL}api/v1/MenuCategories/all`, {
                    headers: { Authorization: `Bearer ${token}` }
                });
                if (!res.ok) throw new Error("Failed to fetch Menu categories");
                const data = await res.json();
                setMenuCategories(data); // أو data.data حسب الاستجابة
            } catch (err) {
                console.error(err);
            }
        };
        fetchMenuCategories();
    }, [token]);







    return (
        <Box sx={{ display: "flex", direction: isArabic ? "rtl" : "ltr" }}>
            <CssBaseline />

            {/* ===== AppBar ===== */}
            <AppBar
                position="fixed"
                sx={{
                    backgroundColor: "#1976d2",
                    width: drawerOpen && !isMobile
                        ? `calc(100% - ${drawerMini ? miniDrawerWidth : drawerWidth}px)`
                        : "100%",
                    transition: "all 0.3s ease",
                    ...(drawerOpen && !isArabic && !isMobile && { ml: `${drawerMini ? miniDrawerWidth : drawerWidth}px` }),
                    ...(drawerOpen && isArabic && !isMobile && { mr: `${drawerMini ? miniDrawerWidth : drawerWidth}px` }),
                }}
            >
                <Toolbar sx={{ display: "flex", justifyContent: "space-between" }}>
                    <Box sx={{ display: "flex", alignItems: "center", flexDirection: isArabic ? "row-reverse" : "row" }}>
                        {/*<IconButton*/}
                        {/*    color="inherit"*/}
                        {/*    onClick={() => isMobile ? setDrawerOpen(!drawerOpen) : setDrawerMini(!drawerMini)}*/}
                        {/*    sx={{ mx: 1 }}*/}
                        {/*>*/}
                        {/*    <MenuIcon />*/}
                        {/*</IconButton>*/}

                        <Box sx={{ display: "flex", alignItems: "center" }}>
                            <img
                                src="/logoWhite.png"
                                alt="Logo"
                                style={{
                                    width: 170,
                                    height: 35,
                                    marginInline: isArabic ? "0 10px" : "10px 0",
                                }}
                            />
                         {/*   <Typography variant="h6">Orbit</Typography>*/}
                        </Box>
                    </Box>

                    {/* اللغة + المستخدم */}
                    <Box sx={{ display: "flex", alignItems: "center", flexDirection: isArabic ? "row-reverse" : "row", gap: 1 }}>
                        <IconButton
                            color="inherit"
                            onClick={handleLangMenu}
                        >
                            <TranslateIcon />
                        </IconButton>

                        <Menu
                            anchorEl={anchorEl}
                            open={Boolean(anchorEl)}
                            onClose={handleCloseMenu}
                            anchorOrigin={{ vertical: "bottom", horizontal: isArabic ? "left" : "right" }}
                            transformOrigin={{ vertical: "top", horizontal: isArabic ? "left" : "right" }}
                        >
                            <MenuItem onClick={() => handleChangeLanguage("ar")}>🇸🇦 {t("arabic")}</MenuItem>
                            <MenuItem onClick={() => handleChangeLanguage("en")}>🇺🇸 {t("english")}</MenuItem>
                        </Menu>

                        <Avatar
                            alt="User"
                            src="https://randomuser.me/api/portraits/men/32.jpg"
                            sx={{ cursor: "pointer" }}
                        />
                    </Box>
                </Toolbar>
            </AppBar>

            {/* ===== Drawer ===== */}
            <Drawer
                variant={isMobile ? "temporary" : "persistent"}
                anchor={isArabic ? "right" : "left"}
                open={drawerOpen}
                onClose={() => setDrawerOpen(false)}
                sx={{
                    width: drawerMini ? miniDrawerWidth : drawerWidth,
                    flexShrink: 0,
                    "& .MuiDrawer-paper": {
                        width: drawerMini ? miniDrawerWidth : drawerWidth,
                        overflowX: "hidden",
                        transition: "all 0.3s ease",
                        boxSizing: "border-box",
                    },
                }}
            >
                {/* زر الفتح/الإغلاق داخل الـ Drawer */}
                <Toolbar sx={{ display: "flex", justifyContent: drawerMini ? "center" : "flex-end" }}>
                    <IconButton
                        onClick={() => isMobile ? setDrawerOpen(false) : setDrawerMini(!drawerMini)}
                        size="small"
                    >
                        <MenuIcon />
                    </IconButton>
                </Toolbar>

                <Box sx={{ textAlign: isArabic ? "right" : "left" }}>
                    <List>
                        <Tooltip title={t("home")} placement="right">
                            <ListItemButton
                                component={Link}
                                to="/"
                                onClick={() => isMobile && setDrawerOpen(false)}
                                sx={{ justifyContent: drawerMini ? "center" : "flex-start" }}
                            >
                                <ListItemIcon sx={{ minWidth: 0, justifyContent: "center", color: "#1976d2" }}><HomeIcon /></ListItemIcon>
                                {!drawerMini && <ListItemText primary={t("home")} sx={{ ml: 2 }} />}
                            </ListItemButton>
                        </Tooltip>

                        {/* ===== Settings ===== */}
                        {drawerMini ? (
                            <Tooltip title={t("settings")} placement="right">
                                <ListItemButton
                                    onClick={() => setSettingsOpen(!settingsOpen)}
                                    sx={{ justifyContent: "center" }}
                                >
                                    <ListItemIcon sx={{ minWidth: 0, justifyContent: "center", color: "#1976d2" }}>
                                        <SettingsIcon />
                                    </ListItemIcon>
                                </ListItemButton>
                            </Tooltip>
                        ) : (
                            <ListItemButton
                                onClick={() => setSettingsOpen(!settingsOpen)}
                                sx={{ justifyContent: "flex-start" }}
                            >
                                    <ListItemIcon sx={{ minWidth: 0, justifyContent: "center", color: "#1976d2" }}><SettingsIcon /></ListItemIcon>
                                <ListItemText primary={t("settings")} sx={{ ml: 2 }} />
                                {settingsOpen ? <ExpandLess /> : <ExpandMore />}
                            </ListItemButton>
                        )}

                        <Collapse in={settingsOpen} timeout="auto" unmountOnExit>
                            <List component="div" disablePadding>

                                <Tooltip title={t("countries")} placement="right">
                                    <ListItemButton
                                        component={Link}
                                        to="/countries"
                                        onClick={() => isMobile && setDrawerOpen(false)}
                                        sx={{ justifyContent: drawerMini ? "center" : "flex-start" }}
                                    >
                                        <ListItemIcon sx={{ minWidth: 0, justifyContent: "center", color: "#1976d2" }}><PublicIcon /></ListItemIcon>
                                        {!drawerMini && <ListItemText primary={t("countries")} sx={{ ml: 2 }} />}
                                    </ListItemButton>
                                </Tooltip>

                                <Tooltip title={t("cities")} placement="right">
                                    <ListItemButton
                                        component={Link}
                                        to="/cities"
                                        onClick={() => isMobile && setDrawerOpen(false)}
                                        sx={{ justifyContent: drawerMini ? "center" : "flex-start" }}
                                    >
                                        <ListItemIcon sx={{ minWidth: 0, justifyContent: "center", color: "#1976d2" }}><LocationCityIcon /></ListItemIcon>
                                        {!drawerMini && <ListItemText primary={t("cities")} sx={{ ml: 2 }} />}
                                    </ListItemButton>
                                </Tooltip>


                            </List>
                        </Collapse>




                        {/* ===== WebsiteManagment ===== */}
                        {drawerMini ? (
                            <Tooltip title={t("WebsiteManagment")} placement="right">
                                <ListItemButton
                                    onClick={() => setwebsitemanagmentsOpen(!websitemanagmentsOpen)}
                                    sx={{ justifyContent: "center" }}
                                >
                                    <ListItemIcon sx={{ minWidth: 0, justifyContent: "center", color: "#1976d2" }}>
                                        <SettingsIcon />
                                    </ListItemIcon>
                                </ListItemButton>
                            </Tooltip>
                        ) : (
                            <ListItemButton
                                    onClick={() => setwebsitemanagmentsOpen(!websitemanagmentsOpen)}
                                sx={{ justifyContent: "flex-start" }}
                            >
                                    <ListItemIcon sx={{ minWidth: 0, justifyContent: "center", color: "#1976d2" }}><LanguageIcon /></ListItemIcon>
                                    <ListItemText primary={t("WebsiteManagment")} sx={{ ml: 2 }} />
                                    {websitemanagmentsOpen ? <ExpandLess /> : <ExpandMore />}
                            </ListItemButton>
                        )}





                        <Collapse in={websitemanagmentsOpen} timeout="auto" unmountOnExit>
                            <List component="div" disablePadding>

                                {drawerMini ? (
                                    <Tooltip title={t("blocksManagment")} placement="right">
                                        <ListItemButton
                                            onClick={() => setBlocksOpen(!BlocksOpen)}
                                            sx={{ justifyContent: "center" }}
                                        >
                                            <ListItemIcon sx={{ minWidth: 0, justifyContent: "center", color: "#1976d2" }}>
                                                <SettingsIcon />
                                            </ListItemIcon>
                                        </ListItemButton>
                                    </Tooltip>
                                ) : (
                                    <ListItemButton
                                            onClick={() => setBlocksOpen(!BlocksOpen)}
                                        sx={{ justifyContent: "flex-start" }}
                                    >
                                        <ListItemIcon sx={{ minWidth: 0, justifyContent: "center", color: "#1976d2" }}><LanguageIcon /></ListItemIcon>
                                            <ListItemText primary={t("blocksManagment")} sx={{ ml: 2 }} />
                                        {BlocksOpen ? <ExpandLess /> : <ExpandMore />}
                                    </ListItemButton>
                                )}


                                <Collapse in={BlocksOpen} timeout="auto" unmountOnExit>
                                    <List component="div" disablePadding>

                                        <Tooltip title={t("blockCategories")} placement="right">
                                            <ListItemButton
                                                component={Link}
                                                to="/BlockCategories"
                                                onClick={() => isMobile && setDrawerOpen(false)}
                                                sx={{ justifyContent: drawerMini ? "center" : "flex-start" }}
                                            >
                                                <ListItemIcon sx={{ minWidth: 0, justifyContent: "center", color: "#1976d2" }}><MenuBookIcon /></ListItemIcon>
                                                {!drawerMini && <ListItemText primary={t("blockCategories")} sx={{ ml: 2 }} />}
                                            </ListItemButton>
                                        </Tooltip>

                                 


                                        {drawerMini ? (
                                            <Tooltip title={t("blocksList")} placement="right">
                                                <ListItemButton
                                                    onClick={() => setBlocksListOpen(!BlocksListOpen)}
                                                    sx={{ justifyContent: "center" }}
                                                >
                                                    <ListItemIcon sx={{ minWidth: 0, justifyContent: "center", color: "#1976d2" }}>
                                                        <SettingsIcon />
                                                    </ListItemIcon>
                                                </ListItemButton>
                                            </Tooltip>
                                        ) : (
                                            <ListItemButton
                                                onClick={() => setBlocksListOpen(!BlocksListOpen)}
                                                sx={{ justifyContent: "flex-start" }}
                                            >
                                                <ListItemIcon sx={{ minWidth: 0, justifyContent: "center", color: "#1976d2" }}><LanguageIcon /></ListItemIcon>
                                                    <ListItemText primary={t("blocksList")} sx={{ ml: 2 }} />
                                                {BlocksListOpen ? <ExpandLess /> : <ExpandMore />}
                                            </ListItemButton>
                                        )}

                                        <Collapse in={BlocksListOpen} timeout="auto" unmountOnExit>
                                            <List component="div" disablePadding>
                                                {blockCategories.map((cat) => (
                                                    <Tooltip key={cat.id} title={cat.nameEn} placement="right">
                                                        <ListItemButton
                                                            component={Link}
                                                            to={`/Blocks/${cat.id}/${encodeURIComponent(`${cat.nameEn + ' / '+ cat.nameAr}`)}`}
                                                            onClick={() => isMobile && setDrawerOpen(false)}
                                                            sx={{ justifyContent: drawerMini ? "center" : "flex-start" }}
                                                        >
                                                            <ListItemIcon sx={{ minWidth: 0, justifyContent: "center", color: "#1976d2" }}><MenuBookIcon /></ListItemIcon>
                                                            {!drawerMini && <ListItemText primary={cat.nameEn} sx={{ ml: 2 }} />}
                                                        </ListItemButton>
                                                    </Tooltip>
                                                ))}
                                            </List>
                                        </Collapse>




                                    </List>
                                </Collapse>












                                {drawerMini ? (
                                    <Tooltip title={t("menusManagment")} placement="right">
                                        <ListItemButton
                                            onClick={() => setMenusOpen(!MenusOpen)}
                                            sx={{ justifyContent: "center" }}
                                        >
                                            <ListItemIcon sx={{ minWidth: 0, justifyContent: "center", color: "#1976d2" }}>
                                                <SettingsIcon />
                                            </ListItemIcon>
                                        </ListItemButton>
                                    </Tooltip>
                                ) : (
                                    <ListItemButton
                                            onClick={() => setMenusOpen(!MenusOpen)}
                                        sx={{ justifyContent: "flex-start" }}
                                    >
                                        <ListItemIcon sx={{ minWidth: 0, justifyContent: "center", color: "#1976d2" }}><LanguageIcon /></ListItemIcon>
                                            <ListItemText primary={t("menusManagment")} sx={{ ml: 2 }} />
                                            {MenusOpen ? <ExpandLess /> : <ExpandMore />}
                                    </ListItemButton>
                                )}



                                <Collapse in={MenusOpen} timeout="auto" unmountOnExit>
                                    <List component="div" disablePadding>

                                        <Tooltip title={t("menuCategories")} placement="right">
                                            <ListItemButton
                                                component={Link}
                                                to="/MenuCategories"
                                                onClick={() => isMobile && setDrawerOpen(false)}
                                                sx={{ justifyContent: drawerMini ? "center" : "flex-start" }}
                                            >
                                                <ListItemIcon sx={{ minWidth: 0, justifyContent: "center", color: "#1976d2" }}><MenuBookIcon /></ListItemIcon>
                                                {!drawerMini && <ListItemText primary={t("menuCategories")} sx={{ ml: 2 }} />}
                                            </ListItemButton>
                                        </Tooltip>




                                        {drawerMini ? (
                                            <Tooltip title={t("MenusList")} placement="right">
                                                <ListItemButton
                                                    onClick={() => setMenusListOpen(!MenusListOpen)}
                                                    sx={{ justifyContent: "center" }}
                                                >
                                                    <ListItemIcon sx={{ minWidth: 0, justifyContent: "center", color: "#1976d2" }}>
                                                        <SettingsIcon />
                                                    </ListItemIcon>
                                                </ListItemButton>
                                            </Tooltip>
                                        ) : (
                                            <ListItemButton
                                                    onClick={() => setMenusListOpen(!MenusListOpen)}
                                                sx={{ justifyContent: "flex-start" }}
                                            >
                                                <ListItemIcon sx={{ minWidth: 0, justifyContent: "center", color: "#1976d2" }}><LanguageIcon /></ListItemIcon>
                                                <ListItemText primary={t("menusList")} sx={{ ml: 2 }} />
                                                    {MenusListOpen ? <ExpandLess /> : <ExpandMore />}
                                            </ListItemButton>
                                        )}

                                        <Collapse in={MenusListOpen} timeout="auto" unmountOnExit>
                                            <List component="div" disablePadding>
                                                {menuCategories.map((cat) => (
                                                    <Tooltip key={cat.id} title={cat.nameEn} placement="right">
                                                        <ListItemButton
                                                            component={Link}
                                                            to={`/Menue/${cat.id}/${encodeURIComponent(`${cat.nameEn + ' / ' + cat.nameAr}`)}`}
                                                            onClick={() => isMobile && setDrawerOpen(false)}
                                                            sx={{ justifyContent: drawerMini ? "center" : "flex-start" }}
                                                        >
                                                            <ListItemIcon sx={{ minWidth: 0, justifyContent: "center", color: "#1976d2" }}><MenuBookIcon /></ListItemIcon>
                                                            {!drawerMini && <ListItemText primary={cat.nameEn} sx={{ ml: 2 }} />}
                                                        </ListItemButton>
                                                    </Tooltip>
                                                ))}
                                            </List>
                                        </Collapse>




                                    </List>
                                </Collapse>

























                                <Tooltip title={t("Pages")} placement="right">
                                    <ListItemButton
                                        component={Link}
                                        to="/Pages"
                                        onClick={() => isMobile && setDrawerOpen(false)}
                                        sx={{ justifyContent: drawerMini ? "center" : "flex-start" }}
                                    >
                                        <ListItemIcon sx={{ minWidth: 0, justifyContent: "center", color: "#1976d2" }}><DescriptionIcon /></ListItemIcon>
                                        {!drawerMini && <ListItemText primary={t("Pages")} sx={{ ml: 2 }} />}
                                    </ListItemButton>
                                </Tooltip>


                            </List>
                        </Collapse>



                        <Tooltip title={t("logout")} placement="right">
                            <ListItemButton component={Link} to="/logout" sx={{ justifyContent: drawerMini ? "center" : "flex-start" }}>
                                <ListItemIcon sx={{ minWidth: 0, justifyContent: "center", color: "#1976d2" }}><LogoutIcon /></ListItemIcon>
                                {!drawerMini && <ListItemText primary={t("logout")} sx={{ ml: 2 }} />}
                            </ListItemButton>
                        </Tooltip>
                    </List>
                </Box>
            </Drawer>

            {/* ===== المحتوى ===== */}
            <Box
                component="main"
                sx={{
                    flexGrow: 1,
                    p: 3,
                    mt: 8,
                    transition: "margin 0.3s ease",
                    width: drawerOpen && !isMobile ? `calc(100% - ${drawerMini ? miniDrawerWidth : drawerWidth}px)` : "100%",
                    boxSizing: 'border-box',
                }}
            >





                <Routes>
                    <Route path="/" element={<Typography>{t("welcome")}</Typography>} />
                    <Route path="/countries" element={<CountriesList />} />
                    <Route path="/cities" element={<CitiesList />} />
                    <Route
                        path="/BlockCategories"
                        element={<BlockCatgories
                            blockCategories={blockCategories}
                            setBlockCategories={setBlockCategories}
                        />}
                    />
                    <Route
                        path="/MenuCategories"
                        element={<MenuCatgories
                            menuCategories={menuCategories}
                            setMenuCategories={setMenuCategories}
                        />}
                    />
                    <Route
                        path="/Blocks/:categoryId/:categoryName"
                        element={<Blocks />}
                    />

                    {/* إضافة Block جديد للفئة */}
                    <Route path="/blocks/:categoryId/add" element={<AddBlockPage />} />

                    {/* إضافة Block فرعي (داخل Block آخر) */}
                    <Route path="/blocks/:categoryId/:blockId/add" element={<AddBlockPage />} />

                    {/* تعديل Block */}
                    <Route path="/blocks/:categoryId/edit/:blockId" element={<AddBlockPage />} />

                    <Route path="/blocks/:categoryId/photos/:blockId/:blockName" element={<BlockPhotos />} />

                    <Route path="/blocks/:categoryId/videos/:blockId/:blockName" element={<BlockVideos />} />


                    <Route path="/settings/profile" element={<Typography>{t("profilePage")}</Typography>} />
                    <Route path="/settings/security" element={<Typography>{t("securityPage")}</Typography>} />
                    <Route path="/logout" element={<Logout />} />



                </Routes>
            </Box>
        </Box>
    );
}
