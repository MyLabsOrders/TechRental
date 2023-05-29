import { Box } from "@mui/material";
import { ItemTable } from "../../components";
import { useEffect, useState } from "react";
import {  IProductPage } from "../../shared";
import { createProduct } from "../../shared/models/product/IProduct";
import { getHistory } from "../../lib/products/products";

const createProducts = (count: number): IProductPage => {
    return {
        orders: new Array(count)
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
            ),
        page: 1,
        totalPage: 1,
    };
};

const HistoryPage = () => {
    const [products, setProducts] = useState<IProductPage>(createProducts(2));

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
