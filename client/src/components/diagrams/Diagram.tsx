import { Box, Container, Grid, Typography } from "@mui/material";

interface ProductContainerProps {
    document?: Document;
}

const DiagramsComponent = ({ document }: ProductContainerProps) => {
    return (
        <Container
            sx={{
                display: "flex",
                justifyContent: "center",
                alignItems: "center",
                backgroundColor: "#0a1929"
            }}>
                <Typography color={"white"}>Диаграммы</Typography>
            <>
                
                {
                    //TODO
                }
            </>
        </Container>
    );
};

export default DiagramsComponent;

