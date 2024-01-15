import { FormEvent, useState } from 'react';
import { useAuth } from '../context/AuthContext';
import { Button, Input } from '@nextui-org/react';
import { FaEnvelope, FaLock } from 'react-icons/fa';
import { Link } from 'react-router-dom';

function Login() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const { login } = useAuth();

  async function handleSubmit(event: FormEvent): Promise<void> {
    event.preventDefault();
    await login(email, password);
  }

  return (
    <div className='absolute top-0 left-0 right-0 bottom-0 h-screen bg-[url("/public/images/4850017.jpg")]'>
      <div className='h-screen w-screen flex items-center justify-center'>
        <div className='rounded-3xl bg-default bg-opacity-20 backdrop-blur-md shadow-xl flex flex-col gap-10 p-10'>
          <h1 className='text-black text-3xl font-semibold'>Sign In</h1>
          <form className='flex flex-col gap-10 w-96' onSubmit={(e) => handleSubmit(e)}>
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
              Sign In
            </Button>

            <p className='text-gray-500 opacity-90'>
              don't have an account?{' '}
              <Link className='text-primary opacity-80' to='/register'>
                Sign Up
              </Link>{' '}
              instead
            </p>
          </form>
        </div>
      </div>
    </div>
  );
}

export default Login;
