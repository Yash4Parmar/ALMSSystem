import {
	Attendance,
	Dashboard,
	EmployeeAdd,
	EmployeeDetails,
	EmployeeEdit,
	Employees,
	Holidays,
	LeaveForm,
	Leaves,
	Project,
	ProjectDetails,
	ProjectForm,
} from "../pages";
import AttendanceDetails from "../pages/attendance/AttendanceDetails";
import AttendanceEdit from "../pages/attendance/AttendanceEdit";
import HolidayAdd from "../pages/holiday/HolidayAdd";

import { URL } from "../utils/constants/URLConstants";

export const routerHelper: Global.RouteConfig = [
	{
		name: "Dashboard",
		path: URL.DASHBOARD,
		element: <Dashboard />,
		roles: ["Admin", "Manager", "Employee"],
	},
	{
		name: "Profile",
		path: URL.PROFILE,
		element: <EmployeeDetails />,
		roles: ["Manager", "Employee", "Admin"],
		children: [{ path: URL.EDIT, element: <EmployeeEdit />, roles: ["Employee", "Manager", "Admin"] }],
	},
	{
		name: "Employees",
		path: URL.EMPLOYEES,
		element: <Employees />,
		roles: ["Admin", "Manager"],
		children: [
			{ path: URL.ADD, element: <EmployeeAdd />, roles: ["Admin", "Employee"] },
			{ path: URL.EDIT, element: <EmployeeEdit />, roles: ["Admin", "Employee", "Manager"] },
			{ path: URL.DETAILS, element: <EmployeeDetails />, roles: ["Admin", "Manager"] },
		],
	},
	{
		name: "Projects",
		path: URL.PROJECT,
		element: <Project />,
		roles: ["Admin", "Manager", "Employee"],
		children: [
			{ path: URL.ADD, element: <ProjectForm />, roles: ["Admin"] },
			{ path: URL.EDIT, element: <ProjectForm />, roles: ["Admin", "Manager"] },
			{ path: URL.DETAILS, element: <ProjectDetails />, roles: ["Admin", "Manager"] },
		],
	},
	{
		name: "Leaves",
		path: URL.LEAVES,
		element: <Leaves />,
		roles: ["Admin", "Manager", "Employee"],
		children: [
			{ path: URL.ADD, element: <LeaveForm />, roles: ["Admin", "Manager", "Employee"] },
			{ path: URL.EDIT, element: <LeaveForm />, roles: ["Admin", "Manager", "Employee"] },
		],
	},
	{
		name: "Attendance",
		path: URL.ATTENDANCE,
		element: <Attendance />,
		roles: ["Admin", "Manager", "Employee"],
		children: [
			{ path: URL.EDIT, element: <AttendanceEdit />, roles: ["Admin", "Manager", "Employee"] },
			{ path: URL.DETAILS, element: <AttendanceDetails />, roles: ["Admin", "Manager", "Employee"] },
		],
	},
	{
		name: "Holidays",
		path: URL.HOLIDAYS,
		element: <Holidays />,
		roles: ["Admin", "Manager", "Employee"],
		children: [{ path: URL.ADD, element: <HolidayAdd />, roles: ["Admin"] }],
	},
];
