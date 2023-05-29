import {
    Box,
    Button,
    FormControl,
    IconButton,
    TextField,
    Typography,
} from "@mui/material";
import { useState } from "react";
import { Add, Remove } from "@mui/icons-material";
import { green } from "@mui/material/colors";
import { Report } from "../../reports";
import { PDFDocument } from "pdf-lib";

interface PurchaseProps {
    total: number;
    onSubmit: () => void;
}

const PurchaseForm = ({ total, onSubmit }: PurchaseProps) => {
    const [quantity, setQuantity] = useState(1);
    const [showReport, setShowReport] = useState(false);
    const [purchasedItem] = useState("");

    const handleIncreaseQuantity = () => {
        setQuantity((prevQuantity) => prevQuantity + 1);
    };

    const handleDecreaseQuantity = () => {
        if (quantity > 1) {
            setQuantity((prevQuantity) => prevQuantity - 1);
        }
    };

    const handleQuantityChange = (
        event: React.ChangeEvent<HTMLInputElement>
    ) => {
        const newQuantity = parseInt(event.target.value);
        setQuantity(newQuantity || 0);
    };

    const calculateCost = (price: number, quantity: number) => {
        return price * quantity;
    };

    const handleSubmit = (event: React.FormEvent) => {
        event.preventDefault();
        onSubmit();
        setShowReport(true);
    };

    const handleDownloadPDF = async () => {
        const pdfDoc = await generatePDF();

        const pdfBytes = await pdfDoc.save();
        const blob = new Blob([pdfBytes], { type: "application/pdf" });
        const url = URL.createObjectURL(blob);

        const link = document.createElement("a");
        link.href = url;
        link.download = "invoice.pdf";
        link.click();

        URL.revokeObjectURL(url);
    };

    const generatePDF = async () => {
        const pdfDoc = await PDFDocument.create();
        const page = pdfDoc.addPage();
        page.drawText(`Purchased Item: ${purchasedItem}`, { x: 50, y: 50 });

        return pdfDoc;
    };

    return (
        <Box
            sx={{
                p: 2,
                display: "flex",
                flexDirection: "column",
                alignItems: "center",
                justifyContent: "center",
                backgroundColor: "#132f4b",
                maxWidth: "400px",
                margin: "0 auto",
                borderRadius: 3,
            }}>
            <Typography variant="h6" align="center" color="white">
                Purchase Form
            </Typography>
            <FormControl component="form" onSubmit={handleSubmit}>
                <Box
                    sx={{
                        display: "flex",
                        alignItems: "center",
                        justifyContent: "space-between",
                    }}>
                    <IconButton onClick={handleDecreaseQuantity}>
                        <Remove />
                    </IconButton>
                    <TextField
                        id="quantity"
                        variant="standard"
                        type="text"
                        value={quantity}
                        onChange={handleQuantityChange}
                        InputProps={{
                            style: {
                                color: "white",
                            },
                        }}
                        sx={{
                            width: "70px",
                            textAlign: "center",
                            "& .MuiInputBase-root": {
                                color: "white",
                                "&:before": {
                                    borderBottomColor: "white",
                                },
                                "&:hover:before": {
                                    borderBottomColor: "white",
                                },
                                "&.Mui-focused:before": {
                                    borderBottomColor: "green",
                                },
                                "&.Mui-focused:after": {
                                    borderBottomColor: "green",
                                },
                            },
                        }}
                    />
                    <IconButton onClick={handleIncreaseQuantity}>
                        <Add />
                    </IconButton>
                </Box>
                <Typography color="white">
                    Total price: {calculateCost(total, quantity)}
                </Typography>
                <Button
                    variant="contained"
                    color="primary"
                    type="submit"
                    onClick={handleSubmit}
                    sx={{
                        backgroundColor: "inherit",
                        "&:hover": {
                            backgroundColor: green[500],
                            color: "white",
                        },
                    }}>
                    Submit
                </Button>
                {showReport && <Report onDownloadPDF={handleDownloadPDF} />}
            </FormControl>
        </Box>
    );
};

export default PurchaseForm;

