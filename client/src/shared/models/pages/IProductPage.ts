import { IProduct } from "../product";

interface IProductPage {
    orders: IProduct[];
    page: number;
    totalPage: number;
}

export const createProductPage = (
    orders: IProduct[],
    page: number,
    totalPage: number
): IProductPage => {
    return { orders, page, totalPage };
};

export default IProductPage;
