import { Box } from "@mui/material";
import { useEffect, useState } from "react";
import { CreateProductForm } from "../../components";
import { IProduct, IUser } from "../../shared";
import { getAllUsers } from "../../lib/users/users";
import { getCookie } from "typescript-cookie";
import { UserListComponent } from "../../components/userlist";
import { CreateUserForm } from "../../components/forms/CreateUserForm";

const createUser = (
    id: string,
    birthDate: string,
    firstName: string,
    image: string,
    lastName: string,
    middleName: string,
    money: number,
    number: string,
    orders: IProduct[]
): IUser => {
    return {
        id,
        birthDate,
        firstName,
        image,
        lastName,
        middleName,
        money,
        number,
        orders,
    };
};

const createUsers = (count: number) => {
    return new Array(count).fill(null).map((_, i) => {
        return createUser(
            `${i}`,
            `01.01.000${i}`,
            `Name${i}`,
            "https://loremflickr.com/640/360",
            `lastName${i}`,
            `middleName${i}`,
            1000 + i * 100,
            `number${i}`,
            []
        );
    });
};

const AdminPage = () => {
    const [users, setUsers] = useState<IUser[]>(createUsers(5));

    useEffect(() => {
        fetchUsers();
    });

    const fetchUsers = async () => {
        try {
            const { data } = await getAllUsers(
                getCookie("jwt-authorization") ?? ""
            );
            setUsers(data.users);
        } catch (error) {
            console.log(error);
        }
    };

    const deleteUser = async (user: IUser) => {
        /*
        try {
            const { data } = await deleteUser(
                getCookie("jwt-authorization") ?? "",
                id
            );
            const _users = users.splice(users.indexOf(user),1)
            setUsers(_users);
        } catch (error) {
            console.log(error);
        }
        */
        let _users = users;
        _users.splice(_users.indexOf(user), 1);
        setUsers([..._users]);
        console.log(`deleted user with id: ${user.id}`);
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
                <UserListComponent users={users} deleteCallback={deleteUser} />
                <Box marginTop={"10px"}>
                    <CreateProductForm />
                    <CreateUserForm onCloseCallback={fetchUsers}/>
                </Box>
            </Box>
        </Box>
    );
};

export default AdminPage;
