import { Box, Button, Stack } from "@mui/material";
import { ItemTable, ProfileForm } from "../../components";
import { useEffect, useState } from "react";
import { getCookie } from "typescript-cookie";
import { IProductPage } from "../../shared";
import { Notification } from "../../features";
import { useLocation } from "react-router-dom";
import { getCheque, getInvoice } from "../../lib/products/products";
import { createProduct } from "../../shared/models/product/IProduct";
import { getUser } from "../../lib/users/users";

const createProducts = (count: number): IProductPage => {
    const orders = new Array(count)
        .fill(null)
        .map((_, i) =>
            createProduct(
                `${i}`,
                `status${i}`,
                `name${i}`,
                "https://loremflickr.com/640/360",
                0,
                `01.01.000${i}`
            )
        );

    return { orders, page: 0, totalPage: 2 };
    // return orders;
};

const Profile = () => {
    const [error, setError] = useState<string | null>(null);
    const [products, setProducts] = useState<IProductPage | null>();

    const [documentLink, setDocumentLink] = useState<string>();

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
    }, []);

    const fetchCheque = async () => {
        try {
            const { data } = await getCheque(
                getCookie("jwt-authorization") ?? "",
                getCookie("order-date") ?? ""
            );
            setDocumentLink(data.link);
            window.open(documentLink, "_blank", "noreferrer");
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
            setDocumentLink(data.link);
            window.open(documentLink, "_blank", "noreferrer");
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
                        disabled={!products || products?.orders.length === 0}
                        onClick={fetchCheque}
                        sx={{
                            background: "#0a1929",
                            borderRadius: "15px",
                            ":hover": { background: "#001e3c" },
                        }}>
                        Получить чек
                    </Button>
                    <Button
                        disabled={!products || products?.orders.length === 0}
                        onClick={fetchInvoice}
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
