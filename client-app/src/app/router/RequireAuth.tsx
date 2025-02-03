import { Navigate, Outlet, useLocation } from "react-router-dom";
import { useStore } from "../stores/store";

const RequireAuth = () => {
  const { userStore:{isLoggedIn} } = useStore();
  const location = useLocation();

  if (!isLoggedIn) {
      return <Navigate to="/" state={{ from: location.pathname }} />;
  }

  return <Outlet />;
};
export default RequireAuth;
