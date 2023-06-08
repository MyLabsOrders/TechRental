import { Route, Routes } from "react-router-dom";
import { HomePage, LoginPage, ProfilePage, RegisterPage } from "./pages";
import { Container } from "@mui/material";
import { Header } from "./features";
import { HistoryPage } from "./pages/HistoryPage";
import { DocsPage } from "./pages/DocsPage";
import { AdminPage } from "./pages/AdminPage";
import { useEffect, useState } from "react";
import { authorizeAdmin } from "./lib/identity/identity";
import { getCookie } from "typescript-cookie";

function App() {
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
        <>
            <Container>
                <Header />
                <Routes>
                    <Route path="/" element={<HomePage />} />
                    <Route path="/register" element={<RegisterPage />} />
                    <Route path="/login" element={<LoginPage />} />
                    <Route path="/profile" element={<ProfilePage />} />
                    <Route path="/history" element={<HistoryPage />} />
                    <Route path="/documents" element={<DocsPage />} />
                    <Route
                        path="/admin"
                        element={isAdmin ? <AdminPage /> : <HomePage />}
                    />
                </Routes>
            </Container>
        </>
    );
}

export default App;

