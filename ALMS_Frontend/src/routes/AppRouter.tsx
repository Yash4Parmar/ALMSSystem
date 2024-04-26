import { routerHelper } from "./RouterHelper";
import Layout from "../components/layouts/Layout";
import { Auth } from "../pages";
import { useAppDispatch, useAppSelector } from "../redux/hooks/hooks";
import { login } from "../redux/slice/authSlice";
import { useEffect, useState } from "react";
import { CircularProgress, Container } from "@mui/material";
import { Outlet, Route, RouterProvider, createBrowserRouter, createRoutesFromElements } from "react-router-dom";
import { Dashboard } from "../pages";
import Protected from "../components/layouts/Wrapper";

const AppRouter = () => {
	const dispatch = useAppDispatch();
	const userRole: Global.Role = useAppSelector((state) => state.auth.userData?.role) as Global.Role;
	const [isUserRole, setIsUserRole] = useState(false);
	console.log(userRole);
	const [isInitialized, setIsInitialized] = useState(false);
	useEffect(() => {
		const localData = localStorage.getItem("userData");
		if (localData) {
			const userData = JSON.parse(localData);
			dispatch(login({ token: userData }));
			setIsUserRole(true);
		}

		setIsInitialized(true);
	}, []);

	if (!isInitialized) {
		return (
			<Container
				maxWidth='lg'
				sx={{ display: "flex", justifyContent: "center", alignItems: "center", height: "100vh" }}
			>
				<CircularProgress />
			</Container>
		);
	}

	const routes = createRoutesFromElements(
		<Route
			path='/'
			element={
				<Protected>
					<Layout />
				</Protected>
			}
		>
			<Route path='/auth' element={<Auth />} />
			<Route path='/' element={<Dashboard />} />
			{routerHelper
				.filter((route) => route.roles?.includes(userRole))
				.map((route) => {
					if (route.children && route.children.length > 0) {
						const childRoutes = route.children
							.filter((route) => route.roles.includes(userRole))
							.map((childRoute) => (
								<Route key={childRoute.path} path={childRoute.path} element={childRoute.element} />
							));
						console.log(childRoutes);

						return (
							<Route key={route.path} path={route.path} element={<Outlet />}>
								<Route path='' element={route.element} />
								{childRoutes}
							</Route>
						);
					} else {
						return <Route key={route.path} path={route.path} element={route.element} />;
					}
				})}
		</Route>,
	);
	const router = createBrowserRouter(routes);

	return <RouterProvider router={router} />;
};

export default AppRouter;
