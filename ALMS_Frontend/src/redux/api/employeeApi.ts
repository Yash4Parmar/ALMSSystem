import { indexApi } from "./indexApi";

export const employeeApi = indexApi.injectEndpoints({
	endpoints: (builder) => ({
		getEmployees: builder.query<EmployeeType.GetEmployeesProps, EmployeeType.GetEmployeesQueryParams>({
			query: (queryParams) => ({
				url: "Employees",
				params: queryParams,
			}),
			providesTags: ["Employee"],
		}),
		getEmployeesList: builder.query<Global.helperList, void>({
			query: () => ({
				url: "Employees/EmployeesList",
			}),
			providesTags: ["Employee"],
		}),
		getManagerList: builder.query<Global.helperList, void>({
			query: () => ({
				url: "Employees/ManagersList",
			}),
			providesTags: ["Manager"],
		}),
		getEmployeesById: builder.query<EmployeeType.GetEmployeeDetails, string | undefined>({
			query: (id) => ({
				url: `Employees/${id}`,
			}),
			providesTags: ["Employee"],
		}),
		getOnlyEmployees: builder.query<Global.helperList, void>({
			query: () => ({
				url: "Employees/OnlyEmployeesList",
			}),
			providesTags: ["Employee", "Manager"],
		}),
		editEmployee: builder.mutation<any, any>({
			query: ({ data, id }) => ({
				url: `Employees/${id}`,
				method: "PUT",
				body: data,
			}),
			invalidatesTags: ["Employee", "Manager"],
		}),
		deleteEmployee: builder.mutation<void, string>({
			query: (id) => ({
				url: `Employees/${id}`,
				method: "Delete",
			}),
			invalidatesTags: ["Employee", "Manager"],
		}),
	}),
});

export const {
	useGetEmployeesQuery,
	useGetEmployeesListQuery,
	useEditEmployeeMutation,
	useGetManagerListQuery,
	useGetEmployeesByIdQuery,
	useDeleteEmployeeMutation,
	useGetOnlyEmployeesQuery,
} = employeeApi;
