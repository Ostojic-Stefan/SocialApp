import { BrowserRouter, Route, Routes } from "react-router-dom";
import "./App.css";
import Home from "./features/home/Home";
import UserProfile from "./features/userProfile/UserProfile";
import AppLayout from "./ui/Layout/AppLayout";
import { useAppDispatch } from "./store";
import Login from "./features/identity/Login";
import Register from "./features/identity/Register";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import { useEffect } from "react";
import { getUserInformation } from "./features/identity/identitySlice";
import UserProfilePosts from "./features/userProfile/posts/UserProfilePosts";
import UserProfileComments from "./features/userProfile/comments/UserProfileComments";
import { getFriends } from "./features/userProfile/userProfileSlice";

function App() {
  const dispatch = useAppDispatch();

  useEffect(() => {
    // TODO: move to different component
    dispatch(getUserInformation());
    dispatch(getFriends());
  }, []);

  return (
    <BrowserRouter>
      <ToastContainer
        position="bottom-right"
        autoClose={5000}
        hideProgressBar={false}
        newestOnTop={false}
        closeOnClick
        rtl={false}
        pauseOnFocusLoss
        draggable
        pauseOnHover
        theme="light"
      />
      <Routes>
        <Route element={<AppLayout />}>
          <Route path="/" element={<Home />} />
          <Route path="/profile/:username" element={<UserProfile />}>
            <Route path="posts" element={<UserProfilePosts />} />
            <Route path="comments" element={<UserProfileComments />} />
          </Route>
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;
