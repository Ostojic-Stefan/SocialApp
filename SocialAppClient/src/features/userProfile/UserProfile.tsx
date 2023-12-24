import { NavLink, Outlet, useParams } from "react-router-dom";
import { useAppDispatch, useAppSelector } from "../../store";
import { useEffect } from "react";
import { clearState, getUserProfileInformation } from "./userProfileSlice";
import styles from './UserProfile.module.css';
import Modal from "../../components/Modal";
import UploadUserProfileForm from "./UploadUserProfileForm/UploadUserProfileForm";

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
    <>
      <div className={styles.container}>
      <Modal>
        <Modal.Open>
          <img className={styles.imgStyles} src={userProfileInformation?.avatarUrl} alt="" />
        </Modal.Open>
        <Modal.Content>
          <UploadUserProfileForm />
        </Modal.Content>
      </Modal>
        <aside className={styles.userInformation}>
          <h2>{userProfileInformation?.username}</h2>
          <p>{userProfileInformation?.biography}</p>
        </aside>
      </div>
      <nav className={styles.navigation}>
        <NavLink to="posts">Posts</NavLink>
        <NavLink to="comments">Comments</NavLink>
      </nav>
      <Outlet />
    </>
  );
}

export default UserProfile;
