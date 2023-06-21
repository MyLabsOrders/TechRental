import ReactDOM from "react-dom/client";
import App from "./App";
import { BrowserRouter } from "react-router-dom";
import "./styles/index.css";
import { createTheme, ThemeProvider } from "@mui/material/styles";
import { ruRU } from "@mui/material/locale";

const root = ReactDOM.createRoot(
    document.getElementById("root") as HTMLElement
);

const theme = createTheme(undefined, ruRU);

root.render(
    <BrowserRouter>
        <ThemeProvider theme={theme}>
            <App />
        </ThemeProvider>
    </BrowserRouter>
);

