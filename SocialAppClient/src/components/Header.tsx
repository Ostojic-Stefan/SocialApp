import {
  Avatar,
  Badge,
  Button,
  Input,
  Navbar,
  NavbarBrand,
  NavbarContent,
  NavbarItem,
  Popover,
  PopoverContent,
  PopoverTrigger,
  Spinner,
} from '@nextui-org/react';
import { Link } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';
import { FaGlobe, FaUsers } from 'react-icons/fa';
import UserInfo from './UserInfo';

export default function Header() {
  const { user, logout } = useAuth();

  if (!user) {
    return <Spinner />;
  }

  const renderedNotifications = user.notifications.map((notification) => {
    if (notification.notificationType === 'Like') {
      const { like } = notification;
      return (
        <div key={notification.like.id}>
          <UserInfo userInfo={like.userInfo}>
            <p>
              liked the post: <Link to={`/post/${notification.postId}`}>Post</Link>
            </p>
          </UserInfo>
        </div>
      );
    } else if (notification.notificationType === 'Comment') {
      const { comment } = notification;
      return (
        <div key={notification.comment.id}>
          <UserInfo userInfo={comment.userInfo}>
            <p>
              Commented on post: <Link to={`/post/${notification.postId}`}>Post</Link>
            </p>
          </UserInfo>
        </div>
      );
    }
  });

  // TODO: make friend request return the id of the friend request
  const renderFriendRequests = user.friendRequests.map((fr, idx) => {
    return (
      <div key={idx}>
        <UserInfo userInfo={fr.requesterUser} />
      </div>
    );
  });

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
          <Badge content={user.friendRequests.length} color='secondary' isInvisible={user.friendRequests.length <= 0}>
            <Popover>
              <PopoverTrigger>
                <Button>
                  <FaUsers /> Friend Requests
                </Button>
              </PopoverTrigger>
              <PopoverContent>
                <div className='flex flex-col gap-5 max-h-96 overflow-y-scroll'>{renderFriendRequests}</div>
              </PopoverContent>
            </Popover>
          </Badge>
        </NavbarItem>
        <NavbarItem>
          <Badge content={user.notifications.length} color='secondary' isInvisible={user.notifications.length <= 0}>
            <Popover showArrow>
              <PopoverTrigger>
                <Button>
                  <FaGlobe /> Notifications
                </Button>
              </PopoverTrigger>
              <PopoverContent>
                <div className='flex flex-col gap-5 max-h-96 overflow-y-scroll'>{renderedNotifications}</div>
              </PopoverContent>
            </Popover>
          </Badge>
        </NavbarItem>
        <NavbarItem>
          <Popover placement='bottom' showArrow={true}>
            <PopoverTrigger>
              <Avatar />
            </PopoverTrigger>
            <PopoverContent>
              <div className='flex flex-col gap-2 w-full m-4'>
                <Link to={`/profile/${user.userInfo.username}`}>
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
