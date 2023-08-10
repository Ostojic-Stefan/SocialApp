import { NavLink, Outlet, useParams } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "../../store";
import { useEffect } from "react";
import { clearState, getUserProfileInformation } from "./userProfileSlice";

function UserProfile() {
  const dispatch = useAppDispatch();
  const userProfileInformation = useAppSelector((store) => store.user.userInfo);

  const { username } = useParams();

  useEffect(() => {
    if (username) {
      dispatch(getUserProfileInformation({ username }));
    }

    return () => {
      dispatch(clearState(null));
    };
  }, []);

  return (
    <div>
      <nav>
        <NavLink to="posts">Posts</NavLink>
        <NavLink to="comments">Comments</NavLink>
      </nav>
      <div>UserName: {userProfileInformation?.username}</div>
      <Outlet />
    </div>
  );
}

export default UserProfile;
