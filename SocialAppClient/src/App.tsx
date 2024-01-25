import { Navigate, Route, Routes } from 'react-router-dom';
import Home from './pages/Home';
import Login from './pages/Login';
import Register from './pages/Register';
import 'react-toastify/dist/ReactToastify.css';
import Post from './components/Post';
import AppLayout from './pages/AppLayout';
import PostListAll from './components/PostListAll';
import PostListFriends from './components/PostListFriends';
import UserProfile from './pages/UserProfile';
import PostDetails from './pages/PostDetails';
import Auth from './components/Auth';

export default function App() {
  return (
    <div>
      <Routes>
        <Route
          element={
            <Auth>
              <AppLayout />
            </Auth>
          }
        >
          <Route path='/' element={<Home />}>
            <Route path='' element={<Navigate to='all-posts' />} />
            <Route index path='all-posts' element={<PostListAll />} />
            <Route path='friend-posts' element={<PostListFriends />} />
          </Route>
          <Route path='/post/:postId' element={<PostDetails />} />
          <Route path='/profile/:username' element={<UserProfile />}>
            {/* <Route path='posts' element={<UserProfilePosts />} />
            <Route path='comments' element={<UserProfileComments />} /> */}
          </Route>
          <Route path='/post/:postId' element={<Post />} />
        </Route>
        <Route path='/login' element={<Login />} />
        <Route path='/register' element={<Register />} />
      </Routes>
    </div>
  );
}
