import { Box, CircularProgress, Container, IconButton, Paper, Typography } from "@mui/material";
import { URL } from "../../utils";
import { KeyboardBackspace } from "@mui/icons-material";
import { useNavigate, useParams } from "react-router-dom";
import { editEmployee } from "../../utils/constants/formConstant";
import DynamicForm from "../../components/dynamicForm/DynamicForm";
import {
	useEditEmployeeMutation,
	useGetEmployeesByIdQuery,
	useGetEmployeesListQuery,
	useGetManagerListQuery,
	useGetOnlyEmployeesQuery,
} from "../../redux/api/employeeApi";
import dayjs from "dayjs";
import { useAppDispatch, useAppSelector } from "../../redux/hooks/hooks";
import { useEffect, useState } from "react";
import { useGetProjectListQuery } from "../../redux/api/projectApi";
import { setEmployeeFormSchema, setFormSchema } from "../../utils/helperFunctions/setFormSchema";
import { openSnackbar } from "../../redux/slice/snackbarSlice";

const EmployeeEdit = () => {
	const navigate = useNavigate();
	const dispatch = useAppDispatch();
	const { id } = useParams();

	const [detailId, setDetailId] = useState(id);
	const { data: employeeList } = useGetOnlyEmployeesQuery();
	const { data: managerList } = useGetManagerListQuery();
	const { data: projectList } = useGetProjectListQuery();
	const { data: employee, isLoading: isEmployeeLoading } = useGetEmployeesByIdQuery(id);
	const userData = useAppSelector((state) => state.auth.userData);
	const role = userData?.role;
	const [filteredSchema, setFilteredSchema] = useState<any>();
	const [editEmp, { data: editResponse, error: editError }] = useEditEmployeeMutation();

	useEffect(() => {
		if (editResponse) {
			dispatch(
				openSnackbar({
					severity: "success",
					message: editResponse.message,
				}),
			);
			handleNavigate();
		}
		if (editError) {
			dispatch(
				openSnackbar({
					severity: "error",
					message: editError?.data?.message,
				}),
			);
		}
	}, [editError, editResponse?.data]);

	const handleNavigate = () => {
		const url = window.location.pathname;
		url.includes("employees") ? navigate("/employees") : navigate("/profile");
	};

	useEffect(() => {
		if (managerList && employeeList && projectList && employee && role) {
			const schema = setEmployeeFormSchema(editEmployee, {
				options: [
					{ projectIds: projectList.data },
					{ employeeIds: employeeList.data },
					{ managerId: managerList.data },
				],
				initialValues: employee.data,
				role: role,
			});

			setFilteredSchema(schema);
		}
	}, [managerList, employeeList, projectList, employee, role]);

	useEffect(() => {
		if (id === ":id") {
			setDetailId(userData?.id as string);
			navigate(`/profile/edit/${userData?.id}`);
		}
		if (userData?.role === "Employee" && detailId !== ":id" && detailId !== userData?.id) {
			navigate("/");
		}
	}, []);

	const handleSubmit = (data: Record<string, unknown>) => {
		console.log(data);
		const formattedData = {
			...data,
			dob: data.dob && dayjs(data.dob).isValid() ? dayjs(data.dob).format("YYYY-MM-DD") : null,
			dateOfJoin:
				data.dateOfJoin && dayjs(data.dateOfJoin).isValid()
					? dayjs(data.dateOfJoin).format("YYYY-MM-DD")
					: null,
		};

		editEmp({ data: formattedData, id: id });
	};

	if (!filteredSchema) return <></>;

	console.log(filteredSchema, "sd");

	return (
		<Container maxWidth='xl'>
			<Box display='flex' gap={2} alignItems='center' mb={2}>
				<IconButton onClick={handleNavigate}>
					<KeyboardBackspace />
				</IconButton>
				<Typography variant='h5' ml={-2}>
					Edit Employee
				</Typography>
			</Box>
			<Paper variant='elevation' sx={{ p: 2, maxWidth: "lg" }} elevation={2}>
				{isEmployeeLoading ? (
					<Box display='flex' justifyContent='center'>
						<CircularProgress />
					</Box>
				) : (
					filteredSchema && (
						<DynamicForm
							fields={filteredSchema.fields}
							onSubmit={handleSubmit}
							defaultValues={filteredSchema.defaultValues}
							schema={filteredSchema.validationSchema}
						/>
					)
				)}
			</Paper>
		</Container>
	);
};

export default EmployeeEdit;
