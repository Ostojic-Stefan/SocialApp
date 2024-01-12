import {
  Avatar,
  Button,
  Input,
  Navbar,
  NavbarBrand,
  NavbarContent,
  NavbarItem,
  Popover,
  PopoverContent,
  PopoverTrigger,
} from '@nextui-org/react';
import { Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

export default function Header() {
  const { user, logout } = useAuth();
  return (
    <Navbar className='shadow'>
      <NavbarBrand>
        <Link to='/'>
          <p className='font-bold'>ChadBook</p>
        </Link>
      </NavbarBrand>
      <NavbarContent className='hidden sm:flex gap-4' justify='center'>
        <NavbarItem>
          <Input label='Search People' />
        </NavbarItem>
      </NavbarContent>
      <NavbarContent justify='end'>
        <NavbarItem>
          <Button>Notifications</Button>
        </NavbarItem>
        <NavbarItem>
          <Popover placement='bottom' showArrow={true}>
            <PopoverTrigger>
              <Avatar />
            </PopoverTrigger>
            <PopoverContent>
              <div className='flex flex-col gap-2 w-full m-4'>
                <Link to={`/profile/${user?.userInformation.username}`}>
                  <Button>Check Profile</Button>
                </Link>
                <Button onClick={() => logout()}>Sign Out</Button>
              </div>
            </PopoverContent>
          </Popover>
        </NavbarItem>
      </NavbarContent>
    </Navbar>
  );
}
