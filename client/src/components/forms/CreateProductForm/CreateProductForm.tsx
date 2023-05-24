import { Button, Box, Modal, FormControl, TextField } from "@mui/material"
import { green } from "@mui/material/colors"
import { borderRadius } from "@mui/system"
import { useState } from "react"
import { createProduct } from "../../../lib/products/products"
import { getCookie } from "typescript-cookie"
import { useNavigate } from "react-router"

const CreateProductForm = () => {
	const [isModaOpen, setIsModalOpen] = useState(false)
	const [status, setStatus] = useState('')
	const [total, setTotal] = useState(0)
	const [name, setName] = useState('')
	const navigate = useNavigate()

	const handleOpen = () => {
		setIsModalOpen(true)
	}

	const handleClose = () => {
		setIsModalOpen(false)
	}

	const handleClick = async () => {
		setIsModalOpen(false);

		try {
			await createProduct(
				getCookie('jwt-authorization') ?? '',
				{ name: name, status: status, total: total, orderImage: '' })
		} catch (error: any) {
			navigate("/profile", { state: { message: `${error?.message}`, type: "error" } })

		}
	}

	return (
		<Box>
			<Button
				onClick={handleOpen}
				sx={{
					"&:hover": {
						backgroundColor: green[500],
						color: "white",
					}
				}}>
				Create product
			</Button>
			<Modal open={isModaOpen} onClose={handleClose} sx={{ backdropFilter: "blur(5px)" }}>
				<Box
					sx={{
						position: "absolute",
						top: "50%",
						left: "50%",
						transform: "translate(-50%, -50%)",
						width: 400,
						bgcolor: "background.paper",
						boxShadow: 24,
						p: 4,
						display: 'flex',
						justifyContent: 'center',
						alignItems: 'center',
						borderRadius: '10px'
					}}
				>
					<FormControl>
						<TextField value={status} onChange={(e) => setStatus(e.target.value)} label="Status ( Available/Rented ) " fullWidth />
						<TextField value={name} onChange={(e) => setName(e.target.value)} label="Name" fullWidth />
						<TextField value={total} onChange={(e) => setTotal(e.target.value as unknown as number)} label="Total" fullWidth type="number" />
						<Button
							onClick={handleClick}
							sx={{
								"&:hover": {
									backgroundColor: green[500],
									color: "white",
								},
								backgroundColor: '#132f4b'
							}}>
							Create
						</Button>
					</FormControl>
				</Box>
			</Modal>
		</Box>
	)
}

export default CreateProductForm