import { Box } from "@mui/material";
import { ItemTable } from "../../components";
import { useEffect, useState } from "react";
import {  IProductPage } from "../../shared";
import { getHistory } from "../../lib/products/products";

const HistoryPage = () => {
    const [products, setProducts] = useState<IProductPage>();

    const fetchHistory = async () => {
        try {
            const historyOrders  = await getHistory();
            setProducts(historyOrders);
        } catch (error) {
            console.log(error)
        }
    }

    useEffect(()=>{
        fetchHistory();
    },[])

    return (
        <Box
            bgcolor="#132f4b"
            display={"flex"}
            alignItems={"center"}
            height={"100vh"}
            width={"100vw"}>
            <Box
                display={"flex"}
                justifyContent={"space-evenly"}
                width={"100%"}>
                <ItemTable
                    products={products ?? { orders: [], page: 0, totalPage: 0 }}
                />
            </Box>
        </Box>
    );
};

export default HistoryPage;
