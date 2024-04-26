import {
	FormHelperText,
	InputLabel,
	MenuItem,
	Select,
	Autocomplete,
	FormControl,
	IconButton,
	InputAdornment,
	TextField,
} from "@mui/material";

import { Controller } from "react-hook-form";
import dayjs from "dayjs";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
import { DatePicker } from "@mui/x-date-pickers/DatePicker";
import { DateTimePicker, TimePicker } from "@mui/x-date-pickers";
import { renderTimeViewClock } from "@mui/x-date-pickers/timeViewRenderers";
import Visibility from "@mui/icons-material/Visibility";
import VisibilityOff from "@mui/icons-material/VisibilityOff";
import { useState } from "react";


const FormField = ({
	name,
	component,
	control,
	options,
	label,
	defaultValue,
	type,
	rows,
	maxDate,
	minDate
}: DynamicForm.FormElements) => {
	const [showPassword, setShowPassword] = useState(false);
	switch (component) {
		case "text":
			return (
				<Controller
					name={name}
					control={control}
					defaultValue={defaultValue}
					render={({ field, fieldState: { error } }) => (
						<>
							<TextField
								{...field}
								label={label}
								multiline={rows ? true : false}
								rows={rows}
								error={!!error}
								helperText={error ? error.message : ""}
								type={type}
								fullWidth
							/>
						</>
					)}
				/>
			);
		case "select":
			return (
				<Controller
					name={name}
					control={control}
					defaultValue={defaultValue}
					render={({ field: { onChange, value, ...fieldProps }, fieldState: { error } }) => {
						const selectedOption = options ? options.find((option) => option.value === value) : null;
						return (
							<Autocomplete
								{...fieldProps}
								options={options || []}
								value={selectedOption}
								getOptionLabel={(option) => option.label}
								isOptionEqualToValue={(option, value) => option.value === value.value}
								disableClearable
								renderInput={(params) => (
									<TextField {...params} label={label} error={!!error} helperText={error?.message} />
								)}
								onChange={(_, data) => onChange(data ? data.value : null)}
							/>
						);
					}}
				/>
			);

		case "multipleSelect":
			return (
				<Controller
					name={name}
					control={control}
					defaultValue={defaultValue}
					render={({ field: { onChange, value, ...fieldProps }, fieldState: { error } }) => {
						const autocompleteValue = value
							? options?.filter((option) => value.includes(option.value))
							: [];
						return (
							<Autocomplete
								{...fieldProps}
								multiple
								options={options || []}
								value={autocompleteValue}
								getOptionLabel={(option) => option.label}
								isOptionEqualToValue={(option, value) => option.value === value.value}
								renderInput={(params) => (
									<TextField {...params} label={label} error={!!error} helperText={error?.message} />
								)}
								onChange={(_, data) => onChange(data.map((d) => d.value))}
							/>
						);
					}}
				/>
			);

		case "datePicker":
			return (
				<LocalizationProvider dateAdapter={AdapterDayjs}>
					<Controller
						name={name}
						control={control}
						defaultValue={defaultValue ? dayjs(defaultValue, "DD/MM/YYYY") : null}
						render={({ field, fieldState: { error } }) => (
							<FormControl error={!!error} fullWidth>
								<DatePicker
									{...field}
									label={label}
									value={field.value ? dayjs(field.value) : null}
									onChange={field.onChange}
									format='DD/MM/YYYY'
									maxDate={maxDate ? maxDate : null}
									minDate={minDate ? minDate : null}
									slotProps={{
										textField: {
											error: !!error,
											helperText: error?.message,
										},
									}}
								/>
							</FormControl>
						)}
					/>
				</LocalizationProvider>
			);
		case "dateTimePicker":
			return (
				<LocalizationProvider dateAdapter={AdapterDayjs}>
					<Controller
						name={name}
						control={control}
						defaultValue={defaultValue ? dayjs(defaultValue) : null}
						render={({ field, fieldState: { error } }) => (
							<FormControl error={!!error} fullWidth>
								<DateTimePicker
									{...field}
									label={label}
									value={field.value}
									onChange={field.onChange}
									viewRenderers={{
										hours: renderTimeViewClock,
										minutes: renderTimeViewClock,
										seconds: renderTimeViewClock,
									}}
									slotProps={{
										textField: {
											error: !!error,
											helperText: error?.message,
										},
									}}
								/>
							</FormControl>
						)}
					/>
				</LocalizationProvider>
			);
		case "timePicker":
			return (
				<LocalizationProvider dateAdapter={AdapterDayjs}>
					<Controller
						name={name}
						control={control}
						defaultValue={defaultValue ? dayjs(defaultValue) : null}
						render={({ field, fieldState: { error } }) => (
							<FormControl error={!!error} fullWidth>
								<TimePicker
									{...field}
									label={label}
									value={field.value}
									onChange={field.onChange}
									viewRenderers={{
										hours: renderTimeViewClock,
										minutes: renderTimeViewClock,
										seconds: renderTimeViewClock,
									}}
									slotProps={{
										textField: {
											error: !!error,
											helperText: error?.message,
										},
									}}
								/>
							</FormControl>
						)}
					/>
				</LocalizationProvider>
			);
		case "password":
			return (
				<Controller
					name={name}
					control={control}
					defaultValue={defaultValue}
					render={({ field, fieldState: { error } }) => (
						<>
							<TextField
								{...field}
								label={label}
								error={!!error}
								helperText={error ? error.message : ""}
								type={showPassword ? "text" : "password"}
								fullWidth
								InputProps={{
									endAdornment: (
										<InputAdornment position='end'>
											<IconButton onClick={() => setShowPassword(!showPassword)} edge='end'>
												{showPassword ? <Visibility /> : <VisibilityOff />}
											</IconButton>
										</InputAdornment>
									),
								}}
							/>
						</>
					)}
				/>
			);
		default:
			return null;
	}
};

export default FormField;
