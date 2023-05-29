import {
    Box,
    Button,
    Dialog,
    MenuItem,
    Select,
    SelectChangeEvent,
    TextField,
    Typography,
} from "@mui/material";
import { IUser } from "../../shared";
import { green, red } from "@mui/material/colors";
import { useEffect, useState } from "react";
import {
    changeRole,
    changeUsername,
    getIdentityUser,
} from "../../lib/identity/identity";
import { getCookie } from "typescript-cookie";
import { changeUserProfile } from "../../lib/users/users";

export interface UserElementProps {
    user: IUser;
    deleteCallback: (user: IUser) => {};
}

const UserElement = ({ user, deleteCallback }: UserElementProps) => {
    const [role, setRole] = useState<string>("user");

    const [isDialogOpen, setDialogOpen] = useState<boolean>(false);
    const [username, setUsername] = useState<string>("");

    const [editedFirstName, setEditedFirstName] = useState(user.firstName);
    const [editedMiddleName, setEditedMiddleName] = useState(user.middleName);
    const [editedLastName, setEditedLastName] = useState(user.lastName);
    const [editedBirthDate, setEditedBirthDate] = useState(user.birthDate);
    const [editedUsername, setEditedUsername] = useState(username);

    useEffect(() => {
        fetchIdentity();
    });

    const deleteUser = () => {
        deleteCallback(user);
    };

    const changeUser = async () => {
        try {
            user.firstName = editedFirstName;
            user.middleName = editedMiddleName;
            user.lastName = editedLastName;
            user.birthDate = editedBirthDate;

            await changeUserProfile(
                getCookie("jwt-authorization") ?? "",
                getCookie("current-user") ?? "",
                {
                    firstName: user.firstName,
                    middleName: user.middleName,
                    lastName: user.lastName,
                    birthDate: user.birthDate,
                }
            );
            await changeUsername(
                getCookie("jwt-authorization") ?? "",
                editedUsername
            );
        } catch (error) {
            // console.log(error);
        }
    };

    const openModal = () => {
        setDialogOpen(true);
        setEditedFirstName(user.firstName);
        setEditedMiddleName(user.middleName);
        setEditedLastName(user.lastName);
        setEditedBirthDate(user.birthDate);
    };
    const closeModal = () => setDialogOpen(false);

    const changeUserRole = async (event: SelectChangeEvent) => {
        setRole(event.target.value as string);
        try {
            await changeRole(
                getCookie("jwt-authorization") ?? "",
                username,
                role
            );
        } catch (error) {}
    };

    const fetchIdentity = async () => {
        try {
            const { data } = await getIdentityUser(
                getCookie("jwt-authorization") ?? "",
                user.id
            );
            setRole(data.role);
            setUsername(data.username);
        } catch (error) {
            // console.log(error);
        }
    };

    return (
        <Box
            display={"flex"}
            flexDirection={"row"}
            justifyContent={"space-between"}
            alignItems={"center"}
            width={"100%"}
            padding={"10px"}
            marginBottom={"10px"}
            bgcolor={"#0a1929"}>
            <Typography color={"white"}>
                {user.firstName} {user.middleName}
            </Typography>
            <Select
                sx={{
                    svg: { color: "white" },
                    input: { color: "white" },
                    label: { color: "white" },
                    color: "white",
                }}
                labelId="change-user-role"
                id="change-role"
                value={role}
                label="Age"
                onChange={changeUserRole}>
                <MenuItem value={"user"}>User</MenuItem>
                <MenuItem value={"admin"}>Admin</MenuItem>
            </Select>
            <Box>
                <Button
                    onClick={openModal}
                    sx={{ backgroundColor: green[500], marginRight: "10px" }}>
                    Изменить
                </Button>
                <Button onClick={deleteUser} sx={{ backgroundColor: red[500] }}>
                    Удалить
                </Button>
            </Box>
            <Dialog
                open={isDialogOpen}
                sx={{
                    "& .MuiPaper-root": {
                        borderRadius: "1rem",
                        bgcolor: "#0a1929",
                    },
                }}>
                <Box bgcolor={"inherit"} padding={"2rem"}>
                    <TextField
                        variant="standard"
                        value={editedUsername}
                        onChange={(e) => setEditedUsername(e.target.value)}
                        fullWidth
                        InputProps={{
                            style: {
                                color: "white",
                            },
                        }}
                        sx={{
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
                    <TextField
                        variant="standard"
                        value={editedFirstName}
                        onChange={(e) => setEditedFirstName(e.target.value)}
                        fullWidth
                        InputProps={{
                            style: {
                                color: "white",
                            },
                        }}
                        sx={{
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
                    <TextField
                        variant="standard"
                        value={editedMiddleName}
                        onChange={(e) => setEditedMiddleName(e.target.value)}
                        fullWidth
                        InputProps={{
                            style: {
                                color: "white",
                            },
                        }}
                        sx={{
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
                    <TextField
                        variant="standard"
                        value={editedLastName}
                        onChange={(e) => setEditedLastName(e.target.value)}
                        fullWidth
                        InputProps={{
                            style: {
                                color: "white",
                            },
                        }}
                        sx={{
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
                    <TextField
                        type="date"
                        variant="standard"
                        value={editedBirthDate}
                        onChange={(e) => {
                            setEditedBirthDate(e.target.value);
                        }}
                        fullWidth
                        InputProps={{
                            style: {
                                color: "white",
                            },
                        }}
                        sx={{
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
                    <Box
                        display={"flex"}
                        flexDirection={"row"}
                        width={"100%"}
                        justifyContent={"space-between"}
                        marginTop={"1rem"}>
                        <Button
                            onClick={closeModal}
                            sx={{
                                backgroundColor: red[500],
                            }}>
                            Закрыть
                        </Button>
                        <Button
                            onClick={() => {
                                closeModal();
                                changeUser();
                            }}
                            sx={{ backgroundColor: green[500] }}>
                            Сохранить
                        </Button>
                    </Box>
                </Box>
            </Dialog>
        </Box>
    );
};

export default UserElement;
