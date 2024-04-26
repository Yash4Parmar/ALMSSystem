import { MouseEvent, useEffect, useState } from "react";
import { Chip, Menu, MenuItem, Snackbar } from "@mui/material";
import { Adjust, ArrowDropDown } from "@mui/icons-material";
import { GridRenderCellParams } from "@mui/x-data-grid";
import { useStatusHelperQuery, useUpdateLeaveMutation } from "../../../redux/api/leaveApi";
import { useAppDispatch, useAppSelector } from "../../../redux/hooks/hooks";
import { useUpdateAttendanceStatusMutation } from "../../../redux/api/attendanceApi";
import { openSnackbar } from "../../../redux/slice/snackbarSlice";

type StatusComponetProps = GridRenderCellParams & {
	type?: string;
};
const StatusComponent = ({ type, ...params }: StatusComponetProps) => {
	const userRole = useAppSelector((state) => state.auth.userData?.role);
	const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
	const open = Boolean(anchorEl);

	const statusId = params.row.statusId;
	const { data } = useStatusHelperQuery();
	const [changeLeaveStatus, { data: changeLeaveResponse, error: changeLeaveError }] = useUpdateLeaveMutation();
	const [changeAttendanceStatus, { data: changeAttendanceResponse, error: changeAttendanceError }] =
		useUpdateAttendanceStatusMutation();
	const [statusName, setStatusName] = useState("");

	const dispatch = useAppDispatch();
	useEffect(() => {
		if (changeLeaveResponse || changeAttendanceResponse) {
			dispatch(
				openSnackbar({
					severity: "success",
					message: changeLeaveResponse ? changeLeaveResponse.message : changeAttendanceResponse.message,
				}),
			);
		}
		if (changeLeaveError || changeAttendanceError) {
			dispatch(
				openSnackbar({
					severity: "error",
					message: changeLeaveError ? changeLeaveError?.data?.message : changeAttendanceError?.data?.message,
				}),
			);
		}
	}, [changeLeaveResponse, changeLeaveError?.data, changeAttendanceResponse, changeAttendanceError?.data]);

	useEffect(() => {
		if (!data) return;
		const newData = [...data.data];
		newData.push({ value: 0, label: "Default" });

		setStatusName(newData.filter((obj) => obj.value === statusId)[0]?.label || "");
	}, [data, statusId]);

	const handleClick = (event: MouseEvent<HTMLElement>) => {
		setAnchorEl(event.currentTarget);
	};

	const handleClose = (value: number) => {
		if (type === "leave") {
			const data = {
				startDate: params.row.from,
				endDate: params.row.to,
				reason: params.row.reason,
				leaveTypeId: params.row.leaveTypeId,
				statusId: value,
			};

			changeLeaveStatus({ data: data, id: params.row.leaveId });
		} else {
			changeAttendanceStatus({
				attendanceId: params.row.attendanceId,
				statusId: value,
			});
		}
		setAnchorEl(null);
	};

	const colorCode = {
		Pending: "primary",
		Approved: "success",
		Rejected: "error",
	};

	return userRole === "Employee" ? (
		statusName === "Default" ? (
			<>-</>
		) : (
			<Chip
				variant='filled'
				icon={
					<Adjust
						fontSize='small'
						color={colorCode[statusName as keyof typeof colorCode] as "error" | "success" | "primary"}
					/>
				}
				label={statusName}
			/>
		)
	) : statusName === "Default" ? (
		<>-</>
	) : (
		<>
			<Chip
				variant='outlined'
				icon={
					<Adjust
						fontSize='small'
						color={colorCode[statusName as keyof typeof colorCode] as "error" | "success" | "primary"}
					/>
				}
				deleteIcon={<ArrowDropDown />}
				onDelete={() => { }}
				onClick={handleClick}
				label={statusName}
			/>
			<Menu
				id='basic-menu'
				anchorEl={anchorEl}
				open={open}
				onClose={handleClose}
				MenuListProps={{
					"aria-labelledby": "basic-button",
				}}
			>
				{data?.data &&
					data.data.map((obj) => (
						<MenuItem key={obj.value} onClick={() => handleClose(obj.value as number)}>
							<Adjust
								fontSize='small'
								color={
									colorCode[obj.label as keyof typeof colorCode] as "error" | "success" | "primary"
								}
							/>
							{obj.label}
						</MenuItem>
					))}
			</Menu>

			{/* <SnackBarComponent /> */}
		</>
	);
};

export default StatusComponent;
