import { Link } from 'react-router-dom';
import ProfileImage from './ProfileImage';
import { UserInfoResponse } from '../api/dtos/user';
import { Button, Popover, PopoverContent, PopoverTrigger } from '@nextui-org/react';

interface UserInfoProps {
  userInfo: UserInfoResponse;
  children?: React.ReactNode;
  dimension?: number;
}

export default function UserInfo({ userInfo, children, dimension }: UserInfoProps) {
  return (
    <div className='flex flex-col justify-center'>
      <Popover>
        <PopoverTrigger>
          <div className='flex gap-2'>
            <ProfileImage dimension={dimension ?? 50} src={userInfo.profileImage.thumbnailImagePath} />
            <div className='flex flex-col  justify-center'>
              <p className='cursor-pointer hover:text-secondary-400 text-md'>{userInfo.username}</p>
              <div className='text-default-400'>{children}</div>
            </div>
          </div>
        </PopoverTrigger>
        <PopoverContent>
          <div className='flex flex-col gap-1 p-2 w-96'>
            <div className='flex items-center justify-between'>
              <div className='flex gap-2 items-center'>
                <ProfileImage dimension={dimension ?? 50} src={userInfo.profileImage.thumbnailImagePath} />
                <p className='text-xl font-semibold'>{userInfo.username}</p>
              </div>
              <Button variant='light' color='primary' as={Link} to={`/profile/${userInfo.username}`}>
                &rarr; Visit Profile
              </Button>
            </div>
            <div className='max-w-full'>{userInfo.biography}</div>
          </div>
        </PopoverContent>
      </Popover>
    </div>
  );
}
