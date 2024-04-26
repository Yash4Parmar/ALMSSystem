import { GridActionsCellItem, GridColDef, GridRenderCellParams, GridRowId } from "@mui/x-data-grid";
import { AutoCompleteField, Table } from "../../components/customTable";
import { useGetProjectsQuery, useGetProjectListQuery, useDeleteProjectMutation } from "../../redux/api/projectApi";
import dayjs from "dayjs";
import { Box, Button, Container, Grid, Typography } from "@mui/material";
import { Link, useNavigate, useSearchParams } from "react-router-dom";
import { URL } from "../../utils";
import DeleteIcon from "@mui/icons-material/Delete";
import EditIcon from "@mui/icons-material/Edit";
import { useGetEmployeesListQuery } from "../../redux/api/employeeApi";
import { useConfirm } from "material-ui-confirm";
import { useAppDispatch, useAppSelector } from "../../redux/hooks/hooks";
import AllProjectsDetails from "./AllProjectsDetails";
import { useEffect } from "react";
import { openSnackbar } from "../../redux/slice/snackbarSlice";

const Project = () => {
	const [searchParams] = useSearchParams();
	const navigate = useNavigate();
	const { data } = useGetProjectsQuery({ ...Object.fromEntries(searchParams.entries()) });
	const { data: projectDD } = useGetProjectListQuery();
	const { data: employeeDD } = useGetEmployeesListQuery();

	const userRole = useAppSelector((state) => state.auth.userData?.role);

	const columns: GridColDef[] = [
		{
			field: "id",
			headerName: "Id",
		},
		{
			field: "name",
			headerName: "Project Name",
			renderCell: (params: GridRenderCellParams) => {
				return (
					<Link
						to={`/projects/details/${params.row.projectId}`}
						style={{ textDecoration: "none", color: "black" }}
					>
						{params.row.name}
					</Link>
				);
			},
			width: 250,
		},
		{
			field: "startDate",
			headerName: " Start Date",
			renderCell: ({ value }: GridRenderCellParams) => dayjs(value).format("DD/MM/YYYY"),
		},
		{
			field: "endDate",
			headerName: "End Date",
			renderCell: ({ value }: GridRenderCellParams) => dayjs(value).format("DD/MM/YYYY"),
		},
		{
			field: "manager",
			headerName: "Manager",
		},
		{
			field: "actions",
			type: "actions",
			headerName: "Actions",
			width: 100,
			renderCell: (params) => (
				<>
					<GridActionsCellItem
						icon={<EditIcon />}
						label='Edit'
						className='textPrimary'
						onClick={() => handleEdit(params.row.projectId)}
					/>

					<GridActionsCellItem icon={<DeleteIcon />} label='Delete' onClick={() => handleDelete(params)} />
				</>
			),
		},
	];

	const [deleteProject, { data: deleteProjectResponse, error: deleteProjectError }] = useDeleteProjectMutation();
	const dispatch = useAppDispatch();
	useEffect(() => {
		if (deleteProjectResponse) {
			dispatch(
				openSnackbar({
					severity: "success",
					message: deleteProjectResponse?.message,
				}),
			);
		}
		if (deleteProjectError) {
			dispatch(
				openSnackbar({
					severity: "error",
					message: deleteProjectError?.data?.message,
				}),
			);
			console.log("error");
		}
	}, [deleteProjectResponse, deleteProjectError?.data]);

	const handleEdit = (id: GridRowId) => {
		navigate(`/projects/edit/${id}`);
	};

	const confirm = useConfirm();
	const handleDelete = (params: GridRenderCellParams) => {
		console.log(params);
		confirm({
			description: `This will permanently delete project: ${params.row.name}.`,
		})
			.then(() => deleteProject(params.row.projectId))
			.catch(() => console.log("Deletion cancelled."));
	};

	const pageInfo: DynamicTable.TableProps = {
		columns: columns,
		rows: data?.data.projects,
		rowCount: data?.data.count,
	};
	if (userRole === "Employee") {
		return <AllProjectsDetails />;
	} else {
		return (
			<Container maxWidth='xl'>

				<Box>
					<Box mb={4} display='flex' justifyContent='space-between' alignItems='center'>
						<Typography variant='h5' color='initial'>
							Projects
						</Typography>
						{userRole === "Admin" ? (
							<Button variant='contained' onClick={() => navigate(URL.ADD)}>
								Add
							</Button>
						) : null}
					</Box>
					<Table {...pageInfo}>
						<Grid container spacing={2} paddingBottom={2}>
							<Grid item xs={6}>
								<AutoCompleteField
									options={employeeDD?.data || []}
									label='employee'
									multiple
									renderIcon
								/>
							</Grid>
							<Grid item xs={6}>
								<AutoCompleteField options={projectDD?.data || []} label='project' />
							</Grid>
						</Grid>
					</Table>
				</Box>
			</Container>
		);
	}
};

export default Project;
