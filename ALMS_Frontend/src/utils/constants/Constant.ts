import { URL } from "..";

export const pages = [
	{ name: "Dashboard", path: URL.DASHBOARD },
	{ name: "Projects", path: URL.PROJECT },
	{ name: "Employees", path: URL.EMPLOYEES },
	{ name: "Attendance", path: URL.ATTENDANCE },
	{ name: "Leaves", path: URL.LEAVES },
	{ name: "Holidays", path: URL.HOLIDAYS },
];

export const tokenFields = {
	role: "http://schemas.microsoft.com/ws/2008/06/identity/claims/role",
	id: "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier",
	userName: "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name",
};
