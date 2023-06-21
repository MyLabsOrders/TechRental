import { Box, Button, FormControl, TextField, Typography } from "@mui/material";
import { green } from "@mui/material/colors";
import { useState } from "react";
import { replenishBalance } from "../../../lib/users/users";
import { getCookie } from "typescript-cookie";
import { useNavigate } from "react-router-dom";

interface ReplenishFormProps {
	close: () => void
}

const ReplenishForm = ({ close }: ReplenishFormProps) => {

	const [amount, setAmount] = useState(0)
	const navigate = useNavigate()

	const handleSubmit = async (e: React.FormEvent) => {
		e.preventDefault()

		try {
			await replenishBalance(getCookie('jwt-authorization') ?? '', getCookie('current-user') ?? '', amount)
			navigate('/profile', { state: { message: `Successfuly deposited ${amount}`, type: "success" } })
		} catch (error: any) {
			close()
			navigate('/profile', { state: { message: `${error?.response?.data?.Detailes}`, type: "error" } })
		}
	}

	const handleAmountChange = (e: React.ChangeEvent<HTMLInputElement>) => {
		const newAmount = parseInt(e.target.value);
		setAmount(newAmount || 0);
	}

	return (
		<Box sx={{
			p: 2,
			display: "flex",
			flexDirection: "column",
			alignItems: "center",
			justifyContent: "center",
			backgroundColor: "#132f4b",
			borderRadius: 3,
			maxWidth: '400px',
			margin: '0 auto',
		}}>
			<Typography variant="h6" align="center" color='white'>
				Пополнить счет
			</Typography>
			<FormControl component="form" onSubmit={handleSubmit}>
				<Box sx={{ display: "flex", alignItems: "center", justifyContent: "space-between" }}>
					<TextField
						id="quantity"
						variant="standard"
						type="text"
						onChange={handleAmountChange}
						value={amount}
						InputProps={{
							style: {
								color: "white",
							},
						}}
						sx={{
							width: "70px",
							textAlign: "center",
							"& .MuiInputBase-root": {
								color: "white",
								"&:before": {
									borderBottomColor: "white",
								},
								"&:hover:before": {
									borderBottomColor: "white",
								},
								"&.Mui-focused:before": {
									borderBottomColor: "green",
								},
								"&.Mui-focused:after": {
									borderBottomColor: "green",
								},
							},
						}}
					/>
				</Box>
				<Button
					variant="contained"
					color="primary"
					type="submit"
					onClick={handleSubmit}
					sx={{
						backgroundColor: 'inherit', "&:hover": {
							backgroundColor: green[500],
							color: "white",
						}
					}}>
					Внести средства
				</Button>
			</FormControl>
		</Box>
	);
}

export default ReplenishForm