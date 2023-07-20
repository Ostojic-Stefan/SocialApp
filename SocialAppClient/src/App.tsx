import { BrowserRouter, Route, Routes } from "react-router-dom";
import "./App.css";
import Home from "./pages/Home";
import UserProfile from "./pages/UserProfile";
import AppLayout from "./ui/Layout/AppLayout";
import { Provider } from "react-redux";
import store from "./store";
import Login from "./pages/Login";
import Register from "./features/user/Register";

function App() {
  return (
    <BrowserRouter>
      <Provider store={store}>
        <Routes>
          <Route element={<AppLayout />}>
            <Route path="/" element={<Home />} />
            <Route path="/profile" element={<UserProfile />} />
            <Route path="/login" element={<Login />} />
            <Route path="/register" element={<Register />} />
          </Route>
        </Routes>
      </Provider>
    </BrowserRouter>
  );
}

export default App;
