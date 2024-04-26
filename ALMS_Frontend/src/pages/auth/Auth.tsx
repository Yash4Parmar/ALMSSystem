import { Box, Container, Paper, Typography } from "@mui/material";
import DynamicForm from "../../components/dynamicForm/DynamicForm";
import { URL, loginForm } from "../../utils";
import { useLoginMutation } from "../../redux/api/authApi";
import { useNavigate } from "react-router-dom";
import { useAppDispatch } from "../../redux/hooks/hooks";
import { login } from "../../redux/slice/authSlice";
import { useEffect } from "react";
import { openSnackbar } from "../../redux/slice/snackbarSlice";
import { SnackBarComponent } from "../../components/common";

const Auth = () => {
	const [authLogin, { data: loginData, error: loginError }] = useLoginMutation();
	const dispatch = useAppDispatch();
	const navigate = useNavigate();

	useEffect(() => {
		if (loginData) {
			dispatch(login(loginData));

			dispatch(
				openSnackbar({
					severity: "success",
					message: "Logged in successfully.",
				}),
			);
			navigate(URL.HOME);
		}
	}, [loginData]);

	// useEffect(() => {
	// 	console.log(loginError);
	// }, [loginError]);

	const handleSubmit = (data: Record<string, string | number>) => {
		authLogin(data);
	};

	return (
		<Container sx={{ width: "500px", display: "flex", alignItems: "center", minHeight: "100vh" }}>
			<Paper sx={{ p: 4 }} elevation={3}>
				<Box>
					<Typography variant='h5' sx={{ width: "fit-content", m: "auto", mb: 1 }}>
						Login
					</Typography>
					<Typography variant='body1' sx={{ width: "fit-content", m: "auto", mb: 2 }}>
						Access to your dashboard
					</Typography>
					{loginError && ( // Render error message if authError exists
						<Box sx={{ color: "error.main", mb: 2, textAlign: "center" }}>{loginError?.data?.message}</Box>
					)}
				</Box>
				<Box>
					<DynamicForm
						fields={loginForm.fields}
						onSubmit={handleSubmit}
						defaultValues={loginForm.defaultValues}
						schema={loginForm.validationSchema}
					/>
				</Box>
			</Paper>
		</Container>
	);
};

export default Auth;
