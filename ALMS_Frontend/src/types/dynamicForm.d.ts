declare namespace DynamicForm {
	type FormDetails = {
		component: string;
		type?: string;
		options?: Global.Option[];
		name: string;
		placeholder?: string;
		label?: string;
		sm?: number;
		rows?: number;
		role?: Array<Global.Role>;
	};

	type DynamicFormProps = {
		fields: FormDetails[];
		defaultValues: Record<string, unknown>;
		onSubmit: (data: Record<string, string | number>) => void;
		schema: yup.AnyObjectSchema;
	};

	type FormElements = {
		name: string;
		component: string;
		control: Control<unknown>;
		options?: Global.Option[];
		label?: string;
		defaultValue: string;
		type?: string;
		rows?: number;
	};

	type LoginForm = {
		username: string;
		password: string;
	};
}
