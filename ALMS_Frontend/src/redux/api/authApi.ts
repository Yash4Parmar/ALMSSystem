import { indexApi } from "./indexApi";

export const authApi = indexApi.injectEndpoints({
	endpoints: (builder) => ({
		login: builder.mutation<Rtk.Token, Record<string, string | number>>({
			query: (data) => ({
				url: "Authenticate/login",
				method: "POST",
				body: data,
			}),
		}),
		register: builder.mutation<void, Record<string, string | number>>({
			query: (data) => ({
				url: "Authenticate/register",
				method: "POST",
				body: data,
			}),
			invalidatesTags: ["Employee", "Manager"],
		}),
	}),
});

export const { useLoginMutation, useRegisterMutation } = authApi;
