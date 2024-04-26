import { Link, useNavigate, useSearchParams } from "react-router-dom";
import { useGetLeavesQuery, useLeaveHelperQuery, useStatusHelperQuery } from "../../redux/api/leaveApi";
import { GridActionsCellItem, GridColDef, GridRenderCellParams, GridRowId } from "@mui/x-data-grid";
import { Box, Button, Container, FormControlLabel, Grid, Typography } from "@mui/material";
import { AutoCompleteField, NameWithAvatar, StatusComponent, Table } from "../../components/customTable";
import { URL, replaceIdInURL } from "../../utils";
import dayjs from "dayjs";
import { useGetEmployeesListQuery, useGetManagerListQuery } from "../../redux/api/employeeApi";
import EditIcon from "@mui/icons-material/Edit";
import { DatePickerField } from "../../components/customTable/index";
import { useAppSelector } from "../../redux/hooks/hooks";
import { Checkbox } from "@mui/material";
import { SyntheticEvent, useEffect, useState } from "react";

const Leaves = () => {
	const [searchParams] = useSearchParams();
	const [isChecked, setIsChecked] = useState(false);
	const [getLeavesQueryProps, setLeavesQueryProps] = useState({ ...Object.fromEntries(searchParams.entries()) });
	const { data: employeeDD } = useGetEmployeesListQuery();
	const { data: managerDD } = useGetManagerListQuery();
	const { data: statusList } = useStatusHelperQuery();
	const { data: leaveTypeList } = useLeaveHelperQuery();

	const userData = useAppSelector((state) => state.auth.userData);
	const userRole = userData?.role;

	const navigate = useNavigate();
	const columns: GridColDef[] = [
		{
			field: "id",
			headerName: "Id",
		},
		{
			field: "name",
			headerName: "Employee Name",
			renderCell: (params: GridRenderCellParams) => {
				return (
					<Link
						to={replaceIdInURL("/employees/details/:id", params.row.employeeId)}
						style={{ textDecoration: "none", color: "black" }}
					>
						<NameWithAvatar name={params.row.name} />
					</Link>
				);
			},
			width: 150,
		},
		{
			field: "leaveTypeName",
			headerName: "Leave Type",
		},
		{
			field: "from",
			headerName: "From",
			renderCell: ({ value }: GridRenderCellParams) => dayjs(value).format("DD/MM/YYYY"),
			width: 120,
		},
		{
			field: "to",
			headerName: "To",
			renderCell: ({ value }: GridRenderCellParams) => dayjs(value).format("DD/MM/YYYY"),
			width: 120,
		},
		{
			field: "noOfDays",
			headerName: "Total Days",
		},
		{
			field: "reason",
			headerName: "Reason",
		},
		{
			field: "status",
			headerName: "Status",
			renderCell: (params: GridRenderCellParams) => <StatusComponent type='leave' {...params} />,
			width: 150,
		},
		{
			field: "leaveActions",
			headerName: "Actions",
			type: "actions",
			width: 100,
			renderCell: (params) => (
				<>
					{params.row.statusId === 2 ? (
						<></>
					) : (
						<GridActionsCellItem
							icon={<EditIcon />}
							label='Edit'
							className='textPrimary'
							onClick={() => handleEdit(params.row.leaveId)}
						/>
					)}
				</>
			),
		},
	];
	const handleEdit = (id: GridRowId) => {
		navigate(replaceIdInURL(URL.EDIT, id));
	};

	const handleCheckbox = (_: SyntheticEvent, isChecked: boolean) => {
		console.log(isChecked);
		isChecked ? setIsChecked(true) : setIsChecked(false);
	};

	useEffect(() => {
		if (isChecked)
			setLeavesQueryProps({
				managerLeave: true,
				// fromDate: dayjs(Date.now()).format("YYYY/MM/DD"),
				// toDate: dayjs(Date.now()).format("YYYY/MM/DD"),
				...Object.fromEntries(searchParams.entries()),
			});
		else {
			setLeavesQueryProps({
				// fromDate: dayjs(Date.now()).format("YYYY/MM/DD"),
				// toDate: dayjs(Date.now()).format("YYYY/MM/DD"),
				...Object.fromEntries(searchParams.entries()),
			});
		}
	}, [isChecked, searchParams]);

	const { data } = useGetLeavesQuery(getLeavesQueryProps);

	const pageInfo: DynamicTable.TableProps = {
		columns: columns,
		rows: data?.data.leaves,
		rowCount: data?.data.count,
	};

	return (
		<>
			<Container maxWidth='xl'>
				<Box mb={4} display='flex' justifyContent='space-between' alignItems='center'>
					<Typography variant='h5' color='initial'>
						Leaves
					</Typography>
					<Box>
						<Button variant='contained' onClick={() => navigate(URL.ADD)}>
							Add
						</Button>
					</Box>
				</Box>
				{userRole === "Manager" && (
					<FormControlLabel control={<Checkbox />} label='My leaves' onChange={handleCheckbox} />
				)}
				<Table {...pageInfo}>
					<Grid container spacing={2} paddingBottom={2}>
						{isChecked ||
							(userRole !== "Employee" && (
								<>
									{" "}
									<Grid item xs={6} md={3}>
										<AutoCompleteField options={employeeDD?.data || []} label='employee' multiple />
									</Grid>
									<Grid item xs={6} md={3}>
										<AutoCompleteField options={managerDD?.data || []} label='manager' multiple />
									</Grid>
								</>
							))}
						<Grid item xs={6} md={3}>
							<AutoCompleteField options={leaveTypeList?.data || []} label='leaveType' multiple />
						</Grid>
						<Grid item xs={6} md={3}>
							<AutoCompleteField options={statusList?.data || []} label='status' multiple />
						</Grid>
						<Grid item xs={6} md={3}>
							<DatePickerField label='From' />
						</Grid>
						<Grid item xs={6} md={3}>
							<DatePickerField to label='To' />
						</Grid>
					</Grid>
				</Table>
			</Container>
		</>
	);
};

export default Leaves;
