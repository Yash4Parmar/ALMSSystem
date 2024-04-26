import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "../../redux/hooks/hooks";
import { logout } from "../../redux/slice/authSlice";

function Protected({ children }) {
	const navigate = useNavigate();
	const dispatch = useAppDispatch();
	const expireTime = localStorage.getItem("expireTime");

	const [loader, setLoader] = useState(false);

	useEffect(() => {
		const newTime = expireTime && JSON.parse(expireTime || "");
		const time = new Date(Date.now());
		const expTime = new Date(newTime || "");
		console.log(time, expTime);
		if (time >= expTime) {
			setLoader(true);
			dispatch(logout());
			navigate("/auth");
			console.log("expired");
		} else {
			console.log("not");
			setLoader(false);
		}
	}, []);

	// useEffect(() => {
	// 	console.log(authStatus);
	// 	if (authentication && authStatus !== authentication) {
	// 		navigate("/login");
	// 		// } else if (!authentication && authStatus !== authentication) {
	// 		//   navigate("/")
	// 	}
	// }, []);

	return loader ? <h1>Loading...</h1> : <>{children}</>;
}

export default Protected;
