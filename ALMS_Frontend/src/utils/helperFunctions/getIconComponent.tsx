import { Dashboard, Assignment, Group, EventNote, BeachAccess, Flight, Person } from "@mui/icons-material";

export const getIconComponent = (icon: string) => {
	switch (icon) {
		case "Dashboard":
			return <Dashboard />;
		case "Projects":
			return <Assignment />;
		case "Employees":
			return <Group />;
		case "Profile":
			return <Person />;
		case "Attendance":
			return <EventNote />;
		case "Leaves":
			return <BeachAccess />;
		case "Holidays":
			return <Flight />;
		default:
			return null;
	}
};
