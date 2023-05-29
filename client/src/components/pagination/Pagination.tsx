import { useEffect, useState } from "react";
import axios from "axios";
import ProductsContainer from "../containers/ItemContainer";
import { IProduct, createProducts } from "../forms/ProductForm/Product";

export interface IPaginationProps {
    apiUrl: string;
}

const Pagination = ({ apiUrl }: IPaginationProps) => {
    // const [products, setProducts] = useState<IProduct[]>([]);
    const [products, setProducts] = useState<IProduct[]>(createProducts(10));
    const [currentPage, setCurrentPage] = useState(1);
    const [fetching, setFetching] = useState(false);

    useEffect(() => {
        if (fetching) {
            axios
                .get(`${apiUrl}?page=${currentPage}`)
                .then((response) => {
                    setProducts([...products, ...response.data.orders]);
                    setCurrentPage((prev) => prev + 1);
                })
                .finally(() => setFetching(false));
        }
    });

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

