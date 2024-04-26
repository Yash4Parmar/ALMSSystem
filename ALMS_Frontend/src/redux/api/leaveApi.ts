import { indexApi } from "./indexApi";

export const leaveApi = indexApi.injectEndpoints({
	endpoints: (builder) => ({
		getLeaves: builder.query<LeaveType.GetLeavesProps, LeaveType.GetLeaveQueryParams>({
			query: (queryParams) => ({
				url: "Leaves/GetLeaves",
				params: queryParams,
			}),
			providesTags: ["Leave"],
		}),
		getLeavesById: builder.query<LeaveType.GetLeaveDetails, string>({
			query: (id) => `Leaves/${id}`,
			providesTags: ["Leave"],
		}),
		statusHelper: builder.query<Global.helperList, void>({ query: () => "status/list" }),
		leaveHelper: builder.query<Global.helperList, void>({
			query: () => "Leaves/leaveTypes/list",
		}),
		updateLeave: builder.mutation<any, any>({
			query: ({ data, id }) => ({
				url: `Leaves/${id}`,
				method: "PUT",
				body: data,
			}),
			invalidatesTags: ["Leave"],
		}),
		addLeave: builder.mutation<any, any>({
			query: ({ data }) => ({
				url: "Leaves",
				method: "POST",
				body: data,
			}),
			invalidatesTags: ["Leave"],
		}),
	}),
});

export const {
	useGetLeavesQuery,
	useGetLeavesByIdQuery,
	useLeaveHelperQuery,
	useStatusHelperQuery,
	useUpdateLeaveMutation,
	useAddLeaveMutation,
} = leaveApi;
