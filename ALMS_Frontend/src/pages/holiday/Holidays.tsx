import { GridActionsCellItem, GridColDef, GridRenderCellParams } from "@mui/x-data-grid";
import { Table } from "../../components/customTable";
import { useDeleteHolidayMutation, useGetHolidaysQuery } from "../../redux/api/holidayApi";
import dayjs from "dayjs";
import { Box, Button, CircularProgress, Container, Typography } from "@mui/material";
import { useConfirm } from "material-ui-confirm";
import DeleteIcon from "@mui/icons-material/Delete";
import { useAppDispatch, useAppSelector } from "../../redux/hooks/hooks";
import { useNavigate } from "react-router-dom";
import { URL, replaceIdInURL } from "../../utils";
import { openSnackbar } from "../../redux/slice/snackbarSlice";
import { useEffect } from "react";

const Holidays = () => {
	const { data, isLoading } = useGetHolidaysQuery();
	const [deleteHoliday, { data: deleteHolidayResponse, error: deleteHolidayError }] = useDeleteHolidayMutation();
	const userRole = useAppSelector((state) => state.auth.userData?.role);
	const navigate = useNavigate();
	const dispatch = useAppDispatch();

	const columns: GridColDef[] = [
		{
			field: "Holiday",
			headerName: "Holiday",
			sortable: false,
			width: 250,
			renderCell: (params: GridRenderCellParams) => params.row.name,
		},
		{
			field: "startDate",
			headerName: " Start Date",
			sortable: false,
			renderCell: ({ value }: GridRenderCellParams) => dayjs(value).format("DD/MM/YYYY"),
		},
		{
			field: "endDate",
			headerName: "End Date",
			sortable: false,
			renderCell: ({ value }: GridRenderCellParams) => dayjs(value).format("DD/MM/YYYY"),
		},
		{
			field: "actions",
			type: "actions",
			headerName: "Actions",
			width: 100,
			renderCell: (params) => (
				<GridActionsCellItem icon={<DeleteIcon />} label='Delete' onClick={() => handleDelete(params)} />
			),
		},
	];

	useEffect(() => {
		if (deleteHolidayResponse) {
			dispatch(
				openSnackbar({
					severity: "success",
					message: deleteHolidayResponse?.message,
				}),
			);
		}
		if (deleteHolidayError) {
			dispatch(
				openSnackbar({
					severity: "error",
					message: deleteHolidayError?.data?.message,
				}),
			);
		}
	}, [deleteHolidayResponse, deleteHolidayError?.data]);

	const confirm = useConfirm();
	const handleDelete = (params: GridRenderCellParams) => {
		confirm({
			description: `This will permanently delete Holiday: ${params.row.name}.`,
		})
			.then(() => {
				deleteHoliday(params.row.id);
			})
			.catch(() => console.log("Deletion cancelled."));
	};
	const pageInfo: DynamicTable.TableProps = {
		columns: columns,
		rows: data?.data,
		rowCount: 0,
	};

	if (isLoading)
		return (
			<Box display='flex' justifyContent='center'>
				<CircularProgress />
			</Box>
		);

	return (
		<>
			<Container maxWidth='xl'>
				<Box mb={4} display='flex' justifyContent='space-between' alignItems='center'>
					<Typography variant='h5' color='initial'>
						Holidays
					</Typography>
					{userRole === "Admin" ? (
						<Box>
							<Button variant='contained' onClick={() => navigate(URL.ADD)}>
								Add
							</Button>
						</Box>
					) : null}
				</Box>
				<Table {...pageInfo}></Table>
			</Container>
		</>
	);
};

export default Holidays;
