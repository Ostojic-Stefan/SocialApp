import { ChangeEvent, FormEvent, useState } from 'react';
import { useAppDispatch } from '../store';
import { Link, useNavigate } from 'react-router-dom';
import { Input, Button } from '@nextui-org/react';
import { FaBook, FaEnvelope, FaLock, FaUser } from 'react-icons/fa';
import { useAuth } from '../context/AuthContext';

function Register() {
  const dispatch = useAppDispatch();
  const navigate = useNavigate();

  const [username, setUsername] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [biography, setBiography] = useState('');

  const { register } = useAuth();

  async function handleSubmit(e: FormEvent<HTMLFormElement>): Promise<void> {
    e.preventDefault();
    await register({ email, password, username, biography });
  }

  return (
    <div className='absolute top-0 left-0 right-0 bottom-0 h-screen bg-[url("/public/images/4850017.jpg")]'>
      <div className='h-screen w-screen flex items-center justify-center'>
        <div className='rounded-3xl bg-default bg-opacity-20 backdrop-blur-md shadow-xl flex flex-col gap-10 p-10'>
          <h1 className='text-black text-3xl font-semibold'>Sign Up</h1>
          <form className='flex flex-col gap-10 w-96' onSubmit={(e) => handleSubmit(e)}>
            <div className='flex gap-4 items-center'>
              <Input
                type='text'
                endContent={<FaUser className='opacity-50' />}
                variant='underlined'
                label='Username'
                labelPlacement='inside'
                value={username}
                onChange={(e) => setUsername(e.target.value)}
              />
            </div>

            <div className='flex gap-4 items-center'>
              <Input
                type='email'
                endContent={<FaEnvelope className='opacity-50' />}
                variant='underlined'
                label='Email'
                labelPlacement='inside'
                value={email}
                onChange={(e) => setEmail(e.target.value)}
              />
            </div>

            <div className='flex gap-4 items-center'>
              <Input
                type='text'
                endContent={<FaBook className='opacity-50' />}
                variant='underlined'
                label='Biography'
                labelPlacement='inside'
                value={biography}
                onChange={(e) => setBiography(e.target.value)}
              />
            </div>

            <div className='flex gap-4 items-center'>
              <Input
                type='password'
                endContent={<FaLock className='opacity-50' />}
                variant='underlined'
                label='Password'
                labelPlacement='inside'
                value={password}
                onChange={(e) => setPassword(e.target.value)}
              />
            </div>

            <Button type='submit' variant='flat' className='bg-opacity-70 font-semibold'>
              Sign Up
            </Button>

            <p className='text-gray-500 opacity-90'>
              already have an account?{' '}
              <Link className='text-primary opacity-80' to='/login'>
                Sign In
              </Link>{' '}
              instead
            </p>
          </form>
        </div>
      </div>
    </div>
  );
}

export default Register;
