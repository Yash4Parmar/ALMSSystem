import { Container, IconButton, Typography, Paper } from "@mui/material";
import Box from "@mui/material/Box";
import { KeyboardBackspace } from "@mui/icons-material";
import { useNavigate, useParams } from "react-router-dom";
import { URL } from "../../utils";
import DynamicForm from "../../components/dynamicForm/DynamicForm";
import dayjs from "dayjs";
import { addProject } from "../../utils/constants/formConstant";
import { useEffect, useState } from "react";
import { addOptions, setProjectFormSchema } from "../../utils/helperFunctions/setFormSchema";
import {
	useGetEmployeesListQuery,
	useGetManagerListQuery,
	useGetOnlyEmployeesQuery,
} from "../../redux/api/employeeApi";
import {
	useAddProjectMutation,
	useEditProjectMutation,
	useGetProjectByIdQuery,
	useUnAssignedManagerQuery,
} from "../../redux/api/projectApi";
import { useAppDispatch } from "../../redux/hooks/hooks";
import { openSnackbar } from "../../redux/slice/snackbarSlice";

const ProjectForm = () => {
	const navigate = useNavigate();
	const dispatch = useAppDispatch();

	const { id } = useParams();
	const { data: employeeList } = useGetOnlyEmployeesQuery();
	const { data: managerList } = useUnAssignedManagerQuery(id ? { projectId: id } : "");
	const { data: project } = useGetProjectByIdQuery(id ? id : "");

	const [filteredSchema, setFilteredSchema] = useState<any>();

	const [createProject, { data: addProjectResponse, error: addProjectError }] = useAddProjectMutation();
	const [editProject, { data: editProjectResponse, error: editProjectError }] = useEditProjectMutation();

	useEffect(() => {
		console.log(addProjectError, addProjectResponse);
		if (addProjectResponse) {
			dispatch(
				openSnackbar({
					severity: "success",
					message: addProjectResponse.message,
				}),
			);
			navigate("/projects");
		}
		if (addProjectError) {
			dispatch(
				openSnackbar({
					severity: "error",
					message: addProjectError?.data?.message,
				}),
			);

			console.log("error");
		}
	}, [addProjectResponse, addProjectError?.data]);

	useEffect(() => {
		console.log(editProjectError, editProjectResponse);
		if (editProjectResponse) {
			dispatch(
				openSnackbar({
					severity: "success",
					message: editProjectResponse.message,
				}),
			);
			navigate("/projects");
		}
		if (editProjectError) {
			dispatch(
				openSnackbar({
					severity: "error",
					message: editProjectError?.data?.message,
				}),
			);
			console.log("error");
		}
	}, [editProjectError, editProjectResponse?.data]);

	const handleSubmit = (data: Record<string, string | number>) => {
		const formattedData = {
			...data,
			startDate: dayjs(data.startDate).format("YYYY-MM-DD"),
			endDate: dayjs(data.endDate).format("YYYY-MM-DD"),
		};
		console.log(formattedData);

		id ? editProject({ data: formattedData, id: id }) : createProject(formattedData);
	};

	useEffect(() => {
		if (managerList && employeeList) {
			let schema;
			if (id) {
				if (project) {
					console.log("edit");
					schema = setProjectFormSchema(addProject, {
						options: [{ employeeIds: employeeList.data }, { managerId: managerList.data }],
						initialValues: project.data,
					});
				}
			} else {
				console.log("add");
				schema = addOptions(addProject, [{ employeeIds: employeeList.data }, { managerId: managerList.data }]);
			}

			console.log(schema);

			setFilteredSchema(schema);
		}
	}, [managerList, employeeList, project, id]);

	return (
		<Container maxWidth='xl'>
			<Box display='flex' gap={2} alignItems='center' mb={2}>
				<IconButton onClick={() => navigate(URL.PROJECT)}>
					<KeyboardBackspace />
				</IconButton>
				<Typography variant='h5' ml={-2}>
					{id ? "Edit" : "Add"} Project
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

export default ProjectForm;
