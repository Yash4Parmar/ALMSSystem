import { useNavigate, useParams } from "react-router-dom";
import { useGetEmployeesByIdQuery } from "../../redux/api/employeeApi";
import { Box, Button, Card, CardContent, Grid, Typography } from "@mui/material";
import dayjs from "dayjs";
import { useEffect, useState } from "react";
import { useAppSelector } from "../../redux/hooks/hooks";
import { URL, replaceIdInURL } from "../../utils";

const EmployeeDetails = () => {
	const { id } = useParams();
	const [detailId, setDetailId] = useState(id);
	const userData = useAppSelector((state) => state.auth.userData);
	const navigate = useNavigate();

	useEffect(() => {
		if (!id) {
			setDetailId(userData?.id);
		}
	}, [id]);

	const { data: employeeDetails } = useGetEmployeesByIdQuery(detailId);
	const details = employeeDetails?.data;

	return (
		details && (
			<>
				<Box mb={4} display='flex' justifyContent='space-between' alignItems='center'>
					<Typography variant='h5' color='initial'>
						Profile
					</Typography>
					{userData?.role === "Employee" ||
						(userData?.role !== "Employee" && window.location.pathname.includes("profile") && (
							<Box>
								<Button
									variant='contained'
									onClick={() => navigate(replaceIdInURL(URL.EDIT, detailId))}
								>
									Edit
								</Button>
							</Box>
						))}
				</Box>
				<Grid container spacing={4} justifyContent='center'>
					<Grid item xs={12} sm={6}>
						<Card>
							<CardContent>
								<Typography variant='h5' gutterBottom>
									Personal Information
								</Typography>
								<Typography>
									<strong>First Name:</strong> {details.firstName}
								</Typography>
								<Typography>
									<strong>Last Name:</strong> {details.lastName}
								</Typography>
								<Typography>
									<strong>Date of Birth:</strong> {dayjs(details.dob).format("DD/MM/YYYY")}
								</Typography>
								<Typography>
									<strong>Email:</strong> {details.email}
								</Typography>
								<Typography>
									<strong>Gender:</strong> {details.gender}
								</Typography>
								<Typography>
									<strong>Mobile No.:</strong> {details.phoneNumber}
								</Typography>
								<Typography>
									<strong>Address:</strong>
									{details.address}
								</Typography>
								<Typography>
									<strong>Role:</strong> {details.role}
								</Typography>
								{details.manager && (
									<Typography>
										<strong>Manager:</strong> {details.manager}
									</Typography>
								)}
							</CardContent>
						</Card>
					</Grid>
					<Grid item xs={12} sm={6}>
						<Card>
							<CardContent>
								<Typography variant='h5' gutterBottom>
									Projects
								</Typography>
								{details.projects.map((project) => (
									<Typography key={project.projectId}>
										<strong>Project:</strong> {project.project}
									</Typography>
								))}
							</CardContent>
						</Card>
					</Grid>
				</Grid>
			</>
		)
	);
};

export default EmployeeDetails;
