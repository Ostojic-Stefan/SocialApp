import { FormEvent, useState } from "react";
import { useAppDispatch } from "../store";
import { login } from "../features/user/userSlice";
import { useNavigate } from "react-router-dom";

function Login() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");

  function handleSubmit(e: FormEvent) {
    e.preventDefault();
    dispatch(login({ email, password }))
      .unwrap()
      .then(() => navigate("/"));
  }

  return (
    <form onSubmit={(e) => handleSubmit(e)}>
      <input
        type="text"
        placeholder="email"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
      />
      <input
        type="text"
        placeholder="password"
        value={password}
        onChange={(e) => setPassword(e.target.value)}
      />
      <button>Submit</button>
    </form>
  );
}

export default Login;
