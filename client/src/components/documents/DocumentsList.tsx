import { Box, Container } from "@mui/material";
import { Document } from "../forms/DocsForm";
import { IDocument } from "../../shared/models/docs";

interface ProductContainerProps {
    documents: IDocument[];
}

const DocumentsList = ({ documents }: ProductContainerProps) => {
    return (
        <Container
            sx={{
                display: "flex",
                justifyContent: "center",
                flexDirection: "column",
                rowGap: "10px",
            }}>
            {documents.map((document, i) => (
                <Box
                    key={i}
                    height={"100%"}
                    sx={{
                        display: "flex",
                        flexDirection: "column",
                        justifyContent: "space-between",
                        backgroundColor: "#0a1929",
                        padding: "10px",
                        borderRadius: "4px",
                        boxShadow: "0 2px 4px rgba(0, 0, 0, 0.2)",
                    }}>
                    <Document
                        file={document.file}
                        filename={document.filename}
                        link={document.link}
                    />
                </Box>
            ))}
        </Container>
    );
};

export default DocumentsList;

