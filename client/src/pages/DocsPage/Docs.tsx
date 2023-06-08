import { Box, Button, Typography } from "@mui/material";
import { DocumentsList } from "../../components/documents";
import { useCallback, useEffect, useState } from "react";
import { getStats } from "../../lib/products/products";
import { getCookie } from "typescript-cookie";
import { DateField, LocalizationProvider } from "@mui/x-date-pickers";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import { green } from "@mui/material/colors";
import { IDocument } from "../../shared/models/docs";
import dayjs, { Dayjs } from "dayjs";

const debugDocuments = (count: number): IDocument[] => {
    return new Array(count).fill(null).map((_, i) => {
        return {
            link: `http://localhost:3000/document?id=${i}`,
            filename: `File${i}`,
            file: new File([], `f${i}`),
        };
    });
};

const DocsPage = () => {
    const [documents, setDocuments] = useState<IDocument[]>(debugDocuments(0));
    const [beginDate, setBeginDate] = useState<Dayjs | null>(
        dayjs().subtract(1, "month")
    );
    const [endDate, setEndDate] = useState<Dayjs | null>(dayjs());

    const fetchDocuments = useCallback(async (dateFrom?: Date, dateTo?: Date) => {
        try {
            const { data } = await getStats(
                getCookie("jwt-authorization") ?? "",
                beginDate?.toISOString() ?? "",
                endDate?.toISOString() ?? ""
            );

            setDocuments([{ filename: "diagrams-stats", link: data.link }]);
        } catch (error) {}
    },[beginDate, endDate]);

    useEffect(() => {
        fetchDocuments();
    },[fetchDocuments]);

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
