import { useState, ChangeEvent, FormEvent } from "react";
import { Button, TextField, Typography, Box } from "@mui/material";
import { Link, useNavigate } from "react-router-dom";
import { green } from "@mui/material/colors";
import { login } from "../../../lib/identity/identity";
import { Notification } from "../../../features";
import { setCookie } from "typescript-cookie";

const LoginForm = () => {
	const [username, setUsername] = useState("")
	const [password, setPassword] = useState("")
	const [error, setError] = useState<string | null>(null)
	const navigate = useNavigate();

	const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
		event.preventDefault();

		try {
			setError(null)

			const { data } = await login({ username, password })
			setCookie('jwt-authorization', data.token)
			setCookie('current-user', data.userId)
			setCookie('current-username', username);

			navigate("/", { state: { message: "Successfully logged in!", type: "success" } })
		} catch (error: any) {
			setError(error?.response?.data?.Detailes)
		}
	};

	const handleUsernameChange = (event: ChangeEvent<HTMLInputElement>) => {
		setUsername(event.target.value);
	};

	const handlePasswordChange = (event: ChangeEvent<HTMLInputElement>) => {
		setPassword(event.target.value);
	};

	return (
		<>
			{error && <Notification message={error} type='error' />}
			<Box sx={{ display: "flex", justifyContent: 'center' }}>
				<Box
					component="form"
					autoComplete="off"
					onSubmit={handleSubmit}
					sx={{
						display: "flex",
						flexDirection: "column",
						alignItems: "center",
						marginTop: '100px'
					}}
				>
					<Typography variant="h4" sx={{ mb: 3, color: 'white' }}>
						Вход
					</Typography>
					<TextField
						label="Логин"
						onChange={handleUsernameChange}
						required
						variant="outlined"
						color="primary"
						type="text"
						sx={{
							mb: 3, backgroundColor: 'white', borderRadius: 1,
							"& .MuiOutlinedInput-root.Mui-focused .MuiOutlinedInput-notchedOutline": {
								borderColor: "white",
							},
							"& .MuiInputLabel-root.Mui-focused": {
								color: 'white',
								textShadow: '0 2px grey'
							}
						}}
						value={username}
					/>
					<TextField
						label="Пароль"
						onChange={handlePasswordChange}
						required
						variant="outlined"
						color="primary"
						type="password"
						sx={{
							mb: 3, backgroundColor: 'white', borderRadius: 1,
							"& .MuiOutlinedInput-root.Mui-focused .MuiOutlinedInput-notchedOutline": {
								borderColor: "white",
							},
							"& .MuiInputLabel-root.Mui-focused": {
								color: 'white',
								textShadow: '0 2px grey'
							}
						}}
						value={password}
					/>
					<Button
						variant="contained"
						color="primary"
						type="submit"
						sx={{
							backgroundColor: 'inherit', "&:hover": {
								backgroundColor: green[500],
								color: "white",
							}
						}}>
						Войти
					</Button>
					<Typography variant="body2" sx={{ mt: 2, color: 'white' }}>
						Нет аккаунта? <Link to="/register">Регистрация</Link>
					</Typography>
				</Box>
			</Box>
		</>
	);
};

export default LoginForm;
