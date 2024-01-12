import { FormEvent, useState } from 'react';
import { useAuth } from '../context/AuthContext';

function Login() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const { login } = useAuth();

  async function handleSubmit(event: FormEvent): Promise<void> {
    event.preventDefault();
    await login(email, password);
  }

  return (
    <form onSubmit={(e) => handleSubmit(e)}>
      <input type='text' placeholder='email' value={email} onChange={(e) => setEmail(e.target.value)} />
      <input type='text' placeholder='password' value={password} onChange={(e) => setPassword(e.target.value)} />
      <button>Submit</button>
    </form>
  );
}

export default Login;
