import { BrowserRouter, Route, Routes } from "react-router-dom";
import "./App.css";
import Home from "./pages/Home";
import UserProfile from "./pages/UserProfile";
import AppLayout from "./ui/Layout/AppLayout";
import { Provider } from "react-redux";
import store from "./store";

function App() {
  return (
    <BrowserRouter>
      <Provider store={store}>
        <Routes>
          <Route element={<AppLayout />}>
            <Route path="/" element={<Home />} />
            <Route path="/profile" element={<UserProfile />} />
          </Route>
        </Routes>
      </Provider>
    </BrowserRouter>
  );
}

export default App;
