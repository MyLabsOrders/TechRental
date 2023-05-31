import { useEffect, useState } from "react";
import axios from "axios";
import ProductsContainer from "../containers/ItemContainer";
import {
    IProduct,
    createProduct,
    createProducts,
} from "../forms/ProductForm/Product";
import { getAllProducts } from "../../lib/products/products";

import kamazImg from "../../assets/data_tech/Камаз.jpg";
import _695STImg from "../../assets/data_tech/695ST.jpg";
import CDM835Img from "../../assets/data_tech/CDM835.jpg";
import NiewiadowImg from "../../assets/data_tech/Niewiadow.jpg";
import RobinsImg from "../../assets/data_tech/Robbins.jpg";
import ST408Img from "../../assets/data_tech/ST408.jpg";
import TE244Img from "../../assets/data_tech/TE244.jpg";
import ZTC250VImg from "../../assets/data_tech/ZTC250V.jpg";
import taktikImg from "../../assets/data_tech/Тактик.jpg";
import sherpImg from "../../assets/data_tech/Шерп.jpg";

const defaultProducts = (): IProduct[] => {
    const products: IProduct[] = [
        createProduct("0", "Камаз", 17600, "available", kamazImg),
        createProduct("1", "695ST", 20000, "available", _695STImg),
        createProduct("2", "CDM835", 40000, "available", CDM835Img),
        createProduct("3", "Niewiadow", 5000, "available", NiewiadowImg),
        createProduct("4", "Robbins", 8000, "available", RobinsImg),
        createProduct("5", "ST408", 13000, "available", ST408Img),
        createProduct("6", "TE244", 12000, "available", TE244Img),
        createProduct("7", "ZTC250V", 40000, "available", ZTC250VImg),
        createProduct("8", "Тактик", 50000, "available", taktikImg),
        createProduct("9", "Шерп", 40000, "available", sherpImg),
    ];
    return products;
};

export interface IPaginationProps {
    apiUrl: string;
}

const Pagination = ({ apiUrl }: IPaginationProps) => {
    const [products, setProducts] = useState<IProduct[]>([]);
    // const [products, setProducts] = useState<IProduct[]>(defaultProducts());
    const [currentPage, setCurrentPage] = useState(1);
    const [fetching, setFetching] = useState(true);

    useEffect(() => {
        if (fetching) {
            fetchProducts();
        }
    }, []);

    const fetchProducts = async () => {
        try {
            const { data } = await getAllProducts(currentPage);
            setProducts([
                ...products, //Remain already fetched products
                ...data.orders.filter((order) => !order.orderDate), //+ append orders without orderDate
            ]);
            setCurrentPage((prev) => prev + 1);
        } catch (error) {}
    };

    useEffect(() => {
        document.addEventListener("scroll", handleScroll);

        return function () {
            document.removeEventListener("scroll", handleScroll);
        };
    }, []);

    const handleScroll = (e: any) => {
        if (
            e.target.documentElement.scrollHeight -
                (e.target.documentElement.scrollTop + window.innerHeight) <
            100
        ) {
            setFetching(true);
        }
    };

    return (
        <>
            <ProductsContainer products={products} />
        </>
    );
};

export default Pagination;
