import { FormEvent, useState } from 'react';
import { useAppDispatch, useAppSelector } from '../../store';
import { useNavigate } from 'react-router-dom';
import { getUserInformation, login } from './identitySlice';

function Login() {
  const dispatch = useAppDispatch();
  const { authenticated } = useAppSelector((store) => store.identity);
  const navigate = useNavigate();

  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  async function handleSubmit(event: FormEvent): Promise<void> {
    event.preventDefault();
    await dispatch(login({ email, password })).unwrap();
    if (authenticated) {
      await dispatch(getUserInformation());
      navigate('/');
    }
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
