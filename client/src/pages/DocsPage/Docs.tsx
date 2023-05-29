import { Box, Button, Typography } from "@mui/material";
import { DocumentsList } from "../../components/documents";
import { useEffect, useState } from "react";
import { getDocuments } from "../../lib/products/products";
import { getCookie } from "typescript-cookie";
import { DateField, LocalizationProvider } from "@mui/x-date-pickers";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import { green } from "@mui/material/colors";
import { IDocument } from "../../shared/models/docs";

const debugDocuments = (count: number): IDocument[] => {
    return new Array(count).fill(null).map((_, i) => {
        return {
            link: `http://localhost:3000/document?id=${i}`,
            filename: `File${i}`,
            file: new File([], `f${i}`)
        };
    });
};

const DocsPage = () => {
    // const location = useLocation();

    const [documents, setDocuments] = useState<IDocument[]>(
        debugDocuments(5)
    );
    const [beginDate, setBeginDate] = useState<Date | null>();
    const [endDate, setEndDate] = useState<Date | null>();

    const fetchDocuments = async (dateFrom?: Date, dateTo?: Date) => {
        try {
            const fetchedDocuments = await getDocuments(
                getCookie("jwt-authorization") ?? ""
            );
            setDocuments(fetchedDocuments);
        } catch (error) {
            console.log(error);
        }
    };

    useEffect(() => {
        fetchDocuments();
    });

    const handleClick = () => {
        fetchDocuments();
    };

    return (
        <Box
            bgcolor="#132f4b"
            display={"flex"}
            alignItems={"center"}
            height={"100vh"}
            width={"100vw"}>
            <Box
                display={"flex"}
                flexDirection={"column"}
                justifyContent={"space-evenly"}
                width={"80%"}
                marginLeft={"auto"}
                marginRight={"auto"}>
                <Box
                    display={"flex"}
                    justifyContent={"space-evenly"}
                    width={"100%"}
                    padding={"10px"}>
                    <LocalizationProvider dateAdapter={AdapterDayjs}>
                        <Box bgcolor={"#132f4b"}>
                            <DateField
                                sx={{
                                    svg: { color: "white" },
                                    input: { color: "white" },
                                    label: { color: "white" },
                                }}
                                label={<Typography>Начало</Typography>}
                                value={beginDate}
                                onChange={(newValue) => setBeginDate(newValue)}
                            />
                        </Box>
                        <Box bgcolor={"#132f4b"}>
                            <DateField
                                sx={{
                                    svg: { color: "white" },
                                    input: { color: "white" },
                                    label: { color: "white" },
                                }}
                                label={<Typography>Конец</Typography>}
                                value={endDate}
                                onChange={(newValue) => setEndDate(newValue)}
                            />
                        </Box>
                    </LocalizationProvider>

                    <Button
                        sx={{
                            "&:hover": {
                                backgroundColor: green[500],
                                color: "white",
                            },
                        }}
                        onClick={handleClick}>
                        Получить
                    </Button>
                </Box>
                <DocumentsList documents={documents} />
            </Box>
        </Box>
    );
};

export default DocsPage;
