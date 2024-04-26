import dayjs from "dayjs";
import * as yup from "yup";

export const setProjectFormSchema = (FormSchema, { options, initialValues }) => {
	let updatedSchema = {
		...FormSchema,
		defaultValues: {
			projectName: initialValues.name,
			description: initialValues.description,
			startDate: dayjs(initialValues.startDate).format("DD/MM/YYYY"),
			endDate: dayjs(initialValues.endDate).format("DD/MM/YYYY"),
			managerId: initialValues.managerId,
			employeeIds: initialValues.employees?.map((iteam) => iteam.employeeId),
		},
	};

	updatedSchema = addOptions(updatedSchema, options);

	return updatedSchema;
};

export const setLeaveFormSchema = (FormSchema, { options, initialValues, role }) => {
	console.log(FormSchema, options, initialValues, role);

	let updatedSchema = {
		...FormSchema,
		defaultValues: {
			startDate: dayjs(initialValues.startDate).format("DD/MM/YYYY"),
			endDate: dayjs(initialValues.endDate).format("DD/MM/YYYY"),
			reason: initialValues.reason,
			leaveTypeId: initialValues.leaveTypeId,
			statusId: initialValues.statusId,
			employeeId: initialValues.employeeId,
		},
	};

	updatedSchema = addOptions(updatedSchema, options);

	updatedSchema = filterFormSchemaByRole(updatedSchema, role);

	switch (initialValues.role) {
		case "Employee":
			updatedSchema = removeFormFields(updatedSchema, ["employeeId"]);
			break;

		case "Manager":
			break;

		case "Admin":
			break;
	}

	return updatedSchema;
};

export const setEmployeeFormSchema = (FormSchema, { options, initialValues, role }) => {
	console.log(FormSchema, options, initialValues, role);
	//console.log(initialValues.role);

	let updatedSchema = {
		...FormSchema,
		defaultValues: {
			firstName: initialValues.firstName,
			lastName: initialValues.lastName,
			phoneNumber: initialValues.phoneNumber,
			dob: dayjs(initialValues.dob).format("DD/MM/YYYY"),
			dateOfJoin: dayjs(initialValues.dateOfJoin).format("DD/MM/YYYY"),
			gender: initialValues.gender,
			roleName: initialValues.role,
			address: initialValues.address,
			managerId: initialValues.managerId,
			employeeIds: initialValues.employees?.map((iteam) => iteam.employeeId),
			projectIds: initialValues.projects?.map((iteam) => iteam.projectId),
		},
	};

	updatedSchema = addOptions(updatedSchema, options);

	updatedSchema = filterFormSchemaByRole(updatedSchema, role);

	switch (initialValues.role) {
		case "Employee":
			updatedSchema = removeFormFields(updatedSchema, ["employeeIds"]);
			break;

		case "Manager":
			updatedSchema = removeFormFields(updatedSchema, ["managerId"]);
			break;

		case "Admin":
			updatedSchema = removeFormFields(updatedSchema, ["employeeIds", "projectIds", "managerId"]);
			break;
	}

	return updatedSchema;
};

export const addOptions = (formSchema, options) => {
	const updatedFormSchema = { ...formSchema };

	options.forEach((optionSet) => {
		for (const [fieldName, fieldOptions] of Object.entries(optionSet)) {
			const field = updatedFormSchema.fields.find((f) => f.name === fieldName);
			if (field) {
				field.options = fieldOptions.map((option) => ({
					label: option.label,
					value: option.value,
				}));
			}
		}
	});

	return updatedFormSchema;
};

const filterFormSchemaByRole = (formSchema, specifiedRole) => {
	const filteredFields = formSchema.fields.filter((field) => {
		return field.role.includes(specifiedRole);
	});

	formSchema.fields = filteredFields;

	const filteredDefaultValues = {};
	for (const key in formSchema.defaultValues) {
		if (formSchema.defaultValues.hasOwnProperty(key)) {
			if (formSchema.fields.find((field) => field.name === key)) {
				filteredDefaultValues[key] = formSchema.defaultValues[key];
			}
		}
	}
	formSchema.defaultValues = filteredDefaultValues;

	const filteredValidationSchema = {};
	for (const key in formSchema.validationSchema.fields) {
		if (formSchema.validationSchema.fields.hasOwnProperty(key)) {
			if (formSchema.fields.find((field) => field.name === key)) {
				filteredValidationSchema[key] = formSchema.validationSchema.fields[key];
			}
		}
	}
	formSchema.validationSchema.fields = filteredValidationSchema;

	return formSchema;
};

const removeFormFields = (FormSchema: Global.FormSchema, names: string[]): Global.FormSchema => {
	const updatedFields = FormSchema.fields.filter((field) => !names.includes(field.name));

	const updatedValidationSchemaFields = Object.keys(FormSchema.validationSchema.fields)
		.filter((fieldName) => !names.includes(fieldName))
		.reduce((acc, fieldName) => {
			acc[fieldName] = FormSchema.validationSchema.fields[fieldName];
			return acc;
		}, {});

	const updatedValidationSchema = yup.object(updatedValidationSchemaFields);

	const updatedDefaultValues = { ...FormSchema.defaultValues };
	names.forEach((name) => {
		delete updatedDefaultValues[name];
	});

	return {
		...FormSchema,
		fields: updatedFields,
		validationSchema: updatedValidationSchema,
		defaultValues: updatedDefaultValues,
	};
};
