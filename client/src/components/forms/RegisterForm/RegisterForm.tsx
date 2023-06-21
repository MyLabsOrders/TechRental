import { useState, ChangeEvent, FormEvent } from "react";
import { Button, TextField, Typography, Box } from "@mui/material";
import { Link, useNavigate } from "react-router-dom";
import { green } from "@mui/material/colors";
import { login, register } from "../../../lib/identity/identity";
import { Notification } from "../../../features";
import { setCookie } from "typescript-cookie";
import { createUserProfile } from "../../../lib/users/users";

export interface RegisterFormProps {
    isModal?: boolean;
    oncloseCallback?: () => void;
}

const RegisterForm = ({
    oncloseCallback,
    isModal = false,
}: RegisterFormProps) => {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [firstName, setFirstname] = useState("");
    const [middleName, setMiddlename] = useState("");
    const [phoneNumber, setPhoneNumber] = useState("");
    const [birthDate, setBirthdate] = useState("");
    const [error, setError] = useState<string | null>(null);
    const navigate = useNavigate();

    const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
        event.preventDefault();

        try {
            setError(null);
            await register({
                username,
                password,
                roleName: "user",
            });

            const loginResponse = await login({ username, password });
            const jwtToken = loginResponse.data.token;
            const userId = loginResponse.data.userId;

            await createUserProfile(jwtToken, userId, {
                firstName,
                middleName,
                lastName: "",
                birthDate,
                phoneNumber,
                userImage: null,
				gender: "male"
            });

            setCookie("jwt-authorization", jwtToken);
			setCookie("current-user", userId);
			setCookie("current-username", username);
			
            if (!isModal)
                navigate("/profile", {
                    state: {
                        message: "Successfully registered!",
                        type: "success",
                    },
                });
            if (oncloseCallback) oncloseCallback();
        } catch (error: any) {
            setError(error?.response?.data?.Detailes);
        }
    };

    const handleUsernameChange = (event: ChangeEvent<HTMLInputElement>) => {
        setUsername(event.target.value);
    };

    const handlePasswordChange = (event: ChangeEvent<HTMLInputElement>) => {
        setPassword(event.target.value);
    };

    const handleFirstnameChange = (event: ChangeEvent<HTMLInputElement>) => {
        setFirstname(event.target.value);
    };

    const handleMiddlenameChange = (event: ChangeEvent<HTMLInputElement>) => {
        setMiddlename(event.target.value);
    };

	const handlePhonenumberChange = (event: ChangeEvent<HTMLInputElement>) => {
        setPhoneNumber(event.target.value);
    };

    const handleBirthdateChange = (event: ChangeEvent<HTMLInputElement>) => {
        setBirthdate(event.target.value);
    };

    return (
        <>
            {error && <Notification message={error} type="error" />}
            <Box sx={{ display: "flex", justifyContent: "center" }}>
                <Box
                    component="form"
                    autoComplete="off"
                    onSubmit={handleSubmit}
                    sx={{
                        display: "flex",
                        flexDirection: "column",
                        alignItems: "center",
                    }}>
                    <Typography variant="h4" sx={{ mb: 3, color: "white" }}>
                        Регистрация
                    </Typography>
                    <TextField
                        label="Логин"
                        onChange={handleUsernameChange}
                        required
                        variant="outlined"
                        color="primary"
                        type="text"
                        sx={{
                            mb: 3,
                            backgroundColor: "white",
                            borderRadius: 1,
                            "& .MuiOutlinedInput-root.Mui-focused .MuiOutlinedInput-notchedOutline":
                                {
                                    borderColor: "white",
                                },
                            "& .MuiInputLabel-root.Mui-focused": {
                                color: "white",
                                textShadow: "0 2px grey",
                            },
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
                            mb: 3,
                            backgroundColor: "white",
                            borderRadius: 1,
                            "& .MuiOutlinedInput-root.Mui-focused .MuiOutlinedInput-notchedOutline":
                                {
                                    borderColor: "white",
                                },
                            "& .MuiInputLabel-root.Mui-focused": {
                                color: "white",
                                textShadow: "0 2px grey",
                            },
                        }}
                        value={password}
                    />
                    <TextField
                        label="Имя"
                        onChange={handleFirstnameChange}
                        required
                        variant="outlined"
                        color="primary"
                        type="text"
                        sx={{
                            mb: 3,
                            backgroundColor: "white",
                            borderRadius: 1,
                            "& .MuiOutlinedInput-root.Mui-focused .MuiOutlinedInput-notchedOutline":
                                {
                                    borderColor: "white",
                                },
                            "& .MuiInputLabel-root.Mui-focused": {
                                color: "white",
                                textShadow: "0 2px grey",
                            },
                        }}
                        value={firstName}
                    />
                    <TextField
                        label="Фамилия"
                        onChange={handleMiddlenameChange}
                        required
                        variant="outlined"
                        color="primary"
                        type="text"
                        sx={{
                            mb: 3,
                            backgroundColor: "white",
                            borderRadius: 1,
                            "& .MuiOutlinedInput-root.Mui-focused .MuiOutlinedInput-notchedOutline":
                                {
                                    borderColor: "white",
                                },
                            "& .MuiInputLabel-root.Mui-focused": {
                                color: "white",
                                textShadow: "0 2px grey",
                            },
                        }}
                        value={middleName}
                    />
					<TextField
                        label="Телефон"
                        onChange={handlePhonenumberChange}
                        required
                        variant="outlined"
                        color="primary"
                        type="tel"
                        sx={{
                            mb: 3,
                            backgroundColor: "white",
                            borderRadius: 1,
                            "& .MuiOutlinedInput-root.Mui-focused .MuiOutlinedInput-notchedOutline":
                                {
                                    borderColor: "white",
                                },
                            "& .MuiInputLabel-root.Mui-focused": {
                                color: "white",
                                textShadow: "0 2px grey",
                            },
                        }}
                        value={phoneNumber}
                    />
                    <TextField
                        label=""
                        onChange={handleBirthdateChange}
                        required
                        variant="outlined"
                        color="primary"
                        type="date"
                        sx={{
                            mb: 3,
                            backgroundColor: "white",
                            borderRadius: 1,
                            "& .MuiOutlinedInput-root.Mui-focused .MuiOutlinedInput-notchedOutline":
                                {
                                    borderColor: "white",
                                },
                            "& .MuiInputLabel-root.Mui-focused": {
                                color: "white",
                                textShadow: "0 2px grey",
                            },
                        }}
                        value={birthDate}
                    />
                    <Button
                        variant="contained"
                        color="primary"
                        type="submit"
                        sx={{
                            backgroundColor: "inherit",
                            "&:hover": {
                                backgroundColor: green[500],
                                color: "white",
                            },
                        }}>
                        Зарегистрироваться
                    </Button>
                    {!isModal && (
                        <Typography
                            variant="body2"
                            sx={{ mt: 2, color: "white" }}>
                            Уже зарегистрированы?{" "}
                            <Link to="/login">Войти здесь</Link>
                        </Typography>
                    )}
                </Box>
            </Box>
        </>
    );
};

export default RegisterForm;
