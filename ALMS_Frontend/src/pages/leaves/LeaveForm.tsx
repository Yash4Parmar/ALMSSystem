import { Container, IconButton, Typography, Paper } from "@mui/material";
import Box from "@mui/material/Box";
import { KeyboardBackspace } from "@mui/icons-material";
import { useNavigate, useParams } from "react-router-dom";
import { URL } from "../../utils";
import DynamicForm from "../../components/dynamicForm/DynamicForm";
import dayjs from "dayjs";
import { addLeave } from "../../utils/constants/formConstant";
import { useEffect, useState } from "react";
import { setLeaveFormSchema } from "../../utils/helperFunctions/setFormSchema";
import { useGetEmployeesListQuery } from "../../redux/api/employeeApi";
import {
	useAddLeaveMutation,
	useGetLeavesByIdQuery,
	useLeaveHelperQuery,
	useStatusHelperQuery,
	useUpdateLeaveMutation,
} from "../../redux/api/leaveApi";
import { useAppDispatch, useAppSelector } from "../../redux/hooks/hooks";
import { openSnackbar } from "../../redux/slice/snackbarSlice";

const LeaveForm = () => {
	const navigate = useNavigate();
	const dispatch = useAppDispatch();

	const { id } = useParams();
	const { data: employeeList } = useGetEmployeesListQuery();
	const { data: statusList } = useStatusHelperQuery();
	const { data: leaveTypeList } = useLeaveHelperQuery();
	const { data: leave } = useGetLeavesByIdQuery(id ? id : "");
	const userData = useAppSelector((state) => state.auth.userData);
	const role = userData?.role;

	const [filteredSchema, setFilteredSchema] = useState<any>();
	console.log(leave);
	const [createLeave, { data: addLeaveResponse, error: addLeaveError }] = useAddLeaveMutation();
	const [editLeave, { data: editLeaveResponse, error: editLeaveError }] = useUpdateLeaveMutation();

	useEffect(() => {
		console.log(addLeaveError, addLeaveResponse);
		if (addLeaveResponse) {
			dispatch(
				openSnackbar({
					severity: "success",
					message: addLeaveResponse.message,
				}),
			);
			navigate("/leaves");
		}
		if (addLeaveError) {
			dispatch(
				openSnackbar({
					severity: "error",
					message: addLeaveError?.data?.message,
				}),
			);
			console.log("error"); /// toast
		}
	}, [addLeaveResponse, addLeaveError?.data]);

	useEffect(() => {
		console.log(editLeaveError, editLeaveResponse);
		if (editLeaveResponse) {
			dispatch(
				openSnackbar({
					severity: "success",
					message: editLeaveResponse?.message,
				}),
			);
			navigate("/leaves");
		}
		if (editLeaveError) {
			dispatch(
				openSnackbar({
					severity: "error",
					message: editLeaveError?.data?.message,
				}),
			);
			console.log("error");
		}
	}, [editLeaveResponse, editLeaveError?.data]);

	const handleSubmit = (data: Record<string, any>) => {
		//console.log(data);
		const formattedData = {
			...data,
			startDate: dayjs(data.startDate).format("YYYY-MM-DD"),
			endDate: dayjs(data.endDate).format("YYYY-MM-DD"),
			employeeId: role === "Employee" ? userData?.id : data.employeeId,
			statusId: role === "Employee" ? 1 : data.statusId,
		};
		console.log(formattedData);

		id ? editLeave({ data: formattedData, id: id }) : createLeave({ data: formattedData });

		//createLeave({ data: formattedData });
	};

	useEffect(() => {
		if (statusList && leaveTypeList && employeeList) {
			let schema;
			if (id) {
				if (leave) {
					console.log("edit");
					schema = setLeaveFormSchema(addLeave, {
						options: [
							{ employeeId: employeeList.data },
							{ statusId: statusList.data },
							{ leaveTypeId: leaveTypeList.data },
						],
						initialValues: leave.data,
						role: role,
					});
				}
			} else {
				console.log("add");
				schema = setLeaveFormSchema(addLeave, {
					options: [
						{ employeeId: employeeList.data },
						{ statusId: statusList.data },
						{ leaveTypeId: leaveTypeList.data },
					],
					initialValues: addLeave.defaultValues,
					role: role,
				});
			}

			//console.log(schema);

			setFilteredSchema(schema);
		}
	}, [statusList, employeeList, leave, id]);

	return (
		<Container maxWidth='xl'>
			<Box display='flex' gap={2} alignItems='center' mb={2}>
				<IconButton onClick={() => navigate(URL.LEAVES)}>
					<KeyboardBackspace />
				</IconButton>
				<Typography variant='h5' ml={-2}>
					{id ? "Edit" : "Add"} Leave
				</Typography>
			</Box>
			<Paper variant='elevation' sx={{ p: 2, maxWidth: "sm" }} elevation={2}>
				{filteredSchema && (
					<DynamicForm
						fields={filteredSchema.fields}
						onSubmit={handleSubmit}
						defaultValues={filteredSchema.defaultValues}
						schema={filteredSchema.validationSchema}
					/>
				)}
			</Paper>
		</Container>
	);
};

export default LeaveForm;
