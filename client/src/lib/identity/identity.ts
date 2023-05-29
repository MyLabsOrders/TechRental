import axios from "axios"
import { LoginDto, RegisterDto } from "../../shared/dto/identity/request"
import { IdentityResponseDto, LoginResponseDto, TokenResponseDto } from "../../shared/dto/identity"

export const api = axios.create({ baseURL: process.env.REACT_APP_IDENTITY_API })

export const login = async (dto: LoginDto) => {
	return await api.post<LoginResponseDto>('login', dto)
}

export const register = async (dto: RegisterDto) => {
	return await api.post<TokenResponseDto>('user/register', dto)
}

export const changeRole = async (token: string, username: string, roleName: string) => {
	return await api.put(`users/${username}/role`, roleName, {
		headers: {
			Authorization: `Bearer ${token}`
		}
	})
}

export const changeUsername = async (token: string, username: string) => {
	return await api.put(`username`, username, {
		headers: {
			Authorization: `Bearer ${token}`
		}
	})
}

export const changePassword = async (token: string, password: string) => {
	return await api.put(`password`, password, {
		headers: {
			Authorization: `Bearer ${token}`
		}
	})
}

export const authorizeAdmin = async (token: string, username: string) => {
	return await api.post('authorize-admin', {username}, {
		headers: {
			Authorization: `Bearer ${token}`
		}
	})
}

export const getIdentityUser = async (token: string, userId: string) => {
	return await api.get<IdentityResponseDto>(`${userId}`, {
		headers: {
			Authorization: `Bearer ${token}`,
		}
	})
}