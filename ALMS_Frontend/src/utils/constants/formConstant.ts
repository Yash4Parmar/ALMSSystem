import dayjs from "dayjs";
import * as yup from "yup";

const today = dayjs();
const fifteenYearsAgo = today.subtract(15, "year");
const startOfYear = today.startOf("year");
const endOfYear = today.endOf("year");

export const loginForm: Global.FormSchema = {
	validationSchema: yup.object({
		usernameOrEmail: yup.string().required("Please enter your Username."),
		password: yup
			.string()
			.required("Please enter your Password.")
			.matches(
				/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z@\d]{8,}$/,
				"Password must contain at least one uppercase letter, one lowercase letter, and one number.",
			),
	}),

	defaultValues: {
		usernameOrEmail: "",
		password: "",
	},

	fields: [
		{
			component: "text",
			type: "text",
			name: "usernameOrEmail",
			placeholder: "Enter your username or Email",
			label: "Username or Email",
			sm: 12,
		},
		{
			component: "password",
			type: "password",
			name: "password",
			placeholder: "Enter your password",
			label: "Password",
			sm: 12,
		},
	],
};

export const createEmployee: Global.FormSchema = {
	validationSchema: yup.object({
		firstname: yup.string().required("Please enter Employee's first name."),
		lastname: yup.string().required("Please enter Employee's last name."),
		username: yup.string().required("Please enter Employee's username."),
		password: yup
			.string()
			.required("Please enter a password.")
			.matches(
				/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z@\d]{8,}$/,
				"Password must contain at least one uppercase letter, one lowercase letter, and one number.",
			),
		email: yup.string().email().required("Please enter Employee's email."),
		role: yup.string().required("Role is required"),
		dateOfJoin: yup.string().required("Please enter Employee's Joining Date."),
	}),

	defaultValues: {
		firstname: "",
		lastname: "",
		username: "",
		password: "",
		email: "",
		role: "",
		dateOfJoin: "",
	},

	fields: [
		{
			component: "text",
			type: "text",
			name: "firstname",
			placeholder: "Enter employee First Name",
			label: "First Name",
			sm: 12,
		},
		{
			component: "text",
			type: "text",
			name: "lastname",
			placeholder: "Enter employee Last Name",
			label: "Last Name",
			sm: 12,
		},
		{
			component: "text",
			type: "text",
			name: "username",
			placeholder: "Enter your username",
			label: "Username",
			sm: 12,
		},
		{
			component: "text",
			type: "password",
			name: "password",
			placeholder: "Enter your password",
			label: "Password",
			sm: 12,
		},
		{
			component: "text",
			type: "email",
			name: "email",
			placeholder: "Enter your email",
			label: "Email",
			sm: 12,
		},
		{
			component: "select",
			type: "text",
			name: "role",
			placeholder: "Select employee role",
			label: "Role",
			options: [
				{ label: "Admin", value: "Admin" },
				{ label: "Manager", value: "Manager" },
				{ label: "Employee", value: "Employee" },
			],
			sm: 6,
		},
		{
			component: "datePicker",
			type: "string",
			name: "dateOfJoin",
			placeholder: "Enter Join Date",
			label: "Join Date",
			sm: 6,
		},
	],
};

export const editEmployee: Global.FormSchema = {
	validationSchema: yup.object({
		firstName: yup
			.string()
			.required("Please enter first name.")
			.matches(/^[a-zA-Z]*$/, "Name should contain alphabets only."),
		lastName: yup
			.string()
			.required("Please enter last name.")
			.matches(/^[a-zA-Z]*$/, "Name should contain alphabets only."),
		phoneNumber: yup
			.string()
			.nullable()
			.matches(/^[\d]{10}$/, "Phone number contains only 10 digits."),
		dob: yup.string().nullable(),
		dateOfJoin: yup.string().required("Please select a joining date."),
		gender: yup.string().nullable(),
		roleName: yup.string().required("Please enter role."),
		address: yup.string().nullable(),
		managerId: yup.string().nullable(),
		projectIds: yup.array().nullable(),
		employeeIds: yup.array().nullable(),
	}),

	defaultValues: {
		firstName: "",
		lastName: "",
		phoneNumber: "",
		dateOfBirth: "",
		dob: "",
		gender: "",
		roleName: "",
		address: "",
		managerId: "",
		projectIds: [],
		employeeIds: [],
	},

	fields: [
		{
			component: "text",
			type: "text",
			name: "firstName",
			placeholder: "Enter your First Name",
			label: "First Name",
			sm: 6,
			role: ["Admin", "Manager", "Employee"],
		},
		{
			component: "text",
			type: "text",
			name: "lastName",
			placeholder: "Enter your Last Name",
			label: "Last Name",
			sm: 6,
			role: ["Admin", "Manager", "Employee"],
		},
		{
			component: "text",
			type: "number",
			name: "phoneNumber",
			placeholder: "Enter your Phone Number",
			label: "Phone Number",
			sm: 6,
			role: ["Admin", "Manager", "Employee"],
		},
		{
			component: "select",
			type: "text",
			name: "roleName",
			placeholder: "Select employee role",
			label: "Role",
			options: [
				{ label: "Admin", value: "Admin" },
				{ label: "Manager", value: "Manager" },
				{ label: "Employee", value: "Employee" },
			],
			sm: 6,
			role: ["Admin", "Manager"],
		},
		{
			component: "datePicker",
			type: "string",
			name: "dateOfJoin",
			placeholder: "Enter Join Date",
			label: "Join Date",
			sm: 6,
			role: ["Admin", "Manager"],
		},
		{
			component: "datePicker",
			type: "string",
			name: "dob",
			placeholder: "Enter your Birth Date",
			label: "Birth Date",
			sm: 6,
			role: ["Admin", "Manager", "Employee"],
			maxDate: fifteenYearsAgo,
		},
		{
			component: "select",
			type: "text",
			name: "gender",
			placeholder: "Select gender",
			label: "Gender",
			options: [
				{ label: "Male", value: "Male" },
				{ label: "Female", value: "Female" },
			],
			sm: 12,
			role: ["Admin", "Manager", "Employee"],
		},
		{
			component: "select",
			type: "text",
			name: "managerId",
			placeholder: "Select manager",
			label: "Manager",
			options: [],
			sm: 12,
			role: ["Admin", "Manager"],
		},
		{
			component: "text",
			type: "string",
			name: "address",
			placeholder: "Enter your address",
			label: "Address",
			sm: 12,
			rows: 3,
			role: ["Admin", "Manager", "Employee"],
		},
		{
			component: "multipleSelect",
			type: "text",
			name: "projectIds",
			placeholder: "Select project",
			label: "Projects",
			options: [],
			sm: 12,
			role: ["Admin", "Manager"],
		},
		{
			component: "multipleSelect",
			type: "text",
			name: "employeeIds",
			placeholder: "Select buddies",
			label: "Buddies",
			options: [],
			sm: 12,
			role: ["Admin", "Manager"],
		},
	],
};

export const addProject: Global.FormSchema = {
	validationSchema: yup.object({
		projectName: yup.string().required("Please enter project name."),
		description: yup.string().required("Please enter project description."),
		startDate: yup.string().required("Please select a project start date."),
		endDate: yup
			.string()
			.required("Please select a project end date.")
			.test("is-later", "End date must be later than start date", function (value) {
				const { startDate } = this.parent;
				return new Date(value) > new Date(startDate);
			}),
		managerId: yup.string().required("Please select a manager."),
		employeeIds: yup.array().required("Please select atleast one employee."),
	}),

	defaultValues: {
		projectName: "",
		description: "",
		startDate: "",
		endDate: "",
		managerId: "",
		employeeIds: [],
	},

	fields: [
		{
			component: "text",
			type: "text",
			name: "projectName",
			placeholder: "Enter Project Name",
			label: "Project Name",
			sm: 12,
		},
		{
			component: "text",
			type: "text",
			name: "description",
			placeholder: "Enter your Project Description",
			label: "Project Description",
			sm: 12,
			rows: 5,
		},
		{
			component: "datePicker",
			type: "string",
			name: "startDate",
			placeholder: "Enter Project Start Date",
			label: "Start Date",
			sm: 6,
		},
		{
			component: "datePicker",
			type: "string",
			name: "endDate",
			placeholder: "Enter Project End Date",
			label: "End Date",
			sm: 6,
		},
		{
			component: "select",
			type: "text",
			name: "managerId",
			placeholder: "Select manager",
			label: "Manager",
			options: [],
			sm: 12,
		},
		{
			component: "multipleSelect",
			type: "text",
			name: "employeeIds",
			placeholder: "Select buddies",
			label: "Employees",
			options: [],
			sm: 12,
		},
	],
};

export const addLeave: Global.FormSchema = {
	validationSchema: yup.object({
		startDate: yup.string().required("Please enter start date."),
		endDate: yup
			.string()
			.required("Please enter end date.")
			.test("is-later", "End date must be later than start date", function (value) {
				const { startDate } = this.parent;
				return new Date(value) > new Date(startDate);
			}),
		reason: yup.string().required("Please enter reason."),
		leaveTypeId: yup.string().required("Please select a leave type."),
		statusId: yup.string().required("Please select leave status."),
		employeeId: yup.string().required("Please select the employee."),
	}),

	defaultValues: {
		startDate: "",
		endDate: "",
		reason: "",
		leaveTypeId: "",
		statusId: "",
		employeeId: "",
	},

	fields: [
		{
			component: "select",
			type: "text",
			name: "employeeId",
			placeholder: "Select buddies",
			label: "Employees",
			options: [],
			sm: 12,
			role: ["Admin", "Manager"],
		},
		{
			component: "select",
			type: "text",
			name: "leaveTypeId",
			placeholder: "Select Leave Type",
			label: "Leave Type",
			options: [],
			sm: 6,
			role: ["Admin", "Manager", "Employee"],
		},
		{
			component: "select",
			type: "text",
			name: "statusId",
			label: "Status",
			sm: 6,
			role: ["Admin", "Manager"],
		},
		{
			component: "datePicker",
			type: "string",
			name: "startDate",
			placeholder: "Enter Leave Start Date",
			label: "From",
			sm: 6,
			role: ["Admin", "Manager", "Employee"],
			maxDate: endOfYear,
			minDate: startOfYear,
		},
		{
			component: "datePicker",
			type: "string",
			name: "endDate",
			placeholder: "Enter Leavea End Date",
			label: "To",
			sm: 6,
			role: ["Admin", "Manager", "Employee"],
		},
		{
			component: "text",
			type: "text",
			name: "reason",
			placeholder: "Enter Reason of Leave",
			label: "Reason",
			sm: 12,
			rows: 2,
			role: ["Admin", "Manager", "Employee"],
		},
	],
};

export const addholiday: Global.FormSchema = {
	validationSchema: yup.object({
		startDate: yup.string().required("Please enter start date."),
		endDate: yup
			.string()
			.required("Please enter end date.")
			.test("is-later", "End date must be later than start date", function (value) {
				const { startDate } = this.parent;
				return new Date(value) > new Date(startDate);
			}),
		name: yup.string().required("Please enter holiday name."),
	}),

	defaultValues: {
		startDate: "",
		endDate: "",
		name: "",
	},

	fields: [
		{
			component: "datePicker",
			type: "string",
			name: "startDate",
			placeholder: "Enter holiday Start Date",
			label: "From",
			sm: 6,
			role: ["Admin", "Manager", "Employee"],
		},
		{
			component: "datePicker",
			type: "string",
			name: "endDate",
			placeholder: "Enter holiday End Date",
			label: "To",
			sm: 6,
			role: ["Admin", "Manager", "Employee"],
		},
		{
			component: "text",
			type: "text",
			name: "name",
			placeholder: "Enter day name",
			label: "Holiday",
			sm: 12,
			rows: 2,
			role: ["Admin", "Manager", "Employee"],
		},
	],
};
