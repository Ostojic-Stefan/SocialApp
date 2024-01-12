import { Link } from 'react-router-dom';
import { UserInfoResponse } from '../api/postService';
import ProfileImage from './ProfileImage';

interface UserInfoProps {
  userInfo: UserInfoResponse;
  children?: React.ReactNode;
  dimension?: number;
}

export default function UserInfo({ userInfo, children, dimension }: UserInfoProps) {
  return (
    <div className='flex gap-3'>
      <ProfileImage dimension={dimension ?? 50} src={userInfo.avatarUrl} />
      <div className='flex flex-col justify-center items-center'>
        <Link className='text-medium font-semibold' to={`/profile/${userInfo.username}`}>
          <p className='hover:text-secondary-400 hover:font-bold text-md'>{userInfo.username}</p>
        </Link>
        {children}
      </div>
    </div>
  );
}
