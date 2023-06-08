import { Box, Container } from "@mui/material";
import { IUser } from "../../shared";
import UserElement from "./userelement";

export interface UserListProps {
    users: IUser[];
}

const UserListComponent = ({ users }: UserListProps) => {
    return (
        <Container
            sx={{
                display: "flex",
                justifyContent: "center",
                alignItems: "center",
            }}>
            <Box width={"100%"}>
                {users.map((user,i) => (
                    <UserElement key={i} user={user}/>
                ))}
            </Box>
        </Container>
    );
};

export default UserListComponent;

