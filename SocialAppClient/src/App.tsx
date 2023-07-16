import { BrowserRouter, Route, Routes } from "react-router-dom";
import "./App.css";
import Home from "./pages/Home";
import UserProfile from "./pages/UserProfile";
import AppLayout from "./ui/Layout/AppLayout";

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route element={<AppLayout />}>
          <Route path="/" element={<Home />} />
          <Route path="/profile" element={<UserProfile />} />
        </Route>
      </Routes>
    </BrowserRouter>
  );
}

export default App;
