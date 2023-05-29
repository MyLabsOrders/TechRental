import { Button, Box, Modal } from "@mui/material";
import { green } from "@mui/material/colors";
import { useState } from "react";
import { RegisterForm } from "../RegisterForm";

export interface CreateUserProps {
	onCloseCallback: ()=>void
}

const CreateUserForm = ({onCloseCallback}:CreateUserProps) => {
    const [isModaOpen, setIsModalOpen] = useState(false);

    const handleOpen = () => {
        setIsModalOpen(true);
    };

    const handleClose = () => {
        setIsModalOpen(false);
		onCloseCallback();
    };

    return (
        <Box>
            <Button
                onClick={handleOpen}
                sx={{
                    "&:hover": {
                        backgroundColor: green[500],
                        color: "white",
                    },
                }}>
                Create user
            </Button>
            <Modal
                open={isModaOpen}
                onClose={handleClose}
                sx={{ backdropFilter: "blur(5px)" }}>
                <RegisterForm
                    oncloseCallback={handleClose}
                    isModal={true}
                />
            </Modal>
        </Box>
    );
};

export default CreateUserForm;

