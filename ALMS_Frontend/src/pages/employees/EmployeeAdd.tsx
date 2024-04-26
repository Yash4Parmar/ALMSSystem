import { Container, IconButton, Typography, Paper } from "@mui/material";
import Box from "@mui/material/Box";
import { KeyboardBackspace } from "@mui/icons-material";
import { useNavigate } from "react-router-dom";
import { URL, createEmployee } from "../../utils";
import DynamicForm from "../../components/dynamicForm/DynamicForm";
import { useRegisterMutation } from "../../redux/api/authApi";
import { useEffect } from "react";
import { useAppDispatch } from "../../redux/hooks/hooks";
import { openSnackbar } from "../../redux/slice/snackbarSlice";

const EmployeeForm = () => {
	const navigate = useNavigate();
	const dispatch = useAppDispatch();
	const [createEmp, { data: createEmpResponse, error: createEmpError }] = useRegisterMutation();

	useEffect(() => {
		if (createEmpResponse) {
			dispatch(
				openSnackbar({
					severity: "success",
					message: createEmpResponse?.message,
				}),
			);
			navigate("/employees");
		}
		if (createEmpError) {
			dispatch(
				openSnackbar({
					severity: "error",
					message: createEmpError?.data?.message,
				}),
			);
		}
	}, [createEmpResponse, createEmpError?.data]);

	const handleSubmit = (data: Record<string, string | number>) => {
		const formattedData = {
			...data,
			// dateOfJoin: dayjs(data.dateOfJoin).format("YYYY-MM-DD"),
		};
		createEmp(formattedData);
	};

	return (
		<Container maxWidth='xl'>
			<Box display='flex' gap={2} alignItems='center' mb={2}>
				<IconButton onClick={() => navigate(URL.EMPLOYEES)}>
					<KeyboardBackspace />
				</IconButton>
				<Typography variant='h5' ml={-2}>
					Add Employee
				</Typography>
			</Box>
			<Paper variant='elevation' sx={{ p: 2, maxWidth: "sm" }} elevation={2}>
				<DynamicForm
					fields={createEmployee.fields}
					onSubmit={handleSubmit}
					defaultValues={createEmployee.defaultValues}
					schema={createEmployee.validationSchema}
				/>
			</Paper>
		</Container>
	);
};

export default EmployeeForm;
