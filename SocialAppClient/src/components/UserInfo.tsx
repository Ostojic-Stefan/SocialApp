import { Link } from 'react-router-dom';
import ProfileImage from './ProfileImage';
import { UserInfoResponse } from '../api/dtos/user';

interface UserInfoProps {
  userInfo: UserInfoResponse;
  children?: React.ReactNode;
  dimension?: number;
}

export default function UserInfo({ userInfo, children, dimension }: UserInfoProps) {
  return (
    <div className='flex gap-3'>
      <ProfileImage dimension={dimension ?? 50} src={userInfo.profileImage.thumbnailImagePath} />
      <div className='flex flex-col justify-center items-center'>
        <Link className='text-medium font-semibold' to={`/profile/${userInfo.username}`}>
          <p className='hover:text-secondary-400 text-md'>{userInfo.username}</p>
        </Link>
        {children}
      </div>
    </div>
  );
}
