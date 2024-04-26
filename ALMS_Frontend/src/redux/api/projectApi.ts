import { indexApi } from "./indexApi";

export const projectApi = indexApi.injectEndpoints({
	endpoints: (builder) => ({
		getProjects: builder.query<ProjectType.GetProjectsProps, ProjectType.GetProjectsQueryParams>({
			query: (queryParams) => ({
				url: "Projects",
				params: queryParams,
			}),
			providesTags: ["Project"],
		}),
		getProjectById: builder.query<ProjectType.GetProjectDetails, string>({
			query: (id) => `Projects/${id}`,
			providesTags: ["Project"],
		}),

		deleteProject: builder.mutation<void, string>({
			query: (id) => ({
				url: `Projects/${id}`,
				method: "Delete",
			}),
			invalidatesTags: ["Project", "Manager"],
		}),
		getProjectList: builder.query<Global.helperList, void>({
			query: () => ({
				url: "Projects/list",
			}),
			providesTags: ["Project"],
		}),
		addProject: builder.mutation<any, any>({
			query: (data) => ({
				url: "Projects",
				method: "POST",
				body: data,
			}),
			invalidatesTags: ["Project", "Manager"],
		}),
		editProject: builder.mutation<any, any>({
			query: ({ data, id }) => ({
				url: `Projects/${id}`,
				method: "PUT",
				body: data,
			}),
			invalidatesTags: ["Project", "Manager"],
		}),
		unAssignedManager: builder.query<Global.helperList, any>({
			query: (queryParams) => ({
				url: "Projects/UnassignManager/list",
				params: queryParams,
			}),

			providesTags: ["Project", "Manager"],
		}),
	}),
});

export const {
	useEditProjectMutation,
	useAddProjectMutation,
	useGetProjectsQuery,
	useGetProjectByIdQuery,
	useDeleteProjectMutation,
	useGetProjectListQuery,
	useUnAssignedManagerQuery,
} = projectApi;
