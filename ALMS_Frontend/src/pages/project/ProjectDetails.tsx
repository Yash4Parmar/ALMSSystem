import { useParams } from "react-router-dom";
import { useGetProjectByIdQuery } from "../../redux/api/projectApi";
import { List, ListItem, Typography } from "@mui/material";
import dayjs from "dayjs";
import { NameWithAvatar } from "../../components/customTable";

const ProjectDetails = () => {
	const { id } = useParams();
	const { data } = useGetProjectByIdQuery(id ? id : "");
	const projectDetails = data?.data;
	console.log(projectDetails);
	return (
		projectDetails && (
			<List>
				<ListItem>
					<Typography variant='h4'> {projectDetails.name}</Typography>
				</ListItem>
				<ListItem> {projectDetails.description}</ListItem>
				<ListItem>
					<Typography>Start Date</Typography> : {dayjs(projectDetails.startDate).format("DD/MM/YYYY")}
				</ListItem>
				<ListItem>End Date : {dayjs(projectDetails.endDate).format("DD/MM/YYYY")}</ListItem>
				<ListItem>Manager : {projectDetails.manager}</ListItem>
				<ListItem>Employees :</ListItem>
				<ListItem>
					<List>
						{projectDetails.employees.map((emp) => (
							<ListItem key={emp.employeeId}>
								<NameWithAvatar name={emp.employee} />
								{/* {emp.employee} */}
							</ListItem>
						))}
					</List>
				</ListItem>
			</List>
		)
	);
};

export default ProjectDetails;
