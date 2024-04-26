import { createApi, fetchBaseQuery } from "@reduxjs/toolkit/query/react";
import { RootState } from "../store";

export const indexApi = createApi({
	reducerPath: "indexApi",
	baseQuery: fetchBaseQuery({
		baseUrl: "http://localhost:19050/api/",
		prepareHeaders: (headers, { getState }) => {
			const { token: token } = (getState() as RootState).auth;

			if (token) {
				headers.set("authorization", `Bearer ${token}`);
			}
			return headers;
		},
	}),
	tagTypes: ["Employee", "Manager", "Project", "Leave", "Attendance", "Holiday"],
	endpoints: (builder) => ({}),
});

export const {} = indexApi;
