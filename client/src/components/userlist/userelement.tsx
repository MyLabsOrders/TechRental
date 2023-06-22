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
import { useCallback, useEffect, useState } from "react";
import {
    changeRole,
    getIdentityUser,
} from "../../lib/identity/identity";
import { getCookie } from "typescript-cookie";
import { changeUserProfile } from "../../lib/users/users";

export interface UserElementProps {
    user: IUser;
}

const UserElement = ({ user }: UserElementProps) => {
    const [role, setRole] = useState<string>("user");

    const [isDialogOpen, setDialogOpen] = useState<boolean>(false);
    const [username, setUsername] = useState<string>("");

    const [editedFirstName, setEditedFirstName] = useState(user.firstName);
    const [editedMiddleName, setEditedMiddleName] = useState(user.middleName);
    const [editedLastName, setEditedLastName] = useState(user.lastName);
    const [editedBirthDate, setEditedBirthDate] = useState(user.birthDate);
    
    const fetchIdentity = useCallback(async () => {
        try {
            const { data } = await getIdentityUser(
                getCookie("jwt-authorization") ?? "",
                user.id
            );
            setRole(data.role);
            setUsername(data.username);
        } catch (error) {}
    }, [user.id]);

    useEffect(() => {
        fetchIdentity();
    }, [fetchIdentity]);

    const changeUser = async () => {
        try {
            user.firstName = editedFirstName;
            user.middleName = editedMiddleName;
            user.lastName = editedLastName;
            user.birthDate = editedBirthDate;

            await changeUserProfile(
                getCookie("jwt-authorization") ?? "",
                user.id,
                {
                    identityId: user.id,
                    firstName: user.firstName,
                    middleName: user.middleName,
                    lastName: user.lastName,
                    birthDate: user.birthDate,
                    gender: user.gender?.toLowerCase() as ("male" | "female"),
                    phoneNumber: user.number,
                    userImage: user.image,
                    isActive: true
                }
            );
        } catch (error) {
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
        let _role = event.target.value;
        try {
            await changeRole(
                getCookie("jwt-authorization") ?? "",
                username,
                _role
            );
        } catch (error) {}
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
                <MenuItem value={"user"}>Польз.</MenuItem>
                <MenuItem value={"admin"}>Админ</MenuItem>
            </Select>
            <Box>
                <Button
                    onClick={openModal}
                    sx={{ backgroundColor: green[500], marginRight: "10px" }}>
                    Изменить
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
