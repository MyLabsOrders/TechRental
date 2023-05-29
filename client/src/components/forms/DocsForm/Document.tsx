import { Box, Link, Typography } from "@mui/material";
import { green } from "@mui/material/colors";
import { IDocument } from "../../../shared/models/docs";

const Document = ({ filename, link }: IDocument) => {
    return (
        <Box
            sx={{
                display: "flex",
                flexDirection: "row",
                justifyContent: "space-between",
            }}>
            <Typography variant="body1" color={"white"}>{filename}</Typography>
            <Link
                href={link}
                target="_blank"
                rel="noopener"
                underline="none"
                padding={"5px"}
                sx={{
                    color: "white",
                    transition: "0.3s",
                    borderRadius: "5px",
                    "&:hover": {
                        backgroundColor: green[500],
                        color: "white",
                    },
                }}>
                <Typography variant="body1" color={"white"}>Open</Typography>
            </Link>
        </Box>
    );
};

export default Document;

