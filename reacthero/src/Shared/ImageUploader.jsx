import React, { useState } from "react";
import { Box, Button, Typography } from "@mui/material";

const ImageUploader = ({
    label = "Image",
    value,
    onChange,
    apiBaseUrl,
    token,
    fileLocation = 1,
    uploadType = 1,
    t = (key) => key, // ترجمة اختيارية
}) => {
    const [loading, setLoading] = useState(false);

    const handleUpload = async (e) => {
        const file = e.target.files[0];
        if (!file) return;

        const formDataUpload = new FormData();
        formDataUpload.append("file", file);

        setLoading(true);
        try {
            const response = await fetch(
                `${apiBaseUrl}api/FileUpload/${fileLocation}/${uploadType}`,
                {
                    method: "POST",
                    headers: { Authorization: `Bearer ${token}` },
                    body: formDataUpload,
                }
            );

            if (!response.ok) throw new Error(await response.text());
            const result = await response.text();
            const cleaned = result.replace(/"/g, "");

            // تمرير النتيجة للأب
            onChange(cleaned);
        } catch (err) {
            console.error("Upload failed:", err);
            alert(t("uploadFailed") || "Upload failed ❌");
        } finally {
            setLoading(false);
        }
    };

    const handleDelete = () => {
        onChange("");
    };

    const getImageUrl = () => {
        if (!value) return null;
        return value.startsWith("http")
            ? `${apiBaseUrl}Files/UploadFiles/BlocksFiles/${value}`
            : `${apiBaseUrl}Files/UploadFiles/BlocksFiles/${value}`;
    };

    return (
        <Box>
            <Typography variant="subtitle1">{label}</Typography>

            <Box
                sx={{
                    mt: 1,
                    width: 180,
                    border: "1px dashed #ccc",
                    borderRadius: 2,
                    padding: 1.5,
                    display: "flex",
                    flexDirection: "column",
                    alignItems: "center",
                    justifyContent: "center",
                    backgroundColor: "#fafafa",
                }}
            >
                {/* معاينة الصورة */}
                <Box
                    sx={{
                        width: 150,
                        height: 150,
                        borderRadius: 2,
                        overflow: "hidden",
                        display: "flex",
                        alignItems: "center",
                        justifyContent: "center",
                        mb: 1.5,
                    }}
                >
                    {value ? (
                        <img
                            src={getImageUrl()}
                            alt="Preview"
                            style={{
                                width: "100%",
                                height: "100%",
                                objectFit: "cover",
                                borderRadius: "8px",
                            }}
                        />
                    ) : (
                        <Typography variant="body2" color="textSecondary">
                            {t("noImage") || "No image"}
                        </Typography>
                    )}
                </Box>

                {/* الأزرار تحت الصورة */}
                <Box
                    sx={{
                        display: "flex",
                        gap: 1,
                        justifyContent: "center",
                        flexWrap: "wrap",
                        width: "100%",
                    }}
                >
                    <Button
                        variant="contained"
                        component="label"
                        size="small"
                        disabled={loading}
                        sx={{ textTransform: "none" }}
                    >
                        {t("uploadImage") || "Upload"}
                        <input
                            type="file"
                            accept="image/*"
                            hidden
                            onChange={handleUpload}
                        />
                    </Button>

                    {value && (
                        <>
                            <Button
                                variant="outlined"
                                size="small"
                                onClick={() => window.open(getImageUrl(), "_blank")}
                            >
                                {t("view") || "View"}
                            </Button>

                            <Button
                                variant="outlined"
                                color="error"
                                size="small"
                                onClick={handleDelete}
                            >
                                {t("delete") || "Delete"}
                            </Button>
                        </>
                    )}
                </Box>
            </Box>
        </Box>
    );
};

export default ImageUploader;
