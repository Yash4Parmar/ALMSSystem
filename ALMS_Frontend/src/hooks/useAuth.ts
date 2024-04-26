import { useEffect } from "react";
import { useAppDispatch, useAppSelector } from "../redux/hooks/hooks";
import { login, logout } from "../redux/slice/authSlice";
import { jwtDecode } from "jwt-decode";

interface DecodedToken {
	"http://schemas.microsoft.com/ws/2008/06/identity/claims/role": string;
	"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier": string;
	"http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name": string;
	exp?: number;
}
const useAuth = () => {
	const dispatch = useAppDispatch();
	const userData = useAppSelector((state) => state.auth.userData);

	const setUser = (data: { token: string }) => {
		const decode = jwtDecode<DecodedToken>(data.token);
		const user = {
			role: decode["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"],
			id: decode["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"],
			userName: decode["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"],
		};

		dispatch(login(user));
	};

	const clearUser = () => {
		localStorage.removeItem("userData");
		dispatch(logout());
	};

	const isLoggedIn = () => {
		return !!userData;
	};

	return { userData, setUser, clearUser, isLoggedIn };
};

export default useAuth;
