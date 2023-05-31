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
    return await api.get<IProductPage>(page ? `?page=${page}` : "");
};

export const getProduct = async (token: string, id: string) => {
    return await api.get<IProduct>(`${id}`, {
        headers: {
            Authorization: `Bearer ${token}`,
        },
    });
};

export const getCheque = async (token: string, date: string) => {
    const { data } = await api.get<Blob>(`/cheque?orderTime=${date}`, {
        responseType: "arraybuffer",
        headers: {
            "Content-Type": "application/pdf",
            Accept: "application/pdf",
            Authorization: `Bearer ${token}`,
        },
    });
    // console.log(data);
    const blob = new Blob([data as unknown as BlobPart], {
        type: "application/pdf",
    });
    const url = URL.createObjectURL(blob);
    return { data: { link: url } };
};

export const getInvoice = async (token: string, date: string) => {
    const { data } = await api.get<Blob>(`/invoice?orderTime=${date}`, {
        responseType: "arraybuffer",
        headers: {
            "Content-Type": "application/pdf",
            Accept: "application/pdf",
            Authorization: `Bearer ${token}`,
        },
    });
    const blob = new Blob([data], { type: "application/pdf" });
    const url = URL.createObjectURL(blob);
    return { data: { link: url } };
};


export const getStats = async (token: string, from: string, to: string) => {
    const { data } = await api.get<Blob>(`/stats?From=${from}&To=${to}`, {
        headers: {
            "Content-Type": "application/pdf",
            Accept: "application/pdf",
            Authorization: `Bearer ${token}`,
        },
    });

    const blob = new Blob([data], { type: "application/pdf" });
    const url = URL.createObjectURL(blob);
    return { data: { link: url } };
};


export const getHistory = async (page?: number): Promise<IProductPage> => {
    const { data } = await api.get<IProductPage>(`${page ?? "/"}`);
    data.orders = data.orders.filter((order) => order.orderDate !== null);
    return data;
};
