import { GridActionsCellItem, GridColDef, GridRenderCellParams, GridRowId } from "@mui/x-data-grid";
import { AutoCompleteField, NameWithAvatar, Table } from "../../components/customTable";
import {
	useDeleteEmployeeMutation,
	useGetEmployeesListQuery,
	useGetEmployeesQuery,
	useGetManagerListQuery,
} from "../../redux/api/employeeApi";
import dayjs from "dayjs";
import capitalizeFirstLetter from "../../utils/helperFunctions/capitalizeFirstLetter";
import { Box, Button, Container, Grid, Typography } from "@mui/material";
import { Link, useNavigate, useSearchParams } from "react-router-dom";
import { URL, replaceIdInURL } from "../../utils";
import DeleteIcon from "@mui/icons-material/Delete";
import EditIcon from "@mui/icons-material/Edit";
import { useGetProjectListQuery } from "../../redux/api/projectApi";
import { useConfirm } from "material-ui-confirm";
import { useAppSelector } from "../../redux/hooks/hooks";
import { useEffect } from "react";
import { openSnackbar } from "../../redux/slice/snackbarSlice";
import { useDispatch } from "react-redux";

const Employees = () => {
	const [searchParams] = useSearchParams();
	const navigate = useNavigate();
	const { data } = useGetEmployeesQuery({ ...Object.fromEntries(searchParams.entries()) });
	const { data: employeeDD } = useGetEmployeesListQuery();
	const { data: managerDD } = useGetManagerListQuery();
	const { data: projectDD } = useGetProjectListQuery();

	const userRole = useAppSelector((state) => state.auth.userData?.role);

	const columns: GridColDef[] = [
		{
			field: "id",
			headerName: "Id",
		},
		{
			field: "userName",
			headerName: "User Name",
		},
		{
			field: "name",
			headerName: "Full Name",
			renderCell: (params: GridRenderCellParams) => {
				return (
					<Link
						to={replaceIdInURL(URL.DETAILS, params.row.employeeId)}
						style={{ textDecoration: "none", color: "black" }}
					>
						<NameWithAvatar name={params.row.name} />
					</Link>
				);
			},
			width: 250,
		},
		{
			field: "phoneNumber",
			headerName: "Mobile No.",
			width: 150,
		},
		{
			field: "dateOfJoin",
			headerName: "Joining Date",
			renderCell: ({ value }: GridRenderCellParams) => dayjs(value).format("DD/MM/YYYY"),
			width: 150,
		},
		{
			field: "role",
			headerName: "Role",
			renderCell: ({ value }: GridRenderCellParams) => capitalizeFirstLetter(value),
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
						onClick={() => handleEdit(params.row.employeeId)}
					/>
					<GridActionsCellItem icon={<DeleteIcon />} label='Delete' onClick={() => handleDelete(params)} />
				</>
			),
		},
	];

	const [deleteEmp, { data: empDeleteResponse, error: empDeleteError }] = useDeleteEmployeeMutation();
	const dispatch = useDispatch();

	useEffect(() => {
		if (empDeleteResponse) {
			dispatch(
				openSnackbar({
					severity: "success",
					message: empDeleteResponse?.message,
				}),
			);
		}
		if (empDeleteError) {
			dispatch(
				openSnackbar({
					severity: "error",
					message: empDeleteError?.data?.message,
				}),
			);
		}
	}, [empDeleteResponse, empDeleteError?.data]);

	const handleEdit = (id: GridRowId) => {
		navigate(replaceIdInURL(URL.EDIT, id));
	};

	const confirm = useConfirm();
	const handleDelete = (params: GridRenderCellParams) => {
		confirm({
			description: `This will permanently delete employee: ${params.row.name}.`,
		})
			.then(() => deleteEmp(params.row.employeeId))
			.catch(() => console.log("Deletion cancelled."));
	};

	const pageInfo: DynamicTable.TableProps = {
		columns: columns,
		rows: data?.data.employees,
		rowCount: data?.data.count,
	};

	return (
		<>
			<Container maxWidth='xl'>
				<Box mb={4} display='flex' justifyContent='space-between' alignItems='center'>
					<Typography variant='h5' color='initial'>
						Employees
					</Typography>
					{userRole === "Admin" ? (
						<Box>
							<Button variant='contained' onClick={() => navigate(URL.ADD)}>
								Add
							</Button>
						</Box>
					) : null}
				</Box>
				<Table {...pageInfo}>
					<Box sx={{ paddingBottom: 2, display: "flex", justifyContent: "space-between", gap: "10px" }}>
						{userRole !== "Employee" && (
							<Box sx={{ width: "100%" }}>
								<AutoCompleteField
									options={employeeDD?.data || []}
									label='employee'
									multiple
									renderIcon
								/>
							</Box>
						)}
						{userRole === "Admin" && (
							<Box sx={{ width: "100%" }}>
								<AutoCompleteField options={managerDD?.data || []} label='manager' renderIcon />
							</Box>
						)}
						<Box sx={{ width: "100%" }}>
							<AutoCompleteField options={projectDD?.data || []} label='project' />
						</Box>
					</Box>
				</Table>
			</Container>
		</>
	);
};

export default Employees;
