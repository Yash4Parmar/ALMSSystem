import { indexApi } from "./indexApi";

export const attendanceApi = indexApi.injectEndpoints({
	endpoints: (builder) => ({
		getAttendances: builder.query<any, any>({
			query: (queryParams) => ({
				url: "Attendances",
				params: queryParams,
			}),
			providesTags: ["Attendance"],
		}),
		addPunch: builder.mutation<any, any>({
			query: (data) => ({
				url: "Attendances",
				method: "POST",
				body: data,
			}),
			invalidatesTags: ["Attendance"],
		}),
		getAllAttendances: builder.query<any, any>({
			query: (queryParams) => ({
				url: "Attendances/GetAllAttendance",
				params: queryParams,
			}),
			providesTags: ["Attendance"],
		}),
		updateAttendance: builder.mutation<any, any>({
			query: (data) => ({
				url: "Attendances/RequestAttendance",
				method: "POST",
				body: data,
			}),
			invalidatesTags: ["Attendance"],
		}),
		updateAttendanceStatus: builder.mutation<any, any>({
			query: (data) => ({
				url: "Attendances/UpdateAttendance",
				method: "POST",
				body: data,
			}),
			invalidatesTags: ["Attendance"],
		}),
		getRequestedAttendanceById: builder.query<any, any>({
			query: (queryParams) => ({
				url: "Attendances/GetRequestedAttendanceById",
				params: queryParams,
			}),
			providesTags: ["Attendance"],
		}),
		getAttendanceById: builder.query<any, any>({
			query: (queryParams) => ({
				url: "Attendances/GetAttendanceByAttenndanceId",
				params: queryParams,
			}),
			providesTags: ["Attendance"],
		}),
	}),
});

export const {
	useGetRequestedAttendanceByIdQuery,
	useGetAttendanceByIdQuery,
	useUpdateAttendanceMutation,
	useUpdateAttendanceStatusMutation,
	useGetAttendancesQuery,
	useAddPunchMutation,
	useGetAllAttendancesQuery,
} = attendanceApi;
