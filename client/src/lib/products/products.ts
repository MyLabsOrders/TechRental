import axios from "axios";
import { ChangeProductStatusDto, CreateOrderDto } from "../../shared/dto";
import { IProduct, IProductPage } from "../../shared";
import { IDocument } from "../../shared/models/docs";

export const api = axios.create({ baseURL: process.env.REACT_APP_PRODUCT_API });

export const changeProductStatus = async (
    token: string,
    dto: ChangeProductStatusDto
) => {
    return await api.put("status", dto, {
        headers: {
            Authorization: `Bearer ${token}`,
        },
    });
};

export const createProduct = async (token: string, dto: CreateOrderDto) => {
    return await api.post<IProduct>("/", dto, {
        headers: {
            Authorization: `Bearer ${token}`,
        },
    });
};

export const getAllProducts = async (page?: number) => {
    return await api.get<IProductPage>(`${page ?? "/"}`);
};

export const getProduct = async (token: string, id: string) => {
    return await api.get<IProduct>(`${id}`, {
        headers: {
            Authorization: `Bearer ${token}`,
        },
    });
};

export const getCheque = async (token: string, date: string) => {
    return await api.get<IDocument>(`/cheque?orderTime=${date}`, {
        headers: {
            Authorization: `Bearer ${token}`,
        },
    });
};

//Todo: review
export const getDocuments = async (token: string) => {
    const result: IDocument[] = [];
    const invoiceResponse = await api.get<IDocument>(`/invoice`, {
        headers: {
            Authorization: `Bearer ${token}`,
        },
    });
    result.push(invoiceResponse.data);

    const chequeResponse = await api.get<IDocument>(`/cheque`, {
        headers: {
            Authorization: `Bearer ${token}`,
        },
    });
    result.push(chequeResponse.data);
    return result;
};

//Todo: review
export const getHistory = async (page?: number): Promise<IProductPage> => {
    const { data } = await api.get<IProductPage>(`${page ?? "/"}`);
    data.orders = data.orders.filter((order) => order.orderDate !== null);
    return data;
};

