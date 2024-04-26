import { useForm, FormProvider } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import { Button, Grid } from "@mui/material";
import FormField from "./FormField";

const DynamicForm = ({ fields, schema, onSubmit, defaultValues }: DynamicForm.DynamicFormProps) => {
	const methods = useForm({
		resolver: yupResolver(schema),
	});

	return (
		<FormProvider {...methods}>
			<form onSubmit={methods.handleSubmit(onSubmit)}>
				<Grid container spacing={2}>
					{fields.map((field, index) => (
						<Grid key={index} item xs={12} sm={field.sm}>
							<FormField
								{...field}
								control={methods.control}
								defaultValue={defaultValues[field.name] as string}
							/>
						</Grid>
					))}
					<Grid item xs={12} style={{ textAlign: "center" }}>
						{/* <Button type='reset' variant='contained' color='secondary' onClick={() => methods.reset()}>
							Reset
						</Button> */}
						<Button type='submit' variant='contained' color='primary' style={{ marginLeft: 10 }}>
							{/* temporary fix */}
							{schema._nodes.length === 2 ? "Login" : "Submit"}
						</Button>
					</Grid>
				</Grid>
			</form>
		</FormProvider>
	);
};

export default DynamicForm;
