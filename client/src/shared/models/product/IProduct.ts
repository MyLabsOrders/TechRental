interface IProduct {
    id: string;
    status: string;
    name: string;
    image: string;
    total: number;
    orderDate: string;
}

export const createProduct = (
    id: string,
    status: string,
    name: string,
    image: string,
    total: number,
    orderDate: string
): IProduct => {
    return { id, status, name, image, total, orderDate };
};

export default IProduct;

