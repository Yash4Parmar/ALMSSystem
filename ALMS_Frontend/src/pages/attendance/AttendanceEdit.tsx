import Container from "@mui/material/Container";
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { useGetAttendanceByIdQuery, useUpdateAttendanceMutation } from "../../redux/api/attendanceApi";
import { LocalizationProvider, TimePicker } from "@mui/x-date-pickers";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import { Box, Button, CircularProgress, IconButton, Typography } from "@mui/material";
import dayjs from "dayjs";
import { openSnackbar } from "../../redux/slice/snackbarSlice";
import { useAppDispatch } from "../../redux/hooks/hooks";
import RemoveCircleOutlineIcon from "@mui/icons-material/RemoveCircleOutline";
import { KeyboardBackspace } from "@mui/icons-material";
import { URL } from "../../utils";

function AttendanceEdit() {
	const { id } = useParams();
	const [punchTimes, setPunchTimes] = useState([]);
	const navigate = useNavigate();
	const dispatch = useAppDispatch();
	const { data: attendance } = useGetAttendanceByIdQuery({
		AttendanceId: id,
	});

	const [updateAttendance, { data: updateResponse, error: updateError }] = useUpdateAttendanceMutation();

	useEffect(() => {
		if (updateResponse) {
			dispatch(
				openSnackbar({
					severity: "success",
					message: updateResponse.message,
				}),
			);
			navigate("/attendance");
		}
		if (updateError) {
			dispatch(
				openSnackbar({
					severity: "error",
					message: updateError?.data?.message,
				}),
			);
		}
	}, [updateResponse, updateError?.data]);

	useEffect(() => {
		if (attendance) {
			setPunchTimes(() => {
				return attendance.data.times.map((timeString) => {
					return dayjs(timeString, "HH:mm:ss");
				});
			});
		}
	}, [attendance]);

	const handleAddTime = () => {
		setPunchTimes([...punchTimes, null, null]);
	};

	const handleTimeChange = (index, newTime) => {
		const updatedTimes = [...punchTimes];
		updatedTimes[index] = newTime;
		setPunchTimes(updatedTimes);
	};

	const handleSubmit = () => {
		if (punchTimes.some((time) => time === null)) {
			dispatch(
				openSnackbar({
					severity: "error",
					message: "Punch times cannot be empty.",
				}),
			);
			return;
		}

		const formattedPunchTimes = punchTimes.map((time) => time.format("HH:mm:ss"));

		updateAttendance({
			AttendanceId: id,
			times: formattedPunchTimes,
		});
	};

	const handleRemovePair = (pairIndex) => {
		const updatedPunchTimes = [...punchTimes];
		updatedPunchTimes.splice(pairIndex, 2); // Remove the pair
		setPunchTimes(updatedPunchTimes);
	};

	if (!attendance)
		return (
			<Box display='flex' justifyContent='center'>
				<CircularProgress />
			</Box>
		);

	console.log(attendance);

	return (
		<Container maxWidth='xl'>
			<Box
				sx={{
					margin: "20px 0",
					display: "flex",
					alignItems: "center",
					justifyContent: "space-between",
					borderBottom: "1px solid #ccc",
					paddingBottom: "10px",
				}}
			>
				<Box sx={{ display: "flex", alignItems: "center" }}>
					<IconButton onClick={() => navigate(URL.ATTENDANCE)}>
						<KeyboardBackspace />
					</IconButton>
					<Typography variant='h5' sx={{ marginRight: "20px" }}>
						{attendance.data.name}
					</Typography>
					<Typography variant='h6'>{dayjs(attendance.data.date).format("DD/MM/YYYY")}</Typography>
				</Box>
				<Typography variant='h6'>{attendance.data.workingHours} Hrs</Typography>
			</Box>

			{punchTimes.map((time, index) => {
				if (index % 2 === 0) {
					return (
						<Box key={index} sx={{ display: "flex", alignItems: "center", marginBottom: 2 }}>
							<LocalizationProvider dateAdapter={AdapterDayjs}>
								<TimePicker
									label='Punch in'
									value={dayjs(time)}
									onChange={(newTime) => handleTimeChange(index, newTime)}
								/>
							</LocalizationProvider>
							<Box sx={{ width: 16 }} />
							<LocalizationProvider dateAdapter={AdapterDayjs}>
								<TimePicker
									label='Punch out'
									value={dayjs(punchTimes[index + 1])}
									onChange={(newTime) => handleTimeChange(index + 1, newTime)}
								/>
							</LocalizationProvider>
							<Button
								sx={{ marginLeft: 2 }}
								onClick={() => handleRemovePair(index)}
								// variant='contained'
								// color='error'
							>
								<RemoveCircleOutlineIcon sx={{ color: "Red" }} />
							</Button>
						</Box>
					);
				}
				return null;
			})}
			<Button variant='contained' color='primary' onClick={handleAddTime}>
				Add Time
			</Button>
			<Button sx={{ marginLeft: 2 }} variant='contained' color='primary' onClick={handleSubmit}>
				Submit
			</Button>
		</Container>
	);
}

export default AttendanceEdit;
