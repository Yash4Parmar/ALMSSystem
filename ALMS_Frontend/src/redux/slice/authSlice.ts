import { createSlice } from "@reduxjs/toolkit";
import { jwtDecode } from "jwt-decode";
import { tokenFields } from "../../utils/constants/Constant";

const initialState: Global.InitialUser = {
	userData: null,
	token: null,
};

export const authSlice = createSlice({
	name: "auth",
	initialState,
	reducers: {
		login: (state, action) => {
			if (action.payload.token) {
				localStorage.setItem("userData", JSON.stringify(action.payload.token));
				action.payload.expiration &&
					localStorage.setItem("expireTime", JSON.stringify(action.payload.expiration));
				const decode = jwtDecode<Global.DecodedToken>(action.payload.token);
				const user: Global.userData = {
					role: decode[tokenFields.role as keyof Global.DecodedToken],
					id: decode[tokenFields.id as keyof Global.DecodedToken][1],
					userName: decode[tokenFields.userName as keyof Global.DecodedToken],
				};
				state.userData = user;
				state.token = action.payload.token;
				console.log(user);
			}
		},
		logout: (state) => {
			state.userData = null;
			state.token = null;
			localStorage.removeItem("userData");
			localStorage.removeItem("expireTime");
		},
	},
});

export const { login, logout } = authSlice.actions;

export default authSlice.reducer;
