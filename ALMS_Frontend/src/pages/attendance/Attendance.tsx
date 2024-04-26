import { GridActionsCellItem, GridColDef, GridRenderCellParams, GridRowId } from "@mui/x-data-grid";
import { AutoCompleteField, NameWithAvatar, StatusComponent, Table } from "../../components/customTable";
import { useGetEmployeesListQuery } from "../../redux/api/employeeApi";
import dayjs from "dayjs";
import { Box, Checkbox, Container, FormControlLabel, Grid } from "@mui/material";
import { Link, useNavigate, useSearchParams } from "react-router-dom";
import { URL, replaceIdInURL } from "../../utils";
import EditIcon from "@mui/icons-material/Edit";
import { useGetAllAttendancesQuery } from "../../redux/api/attendanceApi";
import DatePickerField from "../../components/customTable/components/DatePickerField";
import { useAppSelector } from "../../redux/hooks/hooks";
import InfoIcon from "@mui/icons-material/Info";
import { useStatusHelperQuery } from "../../redux/api/leaveApi";
import { SyntheticEvent, useEffect, useState } from "react";

const Attendance = () => {
	const [searchParams] = useSearchParams();
	const navigate = useNavigate();
	const [isChecked, setIsChecked] = useState(false);
	const [getAttendanceQueryProps, setAttendanceQueryProps] = useState({
		...Object.fromEntries(searchParams.entries()),
	});
	const { data: statusList } = useStatusHelperQuery();
	const { data: employeeList } = useGetEmployeesListQuery();
	const userRole = useAppSelector((state) => state.auth.userData?.role);
	const columns: GridColDef[] = [
		{
			field: "id",
			headerName: "Id",
		},
		{
			field: "name",
			headerName: "Full Name",
			renderCell: (params: GridRenderCellParams) => {
				return (
					<Link
						to={replaceIdInURL(URL.DETAILS, params.row.attendanceId)}
						style={{ textDecoration: "none", color: "black" }}
					>
						<NameWithAvatar name={params.row.name} />
					</Link>
				);
			},
			width: 250,
		},
		{
			field: "date",
			headerName: "Date",
			renderCell: ({ value }: GridRenderCellParams) => dayjs(value).format("DD/MM/YYYY"),
			width: 250,
		},
		{
			field: "workingHours",
			headerName: "Working Time",
			width: 150,
		},
		{
			field: "status",
			headerName: "Status",
			renderCell: (params: GridRenderCellParams) => <StatusComponent {...params} />,
			width: 150,
		},
		{
			field: "action",
			type: "action",
			headerName: "Actions",
			width: 100,
			renderCell: (params) => (
				<>
					<GridActionsCellItem
						icon={<EditIcon />}
						label='Edit'
						className='textPrimary'
						onClick={() => handleEdit(params.row.attendanceId)}
					/>
					<GridActionsCellItem
						icon={<InfoIcon />}
						label='Edit'
						className='textPrimary'
						onClick={() => navigate(replaceIdInURL(URL.DETAILS, params.row.attendanceId))}
					/>
				</>
			),
		},
	];

	const handleEdit = (id: GridRowId) => {
		navigate(replaceIdInURL(URL.EDIT, id));
	};

	const handleCheckbox = (_: SyntheticEvent, isChecked: boolean) => {
		isChecked ? setIsChecked(true) : setIsChecked(false);
	};

	useEffect(() => {
		if (isChecked)
			setAttendanceQueryProps({
				managerAttendance: true,
				fromDate: dayjs(Date.now()).format("YYYY/MM/DD"),
				toDate: dayjs(Date.now()).format("YYYY/MM/DD"),
				...Object.fromEntries(searchParams.entries()),
			});
		else {
			setAttendanceQueryProps({
				fromDate: dayjs(Date.now()).format("YYYY/MM/DD"),
				toDate: dayjs(Date.now()).format("YYYY/MM/DD"),
				...Object.fromEntries(searchParams.entries()),
			});
		}
	}, [isChecked, searchParams]);

	const { data: attendanceData } = useGetAllAttendancesQuery(getAttendanceQueryProps);

	const pageInfo: DynamicTable.TableProps = {
		columns: columns,
		rows: attendanceData?.data.attendaces,
		rowCount: attendanceData?.data.count,
	};

	return (
		<Container maxWidth='xl'>
			<Box mb={4} display='flex' justifyContent='space-between' alignItems='center'>
				{/* <Typography variant='h5' color='initial'>
					Employees
				</Typography>
				<Box>
					<Button variant='contained' onClick={() => navigate(URL.ADD)}>
						Add
					</Button>
				</Box> */}
			</Box>
			{userRole === "Manager" && (
				<FormControlLabel control={<Checkbox />} label='My Attendance' onChange={handleCheckbox} />
			)}
			<Table {...pageInfo}>
				<Grid container spacing={2} paddingBottom={2}>
					<Grid item xs={6} md={3}>
						<AutoCompleteField options={employeeList?.data || []} label='employee' multiple renderIcon />
					</Grid>
					<Grid item xs={6} md={3}>
						<AutoCompleteField options={statusList?.data || []} label='status' multiple />
					</Grid>
					<Grid item xs={6} md={3}>
						<DatePickerField label='From' page='Attendance' />
					</Grid>
					<Grid item xs={6} md={3}>
						<DatePickerField to label='To' page='Attendance' />
					</Grid>
				</Grid>
			</Table>
		</Container>
	);
};

export default Attendance;
