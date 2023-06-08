import { Button, Box, Modal, FormControl, TextField } from "@mui/material";
import { green } from "@mui/material/colors";
import { useState } from "react";
import { createProduct } from "../../../lib/products/products";
import { getCookie } from "typescript-cookie";
import { useNavigate } from "react-router";
import { convertImageToBase64 } from "../../utils/utils";

const CreateProductForm = () => {
    const [isModaOpen, setIsModalOpen] = useState(false);
    const [status, setStatus] = useState("");
    const [price, setPrice] = useState(0);
    const [name, setName] = useState("");
    const [company, setCompany] = useState("");
    const [image, setImage] = useState<File | null>(null);
    const navigate = useNavigate();

    const handleOpen = () => {
        setIsModalOpen(true);
    };

    const handleClose = () => {
        setIsModalOpen(false);
    };

    const handleImageChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (e.target.files && e.target.files.length > 0) {
            setImage(e.target.files[0]);
        }
    };

    const handleClick = async () => {
        setIsModalOpen(false);

        try {
            const convertedImage = await convertImageToBase64(image);
            await createProduct(getCookie("jwt-authorization") ?? "", {
                name,
				company,
                orderImage: convertedImage,
                status,
                price,
            });
        } catch (error: any) {
            navigate("/profile", {
                state: { message: `${error?.message}`, type: "error" },
            });
        }
    };

    return (
        <Box>
            <Button
                onClick={handleOpen}
                sx={{
                    "&:hover": {
                        backgroundColor: green[500],
                        color: "white",
                    },
                }}>
                Создать продукт
            </Button>
            <Modal
                open={isModaOpen}
                onClose={handleClose}
                sx={{ backdropFilter: "blur(5px)" }}>
                <Box
                    sx={{
                        position: "absolute",
                        top: "50%",
                        left: "50%",
                        transform: "translate(-50%, -50%)",
                        width: 400,
                        bgcolor: "background.paper",
                        boxShadow: 24,
                        p: 4,
                        display: "flex",
                        justifyContent: "center",
                        alignItems: "center",
                        borderRadius: "10px",
                    }}>
                    <FormControl>
                        <TextField
                            value={status}
                            onChange={(e) => setStatus(e.target.value)}
                            label="Статус ( Available/Rented ) "
                            fullWidth
                        />
                        <TextField
                            value={name}
                            onChange={(e) => setName(e.target.value)}
                            label="Имя"
                            fullWidth
                        />
                        <TextField
                            value={price}
                            onChange={(e) =>
                                setPrice(e.target.value as unknown as number)
                            }
                            label="Цена"
                            fullWidth
                            type="number"
                        />
						<TextField
                            value={company}
                            onChange={(e) =>
                                setCompany(e.target.value)
                            }
                            label="Производитель"
                            fullWidth
                        />
                        <input
                            type="file"
                            accept="image/*"
                            onChange={handleImageChange}
                        />
                        <Button
                            onClick={handleClick}
                            sx={{
                                "&:hover": {
                                    backgroundColor: green[500],
                                    color: "white",
                                },
                                backgroundColor: "#132f4b",
                            }}>
                            Create
                        </Button>
                    </FormControl>
                </Box>
            </Modal>
        </Box>
    );
};

export default CreateProductForm;

