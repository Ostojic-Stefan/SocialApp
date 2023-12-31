import { ChangeEvent, FormEvent, useState } from "react";
import { useAppDispatch } from "../../store";
import { useNavigate } from "react-router-dom";
import { getUserInformation, register } from "./identitySlice";

function Register() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();

  const [username, setUsername] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [biography, setBiography] = useState("");
  const [avatar, setAvatar] = useState<File>();

  async function handleSubmit(e: FormEvent<HTMLFormElement>): Promise<void> {
    e.preventDefault();
    let imageData;
    if (avatar) {
      imageData = new FormData();
      imageData.append("img", avatar);
    }
    try {
      await dispatch(
        register({ email, password, username, imageData, biography })
      ).unwrap();
      await dispatch(getUserInformation());
      navigate("/");
    } catch (error) {}
  }

  function handleFileChange(event: ChangeEvent<HTMLInputElement>): void {
    if (event.target.files) {
      setAvatar(event.target.files[0]);
    }
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
