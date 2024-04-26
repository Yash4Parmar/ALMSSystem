import { useGetProjectsQuery } from "../../redux/api/projectApi";
import { useAppSelector } from "../../redux/hooks/hooks";
import { List, ListItem, ListSubheader } from "@mui/material";
import { NameWithAvatar } from "../../components/customTable";
import dayjs from "dayjs";

const AllProjectsDetails = () => {
	const userId = parseInt(useAppSelector((state) => state.auth.userData?.id || ""));

	const { data, error } = useGetProjectsQuery();

	const ApplyMargin = {
		marginRight: '6px',
		color: 'rgba(0, 0, 0,0.65)'
	}
	console.log(data, error);
	console.log(data?.data.projects);
	const projects = data?.data.projects;
	if (error) {
		return error.data.message;
	} else {
		return (
			<div style={{ display: 'flex', gap: "50px" }}>
				{projects &&
					projects.map((projectDetails, index) => (
						<List sx={{
							background: (theme) => (theme.palette.mode === "light" ? "rgba(25, 118, 210, 0.08)" : "#308fe8"),
							padding: '20px',
							boxShadow: '0px 0px 5px rgba(0, 0, 0,0.5)'
						}}>
							<ListSubheader sx={{ marginBottom: "20px" }}>No : {index + 1} </ListSubheader>
							<ListItem><b style={ApplyMargin}>Project :</b> {projectDetails.name}</ListItem>
							<ListItem><b style={ApplyMargin}>Project Description :</b> {projectDetails.description}</ListItem>
							<ListItem><b style={ApplyMargin}>Start Date :</b> {dayjs(projectDetails.startDate).format("DD/MM/YYYY")}</ListItem>
							<ListItem><b style={ApplyMargin}>End Date :</b>{dayjs(projectDetails.endDate).format("DD/MM/YYYY")}</ListItem>
							<ListItem><b style={ApplyMargin}>Project Manager :</b> {projectDetails.manager}</ListItem>
							<ListItem>
								<b style={{ position: 'absolute', top: '0%', marginTop: '7px', color: 'rgba(0, 0, 0,0.65)' }}>Employees :</b>{" "}
								<List sx={{ marginTop: '20px' }}>
									{projectDetails.employees.map((emp) => (
										<ListItem><NameWithAvatar name={emp} /></ListItem>

									))}
								</List>
							</ListItem>
						</List>
					))
				}
			</div >
		);
	}
};

export default AllProjectsDetails;
