import {
    Box,
    Button,
    CardMedia,
    Container,
    Stack,
    Typography,
} from "@mui/material";
import { Link } from "react-router-dom";
import "./Navbar.css";
import { useState, useEffect } from "react";
import { getCookie } from "typescript-cookie";
import { authorizeAdmin } from "../../lib/identity/identity";
import TechRentalLogo from "../../assets/TechRentalLogo.png";

const Navbar = () => {
    const [isAdmin, setIsAdmin] = useState(false);

    useEffect(() => {
        getUser();
    }, []);

    const getUser = async () => {
        try {
            //If not error - user = admin
            await authorizeAdmin(
                getCookie("jwt-authorization") ?? "",
                getCookie("current-username") ?? ""
            );
            setIsAdmin(true);
        } catch (error) {
            setIsAdmin(false);
        }
    };

    return (
        <Box
            maxWidth="inherited"
            bgcolor="rgba(7, 27, 47, 0.8)"
            color="#fff"
            top={0}
            py={2}
            boxShadow={0}
            position="fixed"
            width="100%"
            zIndex={1}
            sx={{ backdropFilter: "blur(2px)" }}>
            <Container
                sx={{
                    display: "flex",
                    justifyContent: "space-between",
                    alignItems: "center",
                }}>
                <Box display="flex" flexDirection="row">
                    <CardMedia
                        component="img"
                        src={TechRentalLogo}
                        alt=""
                        sx={{height: "4rem", width: "4rem", marginRight: "1rem", opacity: 0.6, userSelect: "none"}}
                    />
                    <Typography variant="h3" className="logo">
                        Tech rental
                    </Typography>
                </Box>
                <Stack direction="row" spacing={2}>
                    <Button component={Link} to="/">
                        Главная
                    </Button>
                    <Button component={Link} to="/profile">
                        Профиль
                    </Button>
                    <Button component={Link} to="/history">
                        История
                    </Button>
                    <Button component={Link} to="/documents">
                        Документы
                    </Button>
                    <Button component={Link} to="/login">
                        Вход
                    </Button>
                    <Button component={Link} to="/register">
                        Регистрация
                    </Button>
                    {isAdmin && (
                        <Button component={Link} to="/admin">
                            Админ
                        </Button>
                    )}
                </Stack>
            </Container>
        </Box>
    );
};

export default Navbar;
