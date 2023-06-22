import { Box, Button, Stack } from "@mui/material";
import { ItemTable, ProfileForm } from "../../components";
import { useEffect, useState } from "react";
import { getCookie } from "typescript-cookie";
import { IProductPage } from "../../shared";
import { Notification } from "../../features";
import { useLocation } from "react-router-dom";
import { getCheque, getInvoice } from "../../lib/products/products";
import { getUser } from "../../lib/users/users";

const Profile = () => {
    const [error, setError] = useState<string | null>(null);
    const [products, setProducts] = useState<IProductPage | null>();

    const [invoiceLink, setInvoiceLink] = useState<string>();
    const [chequeLink, setChequeLink] = useState<string>();

    const location = useLocation();
    const message = location.state && location.state.message;
    const type = location.state && location.state.type;

    const fetchItems = async () => {
        try {
            const { data } = await getUser(
                getCookie("current-user") ?? "",
                getCookie("jwt-authorization") ?? ""
            );
            setProducts({ orders: data.orders, page: 1, totalPage: 1 });
        } catch (error) {
            setProducts(null);
        }
    };
    useEffect(() => {
        fetchItems();
        fetchCheque();
        fetchInvoice();
    }, []);

    const fetchCheque = async () => {
        try {
            const { data } = await getCheque(
                getCookie("jwt-authorization") ?? "",
                getCookie("order-date") ?? ""
            );
            setChequeLink(data.link);
        } catch (error) {
            console.log(error);
        }
    };

    const fetchInvoice = async () => {
        try {
            const { data } = await getInvoice(
                getCookie("jwt-authorization") ?? "",
                getCookie("order-date") ?? ""
            );
            setInvoiceLink(data.link);
        } catch (error) {
            console.log(error);
        }
    };

    return (
        <Box
            bgcolor="#132f4b"
            display={"flex"}
            alignItems={"center"}
            height={"100vh"}
            width={"100vw"}>
            {error && <Notification message={error} type="error" />}
            {message && <Notification message={message} type={type} />}
            <Box
                display={"flex"}
                justifyContent={"space-evenly"}
                width={"100%"}>
                <Stack spacing={"2rem"}>
                    <ProfileForm setError={setError} />
                    <Button
                        href={chequeLink as string}
                        target="_blank"
                        disabled={!products || products?.orders.length === 0}
                        // onClick={fetchCheque}
                        sx={{
                            background: "#0a1929",
                            borderRadius: "15px",
                            ":hover": { background: "#001e3c" },
                        }}>
                        Получить чек
                    </Button>
                    <Button
                        href={invoiceLink as string}
                        target="_blank"
                        disabled={!products || products?.orders.length === 0}
                        // onClick={fetchInvoice}
                        sx={{
                            background: "#0a1929",
                            borderRadius: "15px",
                            ":hover": { background: "#001e3c" },
                        }}>
                        Получить накладную
                    </Button>
                </Stack>
                <ItemTable
                    products={products ?? { orders: [], page: 0, totalPage: 0 }}
                />
            </Box>
        </Box>
    );
};

export default Profile;
