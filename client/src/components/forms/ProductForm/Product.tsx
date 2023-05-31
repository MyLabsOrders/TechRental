import {
    Box,
    Button,
    CardMedia,
    Dialog,
    Divider,
    Stack,
    Typography,
} from "@mui/material";
import { green, grey } from "@mui/material/colors";
import { PurchaseForm } from "../PurchaseForm";
import { useState } from "react";
import { addProduct } from "../../../lib/users/users";
import { getCookie, setCookie } from "typescript-cookie";
import { Notification } from "../../../features";
import { useNavigate } from "react-router-dom";

export interface IProduct {
    id: string;
    name: string;
    total: number;
    status: string;
    image: string;
}


const Product = ({ id, name, total, status, image }: IProduct) => {
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [message, setMessage] = useState<string | null>(null);
    const navigate = useNavigate();

    const handleClick = () => {
        setIsModalOpen(true);
    };

    const handleCloseModal = async () => {
        setIsModalOpen(false);
    };

    const handleSubmit = async (count: number) => {
        try {
            setMessage(null);
            const {data} = await addProduct(
                getCookie("jwt-authorization") ?? "",
                getCookie("current-user") ?? "",
                {orderId: id, count }
            );
            setCookie('order-date', data);
            setMessage("Successfully bought it!");
        } catch (error: any) {
            setMessage(error?.response?.data?.Detailes);
            navigate("/profile");
        }
    };

    return (
        <>
            {message && message.includes("bought") && (
                <Notification message={message} type={"success"} />
            )}
            {message && !message.includes("bought") && (
                <Notification message={message} type={"warning"} />
            )}
            <Box
                sx={{
                    p: 2,
                    display: "flex",
                    alignItems: "center",
                    backgroundColor: "#0a1929",
                }}>
                <CardMedia component="img" src={image} alt={name} />
            </Box>
            <Divider sx={{ backgroundColor: "white" }} />
            <Box
                sx={{
                    p: 2,
                    display: "flex",
                    justifyContent: "space-between",
                    flexDirection: "column",
                }}>
                <Stack
                    maxWidth={"inherit"}
                    padding={0}
                    spacing={0.5}
                    sx={{ flex: 1, marginLeft: "10px" }}>
                    <Typography variant="h5" color={grey[600]}>
                        {name}
                    </Typography>
                    <Typography variant="body1" color={grey[600]}>
                        Total: {total}
                    </Typography>
                    <Typography variant="body1" color={grey[600]}>
                        Status: {status}
                    </Typography>
                </Stack>
                <div
                    style={{
                        display: "flex",
                        justifyContent: "flex-end",
                        alignItems: "flex-end",
                    }}>
                    <Button
                        sx={{
                            "&:hover": {
                                backgroundColor: green[500],
                                color: "white",
                            },
                        }}
                        onClick={handleClick}>
                        Purchase
                    </Button>
                </div>
            </Box>
            <Dialog
                open={isModalOpen}
                onClose={handleCloseModal}
                sx={{
                    backdropFilter: "blur(5px)",
                    "& .MuiPaper-root": {
                        borderRadius: 3,
                        bgcolor: "#132f4b",
                    }
                }}>
                <PurchaseForm total={total} onSubmit={handleSubmit} />
            </Dialog>
        </>
    );
};
export default Product;

