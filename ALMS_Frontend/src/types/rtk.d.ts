declare namespace EmployeeType {
	type EmployeeProps = {
		id: number;
		employeeId: number;
		name: string;
		mobile: string;
		dateOfJoin: string;
		role: string;
	};

	type EmployeeDataProp = {
		count: number;
		employees: EmployeeProps[];
	};

	type GetEmployeesProps = Omit<Global.apiResponse, "data"> & {
		data: EmployeeDataProp;
	};

	type GetEmployeesQueryParams = Global.SearchParams & {
		employeeIds?: number[];
		managerIds?: number;
		projectIds?: number;
	};

	type GetEmployeeDetails = Omit<Global.apiResponse, "data"> & {
		data: {
			projects: [
				{
					projectId: number;
					project: string;
				},
			];
			employees: unknown;
			id: number;
			firstName: string;
			lastName: string;
			phoneNumber: string;
			dateOfJoin: string;
			role: Global.Role;
			email: string;
			gender: string;
			address: string;
			dob: string;
			managerId: number;
			manager: string;
		};
	};
}

declare namespace ProjectType {
	type ProjectProps = {
		employees: string[];
		id: number;
		projectId: number;
		name: string;
		startDate: string;
		endDate: string;
		manager: string;
	};

	type ProjectDataProp = {
		count: number;
		projects: ProjectProps[];
	};

	type GetProjectsProps = Omit<Global.apiResponse, "data"> & {
		data: ProjectDataProp;
	};

	type GetProjectDetails = Omit<Global.apiResponse, "data"> & {
		data: {
			employees: [
				{
					employeeId: number;
					employee: string;
				},
			];
			id: number;
			name: string;
			description: string;
			startDate: string;
			endDate: string;
			managerId: number;
			manager: string;
		};
	};

	type GetProjectsQueryParams = Global.SearchParams & {
		employeeIds?: number[];
		projectIds?: number;
	};
}

declare namespace LeaveType {
	type GetLeaveQueryParams = Global.SearchParams & {
		leaveTypeIds?: number[];
		employeeIds?: number[];
		managerIds?: number[];
		statusIds?: number[];
		from?: string;
		to?: string;
	};

	type LeaveProps = {
		id: number;
		leaveId: number;
		name: string;
		leaveType: string;
		from: string;
		to: string;
		noOfDays: number;
		reason: string;
		status: {
			statusId: number;
			statusName: string;
		};
	};

	type LeaveDataProp = {
		count: number;
		leaves: LeaveProps[];
	};

	type GetLeavesProps = Omit<Global.apiResponse, "data"> & {
		data: LeaveDataProp;
	};

	type GetLeaveDetails = Omit<Global.apiResponse, "data"> & {
		data: {
			leaveId: number;
			from: string;
			to: string;
			noOfDays: number;

			employeeId: number;
			name: string;

			leaveTypeId: number;
			leaveType: string;

			statusId: number;
			statusName: string;
		};
	};
}

declare namespace HolidayType{

	type holidayProps = {
		id: number,
		name: string,
		startDate: string,
		endDate: string
	}

	type holidayDataProp = {
		count: number;
		leaves: holidayProps[];
	};

	type GetHolidayProps = Omit<Global.apiResponse, "data"> & {
		data: holidayDataProp;
	};
}

declare namespace Rtk {
	type Token = {
		token: string;
		expiration: string;
	};
}
