import { Box } from "@mui/material";
import { useEffect, useState } from "react";
import { CreateProductForm } from "../../components";
import { IUser } from "../../shared";
import { getAllUsers } from "../../lib/users/users";
import { getCookie } from "typescript-cookie";
import { UserListComponent } from "../../components/userlist";
import { CreateUserForm } from "../../components/forms/CreateUserForm";

const AdminPage = () => {
    const [users, setUsers] = useState<IUser[]>([]);

    useEffect(() => {
        fetchUsers();
    },[]);

    const fetchUsers = async () => {
        try {
            const { data } = await getAllUsers(
                getCookie("jwt-authorization") ?? ""
            );
            setUsers(data.users);
        } catch (error) {
        }
    };

    return (
        <Box
            bgcolor="#132f4b"
            display={"flex"}
            alignItems={"center"}
            height={"100vh"}
            width={"100vw"}>
            <Box
                display={"flex"}
                flexDirection={"column"}
                justifyContent={"space-evenly"}
                alignItems={"center"}
                width={"80%"}
                marginLeft={"auto"}
                marginRight={"auto"}>
                <UserListComponent users={users}/>
                <Box marginTop={"10px"}>
                    <CreateProductForm />
                    <CreateUserForm onCloseCallback={fetchUsers}/>
                </Box>
            </Box>
        </Box>
    );
};

export default AdminPage;
