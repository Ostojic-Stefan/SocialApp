import { ChangeEvent, FormEvent, useState } from "react";
import { useAppDispatch } from "../../store";
import { register } from "./userSlice";

function Register() {
  const dispatch = useAppDispatch();

  const [username, setUsername] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [biography, setBiography] = useState("");
  const [avatar, setAvatar] = useState<File>();

  function handleSubmit(e: FormEvent<HTMLFormElement>): void {
    e.preventDefault();
    dispatch(
      register({ email, password, username, avatarUrl: undefined, biography })
    );
  }

  function handleFileChange(event: ChangeEvent<HTMLInputElement>): void {
    throw new Error("Function not implemented.");
  }

  return (
    <form onSubmit={(e) => handleSubmit(e)}>
      <input
        type="text"
        placeholder="username"
        value={username}
        onChange={(e) => setUsername(e.target.value)}
      />
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
      <input
        type="text"
        placeholder="biography"
        value={biography}
        onChange={(e) => setBiography(e.target.value)}
      />
      <input type="file" onChange={handleFileChange} />
      <button>Submit</button>
    </form>
  );
}

export default Register;
