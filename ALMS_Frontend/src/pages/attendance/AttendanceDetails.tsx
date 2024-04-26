import React, { useEffect, useState } from "react";
import Container from "@mui/material/Container";
import { useParams } from "react-router-dom";
import { useGetAttendanceByIdQuery, useGetRequestedAttendanceByIdQuery } from "../../redux/api/attendanceApi";
import { Box, Card, CardContent, List, ListItem, ListItemText, Typography } from "@mui/material";

function AttendanceDetails() {
	const { id } = useParams();
	const [punchTimes, setPunchTimes] = useState([]);
	const [updatedPunchTimes, setUpdatedPunchTimes] = useState([]);

	const { data: attendance } = useGetAttendanceByIdQuery({
		AttendanceId: id,
	});
	const { data: updatedAttendance } = useGetRequestedAttendanceByIdQuery({
		AttendanceId: id,
	});

	useEffect(() => {
		if (attendance) {
			setPunchTimes(attendance?.data.times);
			setUpdatedPunchTimes(updatedAttendance?.data.times);
		}
	}, [attendance, updatedAttendance]);

	return (
		<Container maxWidth='xl'>
			<Box width={300}>
				<Card variant='outlined'>
					<CardContent>
						<Typography variant='h6' gutterBottom>
							Old Activity
						</Typography>
						<List>
							{punchTimes?.map((time, index) => (
								<ListItem key={index}>
									<ListItemText>
										{index % 2 === 0 ? "Punch In" : "Punch Out"} - {time}
									</ListItemText>
								</ListItem>
							))}
						</List>
						<Typography variant='h6' gutterBottom>
							Total Hours : {attendance?.data.workingHours}
						</Typography>
					</CardContent>
				</Card>
				<Card variant='outlined'>
					<CardContent>
						<Typography variant='h6' gutterBottom>
							Requested Activity
						</Typography>
						<List>
							{updatedPunchTimes?.map((time, index) => (
								<ListItem key={index}>
									<ListItemText>
										{index % 2 === 0 ? "Punch In" : "Punch Out"} - {time}
									</ListItemText>
								</ListItem>
							))}
						</List>
						<Typography variant='h6' gutterBottom>
							Total Hours : {updatedAttendance?.data.workingHours}
						</Typography>
					</CardContent>
				</Card>
			</Box>
		</Container>
	);
}

export default AttendanceDetails;
