import { useEffect, useState } from "react";
import ProductsContainer from "../containers/ItemContainer";
import { IProduct } from "../forms/ProductForm/Product";
import { getAllProducts } from "../../lib/products/products";

export interface IPaginationProps {
    apiUrl: string;
}

const Pagination = ({ apiUrl }: IPaginationProps) => {
    const [products, setProducts] = useState<IProduct[]>([]);
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
