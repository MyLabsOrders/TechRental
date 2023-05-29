import { Box, Container } from "@mui/material";
import { IUser } from "../../shared";
import UserElement from "./userelement";

export interface UserListProps {
    users: IUser[];
    deleteCallback: (user: IUser) => {};
}

const UserListComponent = ({ users, deleteCallback }: UserListProps) => {
    return (
        <Container
            sx={{
                display: "flex",
                justifyContent: "center",
                alignItems: "center",
            }}>
            <Box width={"100%"}>
                {users.map((user) => (
                    <UserElement user={user} deleteCallback={deleteCallback} />
                ))}
            </Box>
        </Container>
    );
};

export default UserListComponent;

