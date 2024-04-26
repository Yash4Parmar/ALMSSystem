import { indexApi } from "./indexApi";

export const HolidayApi = indexApi.injectEndpoints({
	endpoints: (builder) => ({
		getHolidays: builder.query<HolidayType.GetHolidayProps, void>({
			query: () => ({
				url: "Holiday"
			}),
			providesTags: ["Holiday"],
		}),
        deleteHoliday: builder.mutation<void, string>({
			query: (id) => ({
				url: `Holiday/${id}`,
				method: "Delete",
			}),
			invalidatesTags: ["Holiday"],
		}),
        addHoliday: builder.mutation<any, any>({
			query: (data) => ({
				url: "Holiday",
				method: "POST",
				body: data,
			}),
			invalidatesTags: ["Holiday"],
		}),
	}),
});

export const {
    useGetHolidaysQuery,
    useDeleteHolidayMutation,
    useAddHolidayMutation
} = HolidayApi;
