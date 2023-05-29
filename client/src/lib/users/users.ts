import { IUser, IUserPage } from "../../shared/models";
import { CreateProfileDto } from "../../shared/dto";
import axios from "axios";

export const api = axios.create({ baseURL: process.env.REACT_APP_USER_API })

export const getUser = async (id: string, token: string) => {
	return await api.get<IUser>(id, {headers: {
		Authorization: `Bearer ${token}`
	}})
}

export const getAllUsers = async (token: string, page?: number) => {
	return await api.get<IUserPage>(`${page ?? ''}`, {
		headers: {
			Authorization: `Bearer ${token}`
		}
	})
}

export const createUserProfile = async (token: string, id: string, dto: CreateProfileDto) => {
	return await api.post<IUser>(`${id}/profile`, dto, {
		headers: {
			Authorization: `Bearer ${token}`
		}
	})
}

export const changeUserProfile = async (token: string, id: string, dto: Partial<CreateProfileDto>) => {
	return await api.patch<IUser>(`${id}/profile`, dto, {
		headers: {
			Authorization: `Bearer ${token}`
		}
	})
}

export const addProduct = async (token: string, id: string, orderId: string) => {
	return await api.put(`${id}/orders`, {orderId}, {
		headers: {
			Authorization: `Bearer ${token}`
		}
	})
}

export const removeProduct = async (token: string, id: string, productId: string) => {
	return await api.delete(`${id}/orders`, {
		headers: {
			Authorization: `Bearer ${token}`
		},
		data: {
			source: productId
		}
	})
}

export const replenishBalance = async (token: string, id: string, total: number) => {
	return await api.put(`${id}/account`, {total}, {
		headers: {
			Authorization: `Bearer ${token}`
		}
	})
}
