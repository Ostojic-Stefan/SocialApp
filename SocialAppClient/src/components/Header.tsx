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

  const numOfNotifications = user.notifications.commentsOnPost.length + user.notifications.likesOnPost.length;

  // TODO: fix bug where User's own like/comment apprears as a notification
  // TODO: all posts should have a title too
  const renderedCommentNotifications = user.notifications.commentsOnPost.map((comment) => {
    return (
      <div key={comment.commentId}>
        <UserInfo userInfo={comment.userInformation}>
          commented on post: <Link to={`/post/${comment.postResponse.id}`}>{comment.postResponse.contents}</Link>
        </UserInfo>
      </div>
    );
  });

  const renderedLikeNotifications = user.notifications.likesOnPost.map((like) => {
    return (
      <div key={like.likeId}>
        <UserInfo userInfo={like.userInformation}>
          liked the post: <Link to={`/post/${like.postResponse.id}`}>{like.postResponse.contents}</Link>
        </UserInfo>
      </div>
    );
  });

  const renderedNotifications = renderedCommentNotifications.concat(renderedLikeNotifications);

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
            <Button>
              <FaUsers /> Friend Requests
            </Button>
          </Badge>
        </NavbarItem>
        <NavbarItem>
          <Badge content={numOfNotifications} color='secondary' isInvisible={numOfNotifications <= 0}>
            <Popover showArrow>
              <PopoverTrigger>
                <Button>
                  <FaGlobe /> Notifications
                </Button>
              </PopoverTrigger>
              <PopoverContent>
                <div className='flex flex-col gap-5 h-96 overflow-y-scroll'>{renderedNotifications}</div>
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
