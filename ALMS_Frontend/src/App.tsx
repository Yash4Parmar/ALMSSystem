import { CssBaseline, ThemeProvider } from "@mui/material";
import AppRouter from "./routes/AppRouter";
import theme from "./theme";
import { Provider } from "react-redux";
import { store } from "./redux/store";
import { ConfirmProvider } from "material-ui-confirm";
import { SnackBarComponent } from "./components/common";

const App = () => {
	return (
		<Provider store={store}>
			<ConfirmProvider>
				<ThemeProvider theme={theme}>
					<CssBaseline />
					<AppRouter />
					<SnackBarComponent />
				</ThemeProvider>
			</ConfirmProvider>
		</Provider>
	);
};

export default App;
