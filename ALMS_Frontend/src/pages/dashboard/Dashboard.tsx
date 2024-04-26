import React, { useEffect, useState } from "react";
import Container from "@mui/material/Container";
import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import { Box, Button, Typography, List, ListItem, ListItemText } from "@mui/material";
import CircularProgress, { circularProgressClasses } from "@mui/material/CircularProgress";
import { useAddPunchMutation, useGetAttendancesQuery } from "../../redux/api/attendanceApi";
import { useAppSelector } from "../../redux/hooks/hooks";
import dayjs from "dayjs";
import { useNavigate } from "react-router-dom";

const Dashboard = () => {
	const [time, setTime] = useState(new Date());
	const [isPunchedIn, setIsPunchedIn] = useState(false);
	const [punchTimes, setPunchTimes] = useState([]);
	const employeeId = useAppSelector((state) => state.auth.userData?.id);
	const currentDate = dayjs().format("YYYY-MM-DD");
	const currentTime = dayjs().format("hh:mm:ss");
	const [addPunch] = useAddPunchMutation();

	const navigate = useNavigate();
	useEffect(() => {
		if (employeeId) {
			navigate("/dashboard");
		} else {
			navigate("/auth");
		}
	}, []);

	const { data: attendance } = useGetAttendancesQuery({
		EmployeeId: employeeId,
		Date: currentDate,
	});

	useEffect(() => {
		if (attendance) {
			setPunchTimes(attendance?.data?.times);
			attendance?.data?.times.length % 2 === 0 ? setIsPunchedIn(false) : setIsPunchedIn(true);
		}
	}, [attendance]);

	const handlePunch = () => {
		// setIsPunchedIn(!isPunchedIn);
		addPunch({
			time: dayjs().format("HH:mm:ss"),
			employeeId: Number(employeeId),
			date: dayjs().format("YYYY-MM-DD"),
		});
	};

	return (
		<Container maxWidth='xl' sx={{ display: "flex", gap: 5 }}>
			<Card variant='outlined'>
				<CardContent>
					<Box
						sx={{
							display: "flex",
							gap: 2,
							flexDirection: "column",
							position: "relative",
							textAlign: "center",
							width: 400,
						}}
					>
						<Box>
							<Typography variant='h5' gutterBottom>
								Timesheet
							</Typography>
							<Typography variant='subtitle1' color='text.secondary' gutterBottom>
								{time.toDateString()}
							</Typography>
						</Box>
						<Box>
							<Box component='div' sx={{ position: "relative", display: "inline-block" }}>
								<FacebookCircularProgress isPunchedIn={isPunchedIn} />
								<Typography
									variant='h6'
									component='div'
									sx={{
										position: "absolute",
										top: "50%",
										left: "50%",
										transform: "translate(-50%, -50%)",
									}}
								>
									{attendance?.data.workingHours} Hours
								</Typography>
							</Box>
						</Box>
						<Button variant='contained' color='primary' onClick={handlePunch}>
							{isPunchedIn ? "Punch Out" : "Punch In"} {/* Toggle punch in/out */}
						</Button>
					</Box>
				</CardContent>
			</Card>
			<Card variant='outlined'>
				<CardContent>
					<Box width={400}>
						<Typography variant='h6' gutterBottom>
							Today Activity
						</Typography>
						<List>
							{punchTimes.map((time, index) => (
								<ListItem key={index}>
									<ListItemText>
										{index % 2 === 0 ? "Punch In" : "Punch Out"} - {time}
									</ListItemText>
								</ListItem>
							))}
						</List>
					</Box>
				</CardContent>
			</Card>
		</Container>
	);
};

export default Dashboard;

function FacebookCircularProgress({ isPunchedIn }) {
	const progressValue = isPunchedIn ? 0 : 100;
	return (
		<Box sx={{ position: "relative" }}>
			<CircularProgress
				variant='determinate'
				color='primary'
				value={progressValue}
				size={120}
				thickness={2}
				sx={{
					position: "absolute",
					left: 0,
					top: 0,
					zIndex: 1,
				}}
			/>
			<CircularProgress
				variant='indeterminate'
				disableShrink
				sx={{
					color: (theme) => (theme.palette.mode === "light" ? "#1a90ff" : "#308fe8"),
					animationDuration: isPunchedIn ? "2000ms" : "0ms",
					[`& .${circularProgressClasses.circle}`]: {
						strokeLinecap: "round",
					},
				}}
				size={120}
				thickness={2}
			/>
		</Box>
	);
}
