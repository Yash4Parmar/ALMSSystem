import React, { useEffect } from "react";
import { useAddHolidayMutation } from "../../redux/api/holidayApi";
import { Box, Container, IconButton, Paper, Typography } from "@mui/material";
import { KeyboardBackspace } from "@mui/icons-material";
import DynamicForm from "../../components/dynamicForm/DynamicForm";
import { useNavigate } from "react-router-dom";
import { URL } from "../../utils";
import { addholiday } from "../../utils/constants/formConstant";
import dayjs from "dayjs";
import { openSnackbar } from "../../redux/slice/snackbarSlice";
import { useAppDispatch } from "../../redux/hooks/hooks";

const HolidayAdd = () => {
	const navigate = useNavigate();
	const dispatch = useAppDispatch();
	const [createHoliday, { data: createHolidayResponse, error: createHolidayError }] = useAddHolidayMutation();

	useEffect(() => {
		console.log(createHolidayError, createHolidayResponse);
		if (createHolidayResponse) {
			dispatch(
				openSnackbar({
					severity: "success",
					message: createHolidayResponse?.message,
				}),
			);
			navigate("/holidays");
		}
		if (createHolidayError) {
			dispatch(
				openSnackbar({
					severity: "error",
					message: createHolidayError?.data?.message,
				}),
			);
		}
	}, [createHolidayResponse, createHolidayError?.data]);

	const handleSubmit = (data: Record<string, string | number>) => {
		const formattedData = {
			...data,
			startDate: dayjs(data.startDate).format("YYYY-MM-DD"),
			endDate: dayjs(data.endDate).format("YYYY-MM-DD"),
		};
		createHoliday(formattedData);
	};

	return (
		<Container maxWidth='xl'>
			<Box display='flex' gap={2} alignItems='center' mb={2}>
				<IconButton onClick={() => navigate(URL.HOLIDAYS)}>
					<KeyboardBackspace />
				</IconButton>
				<Typography variant='h5' ml={-2}>
					Add Holiday
				</Typography>
			</Box>
			<Paper variant='elevation' sx={{ p: 2, maxWidth: "sm" }} elevation={2}>
				<DynamicForm
					fields={addholiday.fields}
					onSubmit={handleSubmit}
					defaultValues={addholiday.defaultValues}
					schema={addholiday.validationSchema}
				/>
			</Paper>
		</Container>
	);
};

export default HolidayAdd;
